using Barkoder.Windows.Config;
using Barkoder.Windows.Models;

namespace Barkoder.Windows.UI
{
    /// <summary>
    /// Manages AR barcode result caching, position interpolation, and selection state.
    /// Analogous to BarkoderARManager (iOS) / LocationAROverlay cache (Android).
    /// </summary>
    internal class BarkoderARManager
    {
        private readonly Dictionary<string, ARCacheEntry> _cache = new();
        private readonly object _lock = new();

        /// <summary>
        /// Update the cache with new decode results. Existing entries are updated;
        /// new entries are added; stale entries are faded out (but kept in cache).
        /// Returns the list of keys for newly added entries that were auto-selected.
        /// </summary>
        public List<string> UpdateResults(BarcodeResult[] results, BarkoderARConfig arConfig)
        {
            var newlySelected = new List<string>();

            lock (_lock)
            {
                var now = DateTime.UtcNow;
                var seenKeys = new HashSet<string>();

                foreach (var result in results)
                {
                    if (result.Location == null) continue;

                    var key = $"{result.BarcodeTypeName}:{result.TextualData}";
                    seenKeys.Add(key);

                    var targetLoc = ComputeOverlayRect(result.Location, arConfig.LocationType);

                    if (_cache.TryGetValue(key, out var entry))
                    {
                        // Already in cache — update location/time only, no re-select
                        entry.TargetLocation = targetLoc;
                        entry.LastDetectedTime = now;
                        entry.IsFadingOut = false;
                        entry.Alpha = 1.0f;
                        entry.IsVisible = true;
                        entry.Result = result;
                        entry.BarcodeTypeName = result.BarcodeTypeName;

                        // In MatchFilter mode, selection always follows native match state:
                        // matched => selected (green), non-matched => non-selected (red).
                        if (arConfig.ARMode == BarkoderARMode.MatchFilter)
                        {
                            if (result.IsMatched)
                            {
                                if (!entry.IsSelected)
                                {
                                    bool canSelect = true;
                                    if (arConfig.ResultLimit > 0)
                                    {
                                        int selectedCount = _cache.Values.Count(e => e.IsSelected);
                                        canSelect = selectedCount < arConfig.ResultLimit;
                                    }

                                    if (canSelect)
                                    {
                                        entry.IsSelected = true;
                                        entry.SelectedAt = now;
                                        newlySelected.Add(key);
                                    }
                                }
                            }
                            else if (entry.IsSelected)
                            {
                                entry.IsSelected = false;
                                entry.SelectedAt = null;
                                entry.WasThumbnailSent = false;
                            }
                        }
                    }
                    else
                    {
                        // New barcode — auto-select based on mode
                        bool autoSelect = arConfig.ARMode == BarkoderARMode.InteractiveEnabled ||
                                          arConfig.ARMode == BarkoderARMode.NonInteractive ||
                                          (arConfig.ARMode == BarkoderARMode.MatchFilter && result.IsMatched);

                        // Check result limit
                        if (autoSelect && arConfig.ResultLimit > 0)
                        {
                            int selectedCount = _cache.Values.Count(e => e.IsSelected);
                            if (selectedCount >= arConfig.ResultLimit)
                                autoSelect = false;
                        }

                        var newEntry = new ARCacheEntry
                        {
                            Key = key,
                            Result = result,
                            TextualData = result.TextualData,
                            BarcodeTypeName = result.BarcodeTypeName,
                            CurrentLocation = targetLoc,
                            TargetLocation = targetLoc,
                            LastDetectedTime = now,
                            IsSelected = autoSelect,
                            SelectedAt = autoSelect ? now : null,
                            Alpha = 1.0f,
                            IsFadingOut = false,
                            IsVisible = true,
                            WasThumbnailSent = false
                        };
                        _cache[key] = newEntry;

                        if (autoSelect)
                            newlySelected.Add(key);
                    }
                }

                // Fade out unseen entries (but keep them in cache for re-detection)
                var disappearDelay = TimeSpan.FromMilliseconds(arConfig.ResultDisappearanceDelayMs);

                foreach (var kvp in _cache)
                {
                    if (!seenKeys.Contains(kvp.Key))
                    {
                        var elapsed = now - kvp.Value.LastDetectedTime;
                        if (elapsed > disappearDelay)
                        {
                            kvp.Value.IsVisible = false;
                            kvp.Value.IsFadingOut = true;
                            kvp.Value.Alpha = Math.Max(0, kvp.Value.Alpha - 0.05f);
                        }
                    }
                }
            }

            return newlySelected;
        }

