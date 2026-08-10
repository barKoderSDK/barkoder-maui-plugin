using BarkoderNET;
using Barkoder.Windows.Config;
using Barkoder.Windows.Models;
using System.Reflection;
using System.Text.Json;

using SdkDecoderType = Barkoder.Windows.Models.DecoderType;
using SdkDecodingSpeed = Barkoder.Windows.Models.DecodingSpeed;
using SdkFormattingType = Barkoder.Windows.Models.FormattingType;
using SdkPointF = Barkoder.Windows.Models.PointF;

namespace Barkoder.Windows.Decoder
{
    /// <summary>
    /// Barcode decoder that processes camera frames using the native BarkoderNET engine.
    /// </summary>
    internal class BarcodeDecoder : IDisposable
    {
        private BarkoderNET.Config? _nativeConfig;
        private readonly object _decodeLock = new();
        private bool _isDecoding;
        private bool _isDisposed;
        private bool _matchApiValidated;

        // Performance tracking
        private int _decodeCount;
        private DateTime _lastDpsTime = DateTime.UtcNow;
        private double _currentDps;

        public double CurrentDps => _currentDps;

        /// <summary>
        /// Whether the native decoder has been initialized.
        /// </summary>
        public bool IsInitialized => _nativeConfig != null;

        /// <summary>
        /// Initialize the native decoder with a license key.
        /// Must be called before Decode(). Returns error message if license is invalid (SDK works in demo mode).
        /// </summary>
        public string? Initialize(string licenseKey)
        {
            if (_nativeConfig != null)
            {
                _nativeConfig.Dispose();
                _nativeConfig = null;
            }

            var result = BarkoderNET.Config.InitializeWithLicenseKey(licenseKey);
            _nativeConfig = result.Configuration;
            _matchApiValidated = false;

            return result.Success ? null : result.ErrorMessage;
        }

        /// <summary>
        /// Apply SDK DecoderConfig to the native BarkoderNET.Config.
        /// </summary>
        public void ApplyConfig(DecoderConfig decoderConfig, BarkoderConfig? barkoderConfig = null)
        {
            if (_nativeConfig == null)
                throw new InvalidOperationException("Decoder not initialized. Call Initialize() first.");

            var matchApisRequired = IsMatchApiRequired(decoderConfig);
            if (!EnsureMatchApisAvailable(matchApisRequired))
            {
                // Match APIs are not available and not required for this configuration.
                // Continue applying non-match settings to avoid startup-time crashes.
                System.Diagnostics.Debug.WriteLine("[Barkoder] Match APIs unavailable; continuing without MatchFilter/ReturnOnlyMatchedResults.");
            }

            // Enable/disable decoders
            var enabledDecoders = decoderConfig.GetEnabledDecoders();
            var nativeDecoders = new List<BarkoderNET.DecoderType>();
            foreach (var dt in enabledDecoders)
            {
                var native = MapToNativeDecoderType(dt);
                if (native != null)
                    nativeDecoders.Add(native.Value);
            }
            System.Diagnostics.Debug.WriteLine($"[Barkoder] Enabling {nativeDecoders.Count} decoders: {string.Join(", ", nativeDecoders)}");
            _nativeConfig.SetEnabledDecoders(nativeDecoders);

            // Apply decoding speed
            _nativeConfig.DecodingSpeed = MapDecodingSpeed(decoderConfig.DecodingSpeed);

            // Apply formatting
            _nativeConfig.Formatting = MapFormatting(decoderConfig.Formatting);

            // Apply encoding
            if (!string.IsNullOrEmpty(decoderConfig.EncodingCharacterSet))
                _nativeConfig.EncodingCharacterSet = decoderConfig.EncodingCharacterSet;

            // Apply max results
            _nativeConfig.MaximumResultsCount = decoderConfig.MaximumResultsCount;

            // Apply misshaped 1D
            _nativeConfig.EnableMisshaped1D = decoderConfig.EnableMisshaped1D ? 1 : 0;

            // Apply UPC/EAN deblur
            _nativeConfig.UpcEanDeblur = decoderConfig.UpcEanDeblur ? 1 : 0;

            // Apply composite
            _nativeConfig.EnableComposite = decoderConfig.CompositeCodesEnabled ? 1 : 0;

            // Apply VIN restrictions
            _nativeConfig.EnableVINRestrictions = decoderConfig.EnableVINRestrictions ? 1 : 0;

            // Apply ROI center mark behavior through custom option.
            // POINT/CROSSHAIR enable center-only scanning; NONE disables it.
            var roiCenterMarkEnabled = decoderConfig.RoiCenterMark == BarkoderRoiCenterMark.Point ||
                                       decoderConfig.RoiCenterMark == BarkoderRoiCenterMark.Crosshair;
            try
            {
                _nativeConfig.SetCustomOption("center_only_scan", roiCenterMarkEnabled ? 1 : 0);
                System.Diagnostics.Debug.WriteLine(
                    $"[Barkoder] Applied center_only_scan={(roiCenterMarkEnabled ? 1 : 0)} (RoiCenterMark={decoderConfig.RoiCenterMark})");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Barkoder] Failed to apply center_only_scan: {ex.Message}");
            }

            // Apply match-filter settings when supported by the underlying BarkoderNET package.
            // Reflection keeps this wrapper compatible with older package versions.
            if (!SetNativeStringPropertyIfAvailable(_nativeConfig, "MatchFilter", decoderConfig.MatchFilter))
            {
                if (matchApisRequired)
                throw new MissingMemberException("BarkoderNET.Config.MatchFilter is required but not writable.");
                System.Diagnostics.Debug.WriteLine("[Barkoder] Skipping MatchFilter apply because native property is unavailable.");
            }

            LogNativePropertyValue(_nativeConfig, "MatchFilter");
            System.Diagnostics.Debug.WriteLine($"[Barkoder] Applied MatchFilter config: {decoderConfig.MatchFilter}");

            if (!SetNativeBoolPropertyIfAvailable(_nativeConfig, "ReturnOnlyMatchedResults", decoderConfig.ReturnOnlyMatchedResults))
            {
                if (matchApisRequired)
                throw new MissingMemberException("BarkoderNET.Config.ReturnOnlyMatchedResults is required but not writable as bool/int/nint.");
                System.Diagnostics.Debug.WriteLine("[Barkoder] Skipping ReturnOnlyMatchedResults apply because native property is unavailable.");
            }

            LogNativePropertyValue(_nativeConfig, "ReturnOnlyMatchedResults");
            System.Diagnostics.Debug.WriteLine($"[Barkoder] Applied ReturnOnlyMatchedResults config: {decoderConfig.ReturnOnlyMatchedResults}");

            // Apply duplicates delay
            _nativeConfig.DuplicatesDelayMs = decoderConfig.DuplicatesDelayMs;

            // Apply ROI from BarkoderConfig if provided
            if (barkoderConfig != null)
            {
                var roi = barkoderConfig.GetRegionOfInterest();
                _nativeConfig.RegionOfInterest = new BarkoderNET.Rect
                {
                    Left = roi.Left,
                    Top = roi.Top,
                    Width = roi.Width,
                    Height = roi.Height
                };
            }

            // Apply per-decoder configs
            ApplyPerDecoderConfigs(decoderConfig);

            // Apply global options from BarkoderConfig
            if (barkoderConfig != null)
            {
                _nativeConfig.SetGlobalOption(BKGlobalOption.SetMaximumThreads, barkoderConfig.ThreadsLimit);
                _nativeConfig.SetGlobalOption(BKGlobalOption.MulticodeCachingEnabled, barkoderConfig.MulticodeCachingEnabled ? 1 : 0);
                _nativeConfig.SetGlobalOption(BKGlobalOption.MulticodeCachingDuration, barkoderConfig.MulticodeCachingDuration);
            }
        }

        private static bool IsMatchApiRequired(DecoderConfig decoderConfig)
        {
            if (decoderConfig.ReturnOnlyMatchedResults)
                return true;

            return ParseMatchFilterItems(decoderConfig.MatchFilter).Length > 0;
        }

        private bool EnsureMatchApisAvailable(bool strict)
        {
            if (_matchApiValidated || _nativeConfig == null)
                return true;

            var barkoderAsm = typeof(BarkoderNET.Config).Assembly;
            var barkoderAsmVersion = barkoderAsm.GetName().Version;
            System.Diagnostics.Debug.WriteLine(
                $"[Barkoder] BarkoderNET assembly loaded: {barkoderAsm.FullName}, Version={barkoderAsmVersion}, Location={barkoderAsm.Location}");

            var minRequiredVersion = new Version(1, 7, 3, 0);
            if (barkoderAsmVersion == null || barkoderAsmVersion < minRequiredVersion)
            {
                if (!strict)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[Barkoder] Match APIs not available on loaded BarkoderNET version {barkoderAsmVersion}; strict mode is disabled.");
                    return false;
                }

                throw new MissingMemberException(
                    $"BarkoderNET.OCR 1.7.3+ is required for MatchFilter APIs. Loaded version: {barkoderAsmVersion}");
            }