        /// <summary>
        /// Interpolate current positions toward target positions for smooth transitions.
        /// </summary>
        public void InterpolatePositions(float speed)
        {
            lock (_lock)
            {
                foreach (var entry in _cache.Values)
                {
                    entry.CurrentLocation = Lerp(entry.CurrentLocation, entry.TargetLocation, speed);
                }
            }
        }

        /// <summary>
        /// Toggle selection state for a barcode by key.
        /// Returns: (found, nowSelected) — whether the entry was found and its new selection state.
        /// </summary>
        public (bool Found, bool NowSelected) ToggleSelection(string key, BarkoderARConfig arConfig)
        {
            lock (_lock)
            {
                if (!_cache.TryGetValue(key, out var entry))
                    return (false, false);

                if (entry.IsSelected)
                {
                    // Deselect
                    entry.IsSelected = false;
                    entry.SelectedAt = null;
                    entry.WasThumbnailSent = false;
                    return (true, false);
                }
                else
                {
                    // Check result limit before selecting
                    if (arConfig.ResultLimit > 0)
                    {
                        int selectedCount = _cache.Values.Count(e => e.IsSelected);
                        if (selectedCount >= arConfig.ResultLimit)
                            return (true, false); // At cap, can't select
                    }

                    // Select
                    entry.IsSelected = true;
                    entry.SelectedAt = DateTime.UtcNow;
                    return (true, true);
                }
            }
        }

        /// <summary>
        /// Find which cached barcode (if any) contains the given point.
        /// Returns the cache key, or null.
        /// </summary>
        public string? HitTest(float x, float y)
        {
            lock (_lock)
            {
                foreach (var entry in _cache.Values)
                {
                    var r = entry.CurrentLocation;
                    if (x >= r.Left && x <= r.Right && y >= r.Top && y <= r.Bottom)
                        return entry.Key;
                }
                return null;
            }
        }

        /// <summary>
        /// Get a snapshot of all visible cache entries for rendering.
        /// </summary>
        public ARCacheEntry[] GetSnapshot()
        {
            lock (_lock)
            {
                return _cache.Values.Where(e => e.Alpha > 0).ToArray();
            }
        }

        /// <summary>
        /// Get a snapshot of visible entries, optionally limited to matched results.
        /// </summary>
        public ARCacheEntry[] GetSnapshot(bool displayOnlyMatchedResults)
        {
            if (!displayOnlyMatchedResults)
                return GetSnapshot();

            lock (_lock)
            {
                return _cache.Values
                    .Where(e => e.Alpha > 0 && e.Result?.IsMatched == true)
                    .ToArray();
            }
        }

        /// <summary>
        /// Get all selected results, sorted by selection time.
        /// </summary>
        public BarcodeResult[] GetSelectedResults()
        {
            lock (_lock)
            {
                return _cache.Values
                    .Where(e => e.IsSelected && e.Result != null)
                    .OrderBy(e => e.SelectedAt)
                    .Select(e => e.Result!)
                    .ToArray();
            }
        }

        /// <summary>
        /// Number of currently selected entries.
        /// </summary>
        public int SelectedCount
        {
            get { lock (_lock) return _cache.Values.Count(e => e.IsSelected); }
        }

        /// <summary>
        /// Clear the entire cache (session reset).
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _cache.Clear();
            }
        }

        /// <summary>
        /// Number of cached entries.
        /// </summary>
        public int Count
        {
            get { lock (_lock) return _cache.Count; }
        }

        private static AROverlayRect ComputeOverlayRect(BarcodeLocation loc, BarkoderARLocationType type)
        {
            // Always compute proper bounding box from all 4 corners
            float minX = Math.Min(Math.Min(loc.TopLeft.X, loc.TopRight.X), Math.Min(loc.BottomLeft.X, loc.BottomRight.X));
            float maxX = Math.Max(Math.Max(loc.TopLeft.X, loc.TopRight.X), Math.Max(loc.BottomLeft.X, loc.BottomRight.X));
            float minY = Math.Min(Math.Min(loc.TopLeft.Y, loc.TopRight.Y), Math.Min(loc.BottomLeft.Y, loc.BottomRight.Y));
            float maxY = Math.Max(Math.Max(loc.TopLeft.Y, loc.TopRight.Y), Math.Max(loc.BottomLeft.Y, loc.BottomRight.Y));

            // Always store the polygon points so header can follow the top edge
            return new AROverlayRect(minX, minY, maxX, maxY,
                loc.TopLeft, loc.TopRight, loc.BottomRight, loc.BottomLeft);
        }

        private static AROverlayRect Lerp(AROverlayRect current, AROverlayRect target, float t)
        {
            t = Math.Clamp(t, 0f, 1f);
            float left = current.Left + (target.Left - current.Left) * t;
            float top = current.Top + (target.Top - current.Top) * t;
            float right = current.Right + (target.Right - current.Right) * t;
            float bottom = current.Bottom + (target.Bottom - current.Bottom) * t;

            if (target.HasPolygon && current.HasPolygon)
            {
                return new AROverlayRect(left, top, right, bottom,
                    LerpPoint(current.P0!, target.P0!, t),
                    LerpPoint(current.P1!, target.P1!, t),
                    LerpPoint(current.P2!, target.P2!, t),
                    LerpPoint(current.P3!, target.P3!, t));
            }

            return new AROverlayRect(left, top, right, bottom);
        }

        private static Models.PointF LerpPoint(Models.PointF a, Models.PointF b, float t)
        {
            return new Models.PointF(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t);
        }
    }

    /// <summary>
    /// Overlay rectangle for an AR barcode entry.
    /// For BoundingBox mode, Left/Top/Right/Bottom define the rect.
    /// For Tight mode, P0-P3 contain the polygon points.
    /// </summary>
    internal class AROverlayRect
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

        // Polygon points (for Tight mode)
        public Models.PointF? P0 { get; set; }
        public Models.PointF? P1 { get; set; }
        public Models.PointF? P2 { get; set; }
        public Models.PointF? P3 { get; set; }

        public bool HasPolygon => P0 != null;

        public AROverlayRect(float left, float top, float right, float bottom)
        {
            Left = left; Top = top; Right = right; Bottom = bottom;
        }

        public AROverlayRect(float left, float top, float right, float bottom,
            Models.PointF p0, Models.PointF p1, Models.PointF p2, Models.PointF p3)
            : this(left, top, right, bottom)
        {
            P0 = p0; P1 = p1; P2 = p2; P3 = p3;
        }
    }

    /// <summary>
    /// A cached AR barcode entry with position, selection state, and fade animation.
    /// </summary>
    internal class ARCacheEntry
    {
        public string Key { get; set; } = "";
        public BarcodeResult? Result { get; set; }
        public string TextualData { get; set; } = "";
        public string BarcodeTypeName { get; set; } = "";
        public AROverlayRect CurrentLocation { get; set; } = new(0, 0, 0, 0);
        public AROverlayRect TargetLocation { get; set; } = new(0, 0, 0, 0);
        public DateTime LastDetectedTime { get; set; }
        public DateTime? SelectedAt { get; set; }
        public bool IsSelected { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool WasThumbnailSent { get; set; }
        public float Alpha { get; set; } = 1.0f;
        public bool IsFadingOut { get; set; }
    }
}