            var configType = _nativeConfig.GetType();

            var matchFilterProp = configType.GetProperty("MatchFilter", BindingFlags.Public | BindingFlags.Instance);
            if (matchFilterProp == null || !matchFilterProp.CanWrite)
            {
                if (!strict)
                {
                    System.Diagnostics.Debug.WriteLine("[Barkoder] MatchFilter property missing in native config; strict mode is disabled.");
                    return false;
                }
                throw new MissingMemberException("BarkoderNET.Config.MatchFilter is missing. Use BarkoderNET.OCR 1.7.3+.");
            }

            bool matchFilterTypeSupported =
                matchFilterProp.PropertyType == typeof(string) ||
                matchFilterProp.PropertyType == typeof(string[]) ||
                matchFilterProp.PropertyType == typeof(List<string>) ||
                typeof(IEnumerable<string>).IsAssignableFrom(matchFilterProp.PropertyType);

            if (!matchFilterTypeSupported)
            {
                if (!strict)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[Barkoder] MatchFilter property has unsupported type '{matchFilterProp.PropertyType.FullName}'; strict mode is disabled.");
                    return false;
                }
                throw new MissingMemberException($"BarkoderNET.Config.MatchFilter has unsupported type '{matchFilterProp.PropertyType.FullName}'.");
            }

            System.Diagnostics.Debug.WriteLine(
                $"[Barkoder] Match API Config.MatchFilter property type: {matchFilterProp.PropertyType.FullName}");

            var returnOnlyMatchedProp = configType.GetProperty("ReturnOnlyMatchedResults", BindingFlags.Public | BindingFlags.Instance);
            if (returnOnlyMatchedProp == null || !returnOnlyMatchedProp.CanWrite)
            {
                if (!strict)
                {
                    System.Diagnostics.Debug.WriteLine("[Barkoder] ReturnOnlyMatchedResults property missing in native config; strict mode is disabled.");
                    return false;
                }
                throw new MissingMemberException("BarkoderNET.Config.ReturnOnlyMatchedResults is missing. Use BarkoderNET.OCR 1.7.3+.");
            }

            bool returnTypeSupported =
                returnOnlyMatchedProp.PropertyType == typeof(bool) ||
                returnOnlyMatchedProp.PropertyType == typeof(int) ||
                returnOnlyMatchedProp.PropertyType == typeof(nint);

            if (!returnTypeSupported)
            {
                if (!strict)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[Barkoder] ReturnOnlyMatchedResults property has unsupported type '{returnOnlyMatchedProp.PropertyType.FullName}'; strict mode is disabled.");
                    return false;
                }
                throw new MissingMemberException($"BarkoderNET.Config.ReturnOnlyMatchedResults has unsupported type '{returnOnlyMatchedProp.PropertyType.FullName}'.");
            }

            System.Diagnostics.Debug.WriteLine(
                $"[Barkoder] Match API Config.ReturnOnlyMatchedResults property type: {returnOnlyMatchedProp.PropertyType.FullName}");

            var isMatchedProp = typeof(BaseResult).GetProperty("IsMatched", BindingFlags.Public | BindingFlags.Instance);
            if (isMatchedProp == null || !isMatchedProp.CanRead)
            {
                if (!strict)
                {
                    System.Diagnostics.Debug.WriteLine("[Barkoder] BaseResult.IsMatched is missing; strict mode is disabled.");
                    return false;
                }
                throw new MissingMemberException("BarkoderNET.BaseResult.IsMatched is missing. Use BarkoderNET.OCR 1.7.3+.");
            }

            System.Diagnostics.Debug.WriteLine(
                $"[Barkoder] Match API BaseResult.IsMatched property type: {isMatchedProp.PropertyType.FullName}");

            _matchApiValidated = true;
            return true;
        }

        private static void LogNativePropertyValue(object target, string propertyName)
        {
            try
            {
                var prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (prop == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[Barkoder] Native property '{propertyName}' not found for read-back.");
                    return;
                }

                var value = prop.GetValue(target);
                string printable = value switch
                {
                    null => "<null>",
                    string s => s,
                    IEnumerable<string> seq => "[" + string.Join(", ", seq) + "]",
                    _ => value.ToString() ?? "<null>"
                };

                System.Diagnostics.Debug.WriteLine(
                    $"[Barkoder] Native property '{propertyName}' applied value: {printable} (Type={prop.PropertyType.FullName})");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Barkoder] Failed reading native property '{propertyName}': {ex.Message}");
            }
        }

        private static bool SetNativeStringPropertyIfAvailable(object target, string propertyName, string value)
        {
            var prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null || !prop.CanWrite)
                return false;

            var text = value ?? string.Empty;

            // Most common case.
            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(target, text);
                return true;
            }

            // Newer native builds may expose MatchFilter as a string collection.
            var items = ParseMatchFilterItems(text);

            if (prop.PropertyType == typeof(string[]))
            {
                prop.SetValue(target, items);
                return true;
            }

            if (prop.PropertyType == typeof(List<string>))
            {
                prop.SetValue(target, items.ToList());
                return true;
            }

            // Try interface assignment if the concrete type accepts IEnumerable<string>/ICollection<string>.
            if (typeof(IEnumerable<string>).IsAssignableFrom(prop.PropertyType))
            {
                prop.SetValue(target, items);
                return true;
            }

            return false;
        }

        private static string[] ParseMatchFilterItems(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return Array.Empty<string>();

            // Expected format: JSON array (e.g. ["978...","123..."]).
            try
            {
                var parsed = JsonSerializer.Deserialize<string[]>(raw);
                if (parsed != null)
                    return parsed.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            }
            catch
            {
                // Fall back to delimiter-based parse.
            }

            var trimmed = raw.Trim();
            if (trimmed.StartsWith("[", StringComparison.Ordinal) && trimmed.EndsWith("]", StringComparison.Ordinal))
                trimmed = trimmed[1..^1];

            return trimmed
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim().Trim('"', '\''))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();
        }

        private static bool SetNativeBoolPropertyIfAvailable(object target, string propertyName, bool value)
        {
            var prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null || !prop.CanWrite)
                return false;

            if (prop.PropertyType == typeof(bool))
            {
                prop.SetValue(target, value);
                return true;
            }

            if (prop.PropertyType == typeof(int))
            {
                prop.SetValue(target, value ? 1 : 0);
                return true;
            }

            if (prop.PropertyType == typeof(nint))
            {
                prop.SetValue(target, (nint)(value ? 1 : 0));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to decode barcodes from a BGRA8 camera frame.
        /// Returns null if no barcode found or if already busy decoding.
        /// </summary>
        public BarcodeResult[]? Decode(byte[] bgra8Data, int width, int height)
        {
            if (_nativeConfig == null) return null;

            // Skip if already decoding (non-blocking)
            if (_isDecoding) return null;

            lock (_decodeLock)
            {
                if (_isDecoding) return null;
                _isDecoding = true;
            }

            try
            {
                // Track DPS
                _decodeCount++;
                var now = DateTime.UtcNow;
                var elapsed = (now - _lastDpsTime).TotalSeconds;
                if (elapsed >= 1.0)
                {
                    _currentDps = _decodeCount / elapsed;
                    _decodeCount = 0;
                    _lastDpsTime = now;
                }

                var nativeResults = BarkoderNET.Barkoder.DecodeImageMemory(
                    _nativeConfig, bgra8Data, width, height, BKColorFormat.BGRA);

                if (nativeResults == null || nativeResults.Count == 0) return null;

                // Suppress GC finalizer to prevent native crash in Result_Destroy
                foreach (var nr in nativeResults)
                    GC.SuppressFinalize(nr);

                var results = nativeResults.Select(MapFromNativeResult).ToArray();

                return results;
            }
            finally
            {
                lock (_decodeLock)
                {
                    _isDecoding = false;
                }
            }
        }

        /// <summary>
        /// Decode barcodes from raw grayscale (luminance) data.
        /// </summary>
        public BarcodeResult[]? DecodeGrayscale(byte[] grayscale, int width, int height)
        {
            if (_nativeConfig == null) return null;

            var nativeResults = BarkoderNET.Barkoder.DecodeImageMemory(
                _nativeConfig, grayscale, width, height, BKColorFormat.Grayscale);

            if (nativeResults == null || nativeResults.Count == 0) return null;

            // Suppress GC finalizer to prevent native crash in Result_Destroy
            foreach (var nr in nativeResults)
                GC.SuppressFinalize(nr);

            var results = nativeResults.Select(MapFromNativeResult).ToArray();

            return results;
        }

        /// <summary>
        /// Get the native BarkoderNET library version.
        /// </summary>
        public static string GetLibVersion() => BarkoderNET.Barkoder.GetLibVersion();

        /// <summary>
        /// Check if the native decoder is currently busy.
        /// </summary>
        public static bool IsDecoderBusy() => BarkoderNET.Barkoder.IsDecoderBusy();

        /// <summary>
        /// Check if a string is VIN compliant.
        /// </summary>
        public static bool IsStringVINCompliant(string vinString, int checkLevel = 1)
            => BarkoderNET.Barkoder.IsStringVINCompliant(vinString, checkLevel);

        /// <summary>
        /// Get the device ID from the native decoder (for licensing).
        /// </summary>
        public string? GetDeviceId() => _nativeConfig != null ? BarkoderNET.Config.GetDeviceId() : null;

        /// <summary>
        /// Get license info string.
        /// </summary>
        public string? GetLicenseInfo() => _nativeConfig != null ? BarkoderNET.Config.GetLicenseInfo() : null;

        /// <summary>
        /// Get license error message if any.
        /// </summary>
        public string? GetLicenseErrorMessage() => _nativeConfig != null ? BarkoderNET.Config.GetLicenseErrorMessage() : null;

        /// <summary>
        /// Set a custom native option by name.
        /// </summary>
        public int SetCustomOption(string option, int value)
        {
            if (_nativeConfig == null)
                throw new InvalidOperationException("Decoder not initialized.");
            return _nativeConfig.SetCustomOption(option, value);
        }

        /// <summary>
        /// Get a custom native option by name.
        /// </summary>
        public int GetCustomOption(string option)
        {
            if (_nativeConfig == null)
                throw new InvalidOperationException("Decoder not initialized.");
            return _nativeConfig.GetCustomOption(option);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _nativeConfig?.Dispose();
                _nativeConfig = null;
            }
        }

        // --- Mapping helpers ---

        internal static BarcodeResult MapFromNativeResult(BaseResult nr)
        {
            var result = new BarcodeResult
            {
                TextualData = nr.TextualData ?? "",
                BarcodeTypeName = nr.BarcodeTypeName ?? "",
                Extra = new Dictionary<string, string>()
            };

            try { result.BarcodeType = MapFromNativeDecoderType(nr.DecoderType); } catch { }
            try { result.IsGS1 = nr.IsGS1; } catch { }
            try { result.HasLinkage = nr.HasLinkage; } catch { }
            try { result.IsCached = nr.IsCached; } catch { }
            try { result.TimestampMs = nr.TimestampMs; } catch { }
            try { result.BinaryData = nr.BinaryData; } catch { }
            try { result.CharacterSet = nr.CharacterSet ?? ""; } catch { }

            // Match-filter state may not exist on older BarkoderNET builds.
            try
            {
                var isMatchedProp = nr.GetType().GetProperty("IsMatched");
                if (isMatchedProp != null)
                {
                    var raw = isMatchedProp.GetValue(nr);
                    if (raw is bool b)
                        result.IsMatched = b;
                    else if (raw is int i)
                        result.IsMatched = i != 0;
                    else if (raw is long l)
                        result.IsMatched = l != 0;
                    else if (raw is string s && bool.TryParse(s, out var parsed))
                        result.IsMatched = parsed;
                }
            }
            catch { }

            // Extra data (GS1, ID document fields, etc.)
            try
            {
                var extra = nr.GetExtraData();
                if (extra != null)
                {
                    result.Extra = extra;

                    // Fallback for older/native variants that expose match state via extras.
                    if (!result.IsMatched)
                    {
                        if (extra.TryGetValue("isMatched", out var matched) ||
                            extra.TryGetValue("matched", out matched) ||
                            extra.TryGetValue("is_matched", out matched))
                        {
                            if (bool.TryParse(matched, out var matchedBool))
                                result.IsMatched = matchedBool;
                            else if (int.TryParse(matched, out var matchedInt))
                                result.IsMatched = matchedInt != 0;
                        }

                        if (!result.IsMatched)
                        {
                            foreach (var kv in extra)
                            {
                                if (kv.Key.IndexOf("match", StringComparison.OrdinalIgnoreCase) < 0)
                                    continue;

                                var value = kv.Value;
                                if (bool.TryParse(value, out var boolVal))
                                {
                                    result.IsMatched = boolVal;
                                    break;
                                }

                                if (int.TryParse(value, out var intVal))
                                {
                                    result.IsMatched = intVal != 0;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            // Location — BKPoint[4] corners (available since BarkoderNET 1.7.1)
            try
            {
                var loc = nr.Location;
                if (loc != null && loc.Length >= 4)
                {
                    result.Location = new BarcodeLocation(
                        new SdkPointF(loc[0].X, loc[0].Y),
                        new SdkPointF(loc[1].X, loc[1].Y),
                        new SdkPointF(loc[2].X, loc[2].Y),
                        new SdkPointF(loc[3].X, loc[3].Y));

                    // Center from corners
                    result.LocationCenter = new SdkPointF(
                        (loc[0].X + loc[1].X + loc[2].X + loc[3].X) / 4f,
                        (loc[0].Y + loc[1].Y + loc[2].Y + loc[3].Y) / 4f);
                }
            }
            catch { }

            // MRZ zone locations — use native GetLocationsFromExtras (available since BarkoderNET.OCR 1.7.2)
            try
            {
                var nativeLocations = nr.GetLocationsFromExtras();
                if (nativeLocations != null && nativeLocations.Count > 0)
                {
                    result.MrzLocations = new List<(string, BarcodeLocation)>();
                    foreach (var bkLoc in nativeLocations)
                    {
                        if (bkLoc.Points != null && bkLoc.Points.Length >= 4)
                        {
                            var sdkLoc = new BarcodeLocation(
                                new SdkPointF(bkLoc.Points[0].X, bkLoc.Points[0].Y),
                                new SdkPointF(bkLoc.Points[1].X, bkLoc.Points[1].Y),
                                new SdkPointF(bkLoc.Points[2].X, bkLoc.Points[2].Y),
                                new SdkPointF(bkLoc.Points[3].X, bkLoc.Points[3].Y));
                            result.MrzLocations.Add((bkLoc.LocationName ?? "", sdkLoc));
                        }
                    }
                }
            }
            catch { }

            return result;
        }

        internal static BarkoderNET.DecoderType? MapToNativeDecoderType(SdkDecoderType type) => type switch
        {
            SdkDecoderType.Aztec => BarkoderNET.DecoderType.Aztec,
            SdkDecoderType.AztecCompact => BarkoderNET.DecoderType.AztecCompact,
            SdkDecoderType.QR => BarkoderNET.DecoderType.QR,
            SdkDecoderType.QRMicro => BarkoderNET.DecoderType.QRMicro,
            SdkDecoderType.Code128 => BarkoderNET.DecoderType.Code128,
            SdkDecoderType.Code93 => BarkoderNET.DecoderType.Code93,
            SdkDecoderType.Code39 => BarkoderNET.DecoderType.Code39,
            SdkDecoderType.Codabar => BarkoderNET.DecoderType.Codabar,
            SdkDecoderType.Code11 => BarkoderNET.DecoderType.Code11,
            SdkDecoderType.Msi => BarkoderNET.DecoderType.Msi,
            SdkDecoderType.UPCa => BarkoderNET.DecoderType.UpcA,
            SdkDecoderType.UPCe => BarkoderNET.DecoderType.UpcE,
            SdkDecoderType.UPCe1 => BarkoderNET.DecoderType.UpcE1,
            SdkDecoderType.EAN13 => BarkoderNET.DecoderType.Ean13,
            SdkDecoderType.EAN8 => BarkoderNET.DecoderType.Ean8,
            SdkDecoderType.PDF417 => BarkoderNET.DecoderType.PDF417,
            SdkDecoderType.PDF417Micro => BarkoderNET.DecoderType.PDF417Micro,
            SdkDecoderType.DataMatrix => BarkoderNET.DecoderType.Datamatrix,
            SdkDecoderType.Code25 => BarkoderNET.DecoderType.Code25,
            SdkDecoderType.Interleaved2of5 => BarkoderNET.DecoderType.Interleaved25,
            SdkDecoderType.ITF14 => BarkoderNET.DecoderType.ITF14,
            SdkDecoderType.IATA25 => BarkoderNET.DecoderType.IATA25,
            SdkDecoderType.Matrix25 => BarkoderNET.DecoderType.Matrix25,
            SdkDecoderType.Datalogic25 => BarkoderNET.DecoderType.Datalogic25,
            SdkDecoderType.COOP25 => BarkoderNET.DecoderType.COOP25,
            SdkDecoderType.Code32 => BarkoderNET.DecoderType.Code32,
            SdkDecoderType.Telepen => BarkoderNET.DecoderType.Telepen,
            SdkDecoderType.DotCode => BarkoderNET.DecoderType.Dotcode,
            SdkDecoderType.IDDocument => BarkoderNET.DecoderType.IDDocument,
            SdkDecoderType.Databar14 => BarkoderNET.DecoderType.Databar14,
            SdkDecoderType.DatabarLimited => BarkoderNET.DecoderType.DatabarLimited,
            SdkDecoderType.DatabarExpanded => BarkoderNET.DecoderType.DatabarExpanded,
            SdkDecoderType.PostalIMB => BarkoderNET.DecoderType.PostalIMB,
            SdkDecoderType.Postnet => BarkoderNET.DecoderType.Postnet,
            SdkDecoderType.Planet => BarkoderNET.DecoderType.Planet,
            SdkDecoderType.AustralianPost => BarkoderNET.DecoderType.AustralianPost,
            SdkDecoderType.RoyalMail => BarkoderNET.DecoderType.RoyalMail,
            SdkDecoderType.KIX => BarkoderNET.DecoderType.KIX,
            SdkDecoderType.JapanesePost => BarkoderNET.DecoderType.JapanesePost,
            SdkDecoderType.MaxiCode => BarkoderNET.DecoderType.MaxiCode,
            SdkDecoderType.OCRText => BarkoderNET.DecoderType.OCRText,
            _ => null
        };

        internal static SdkDecoderType MapFromNativeDecoderType(BarkoderNET.DecoderType type) => type switch
        {
            BarkoderNET.DecoderType.Aztec => SdkDecoderType.Aztec,
            BarkoderNET.DecoderType.AztecCompact => SdkDecoderType.AztecCompact,
            BarkoderNET.DecoderType.QR => SdkDecoderType.QR,
            BarkoderNET.DecoderType.QRMicro => SdkDecoderType.QRMicro,
            BarkoderNET.DecoderType.Code128 => SdkDecoderType.Code128,
            BarkoderNET.DecoderType.Code93 => SdkDecoderType.Code93,
            BarkoderNET.DecoderType.Code39 => SdkDecoderType.Code39,
            BarkoderNET.DecoderType.Codabar => SdkDecoderType.Codabar,
            BarkoderNET.DecoderType.Code11 => SdkDecoderType.Code11,
            BarkoderNET.DecoderType.Msi => SdkDecoderType.Msi,
            BarkoderNET.DecoderType.UpcA => SdkDecoderType.UPCa,
            BarkoderNET.DecoderType.UpcE => SdkDecoderType.UPCe,
            BarkoderNET.DecoderType.UpcE1 => SdkDecoderType.UPCe1,
            BarkoderNET.DecoderType.Ean13 => SdkDecoderType.EAN13,
            BarkoderNET.DecoderType.Ean8 => SdkDecoderType.EAN8,
            BarkoderNET.DecoderType.PDF417 => SdkDecoderType.PDF417,
            BarkoderNET.DecoderType.PDF417Micro => SdkDecoderType.PDF417Micro,
            BarkoderNET.DecoderType.Datamatrix => SdkDecoderType.DataMatrix,
            BarkoderNET.DecoderType.Code25 => SdkDecoderType.Code25,
            BarkoderNET.DecoderType.Interleaved25 => SdkDecoderType.Interleaved2of5,
            BarkoderNET.DecoderType.ITF14 => SdkDecoderType.ITF14,
            BarkoderNET.DecoderType.IATA25 => SdkDecoderType.IATA25,
            BarkoderNET.DecoderType.Matrix25 => SdkDecoderType.Matrix25,
            BarkoderNET.DecoderType.Datalogic25 => SdkDecoderType.Datalogic25,
            BarkoderNET.DecoderType.COOP25 => SdkDecoderType.COOP25,
            BarkoderNET.DecoderType.Code32 => SdkDecoderType.Code32,
            BarkoderNET.DecoderType.Telepen => SdkDecoderType.Telepen,
            BarkoderNET.DecoderType.Dotcode => SdkDecoderType.DotCode,
            BarkoderNET.DecoderType.IDDocument => SdkDecoderType.IDDocument,
            BarkoderNET.DecoderType.Databar14 => SdkDecoderType.Databar14,
            BarkoderNET.DecoderType.DatabarLimited => SdkDecoderType.DatabarLimited,
            BarkoderNET.DecoderType.DatabarExpanded => SdkDecoderType.DatabarExpanded,
            BarkoderNET.DecoderType.PostalIMB => SdkDecoderType.PostalIMB,
            BarkoderNET.DecoderType.Postnet => SdkDecoderType.Postnet,
            BarkoderNET.DecoderType.Planet => SdkDecoderType.Planet,
            BarkoderNET.DecoderType.AustralianPost => SdkDecoderType.AustralianPost,
            BarkoderNET.DecoderType.RoyalMail => SdkDecoderType.RoyalMail,
            BarkoderNET.DecoderType.KIX => SdkDecoderType.KIX,
            BarkoderNET.DecoderType.JapanesePost => SdkDecoderType.JapanesePost,
            BarkoderNET.DecoderType.MaxiCode => SdkDecoderType.MaxiCode,
            BarkoderNET.DecoderType.OCRText => SdkDecoderType.OCRText,
            _ => SdkDecoderType.QR
        };

        internal static BarkoderNET.DecodingSpeed MapDecodingSpeed(SdkDecodingSpeed speed) => speed switch
        {
            SdkDecodingSpeed.Fast => BarkoderNET.DecodingSpeed.Fast,
            SdkDecodingSpeed.Normal => BarkoderNET.DecodingSpeed.Normal,
            SdkDecodingSpeed.Slow => BarkoderNET.DecodingSpeed.Slow,
            SdkDecodingSpeed.Rigorous => BarkoderNET.DecodingSpeed.Rigorous,
            _ => BarkoderNET.DecodingSpeed.Normal
        };

        internal static BarkoderNET.Formatting MapFormatting(SdkFormattingType formatting) => formatting switch
        {
            SdkFormattingType.Disabled => BarkoderNET.Formatting.Disabled,
            SdkFormattingType.Automatic => BarkoderNET.Formatting.Automatic,
            SdkFormattingType.GS1 => BarkoderNET.Formatting.GS1,
            SdkFormattingType.AAMVA => BarkoderNET.Formatting.AAMVA,
            SdkFormattingType.SADL => BarkoderNET.Formatting.SADL,
            SdkFormattingType.BCBP => BarkoderNET.Formatting.BCBP,
            _ => BarkoderNET.Formatting.Automatic
        };

        private void ApplyPerDecoderConfigs(DecoderConfig config)
        {
            if (_nativeConfig == null) return;
            ApplyPerDecoderConfigs(_nativeConfig, config);
        }

        internal static void ApplyPerDecoderConfigs(BarkoderNET.Config nativeConfig, DecoderConfig config)
        {
            // QR
            nativeConfig.QR.DpmMode = config.QrDpmMode;
            nativeConfig.QR.MultiPartMerge = config.QrMultiPartMerge;

            // QR Micro
            nativeConfig.QRMicro.DpmMode = config.QrMicroDpmMode;

            // DataMatrix
            nativeConfig.Datamatrix.DpmMode = config.DataMatrixDpmMode;

            // Code39
            nativeConfig.Code39.Checksum = (BarkoderNET.Code39Config.ChecksumType)(int)config.Code39Checksum;

            // Code11
            nativeConfig.Code11.Checksum = (BarkoderNET.Code11Config.ChecksumType)(int)config.Code11Checksum;

            // MSI
            nativeConfig.Msi.Checksum = (BarkoderNET.MsiConfig.ChecksumType)(int)config.MsiChecksum;

            // UPC-E
            nativeConfig.UpcE.ExpandToUPCA = config.UpcEExpandToUPCA;

            // UPC-E1
            nativeConfig.UpcE1.ExpandToUPCA = config.UpcE1ExpandToUPCA;
        }

    }
}
