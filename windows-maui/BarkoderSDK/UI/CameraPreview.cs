using Barkoder.Windows.Config;
using Barkoder.Windows.Models;

namespace Barkoder.Windows.UI
{
    /// <summary>
    /// Cross-platform MAUI view that displays a live camera feed
    /// with optional ROI and barcode location overlays.
    /// </summary>
    public class CameraPreview : View
    {
        /// <summary>
        /// Current overlay configuration (ROI rect, location polygons, colors).
        /// </summary>
        public OverlayData? Overlay { get; set; }

        /// <summary>
        /// Update the preview with a BGRA8 frame.
        /// </summary>
        public void UpdateFrame(byte[] bgra8Data, int width, int height)
        {
            Handler?.Invoke(nameof(UpdateFrame), new FrameData(bgra8Data, width, height, Overlay));
        }
    }

    /// <summary>
    /// Carries a single camera frame plus overlay info to the platform handler.
    /// </summary>
    public record FrameData(byte[] Pixels, int Width, int Height, OverlayData? Overlay);

    /// <summary>
    /// Data for drawing overlays on the camera preview.
    /// </summary>
    public class OverlayData
    {
        /// <summary>ROI rectangle as percentages (0-100).</summary>
        public (float Left, float Top, float Width, float Height)? Roi { get; set; }

        /// <summary>
        /// ROI center-mark mode. POINT/CROSSHAIR renders a visible center marker
        /// and corresponds to center-only scanning behavior.
        /// </summary>
        public BarkoderRoiCenterMark RoiCenterMark { get; set; } = BarkoderRoiCenterMark.None;

        /// <summary>Whether to draw the ROI rectangle.</summary>
        public bool RoiVisible { get; set; }

        /// <summary>ROI border color (BGRA). Default: semi-transparent red (#80FF0000).</summary>
        public (byte B, byte G, byte R, byte A) RoiColor { get; set; } = (0, 0, 255, 128);

        /// <summary>ROI overlay dim color (BGRA). Default: #66000000.</summary>
        public (byte B, byte G, byte R, byte A) RoiDimColor { get; set; } = (0, 0, 0, 102);

        /// <summary>ROI border width in pixels.</summary>
        public int RoiLineWidth { get; set; } = 4;

        /// <summary>Whether to draw barcode location polygons.</summary>
        public bool LocationEnabled { get; set; }

        /// <summary>Location line color (BGRA). Default: red (#FFFF0000).</summary>
        public (byte B, byte G, byte R, byte A) LocationColor { get; set; } = (0, 0, 255, 255);

        /// <summary>Location line width in pixels.</summary>
        public int LocationLineWidth { get; set; } = 4;

        /// <summary>Barcode locations from the latest decode results.</summary>
        public BarcodeLocation[]? Locations { get; set; }

        // ---- AR Overlay ----

        /// <summary>Whether AR mode overlay is active.</summary>
        public bool AREnabled { get; set; }

        /// <summary>AR cache entries to render.</summary>
        public AROverlayEntry[]? AREntries { get; set; }

        /// <summary>AR configuration for rendering.</summary>
        public AROverlayConfig? ARConfig { get; set; }

        /// <summary>Whether the AR view is in frozen state (darkened overlay).</summary>
        public bool IsFrozen { get; set; }
    }

    /// <summary>
    /// Flattened AR overlay entry for the renderer (no internal dependencies on ARManager).
    /// </summary>
    public class AROverlayEntry
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

        // Polygon corner points (always set: TopLeft, TopRight, BottomRight, BottomLeft)
        public Models.PointF? P0 { get; set; }
        public Models.PointF? P1 { get; set; }
        public Models.PointF? P2 { get; set; }
        public Models.PointF? P3 { get; set; }
        public bool HasPolygon { get; set; }

        public bool IsSelected { get; set; }
        public float Alpha { get; set; } = 1.0f;
        public string HeaderText { get; set; } = "";
    }

    /// <summary>
    /// Rendering configuration for AR overlay (flattened from BarkoderARConfig).
    /// </summary>
    public class AROverlayConfig
    {
        public (byte B, byte G, byte R, byte A) SelectedColor { get; set; }
        public (byte B, byte G, byte R, byte A) NonSelectedColor { get; set; }
        public int SelectedLineWidth { get; set; } = 2;
        public int NonSelectedLineWidth { get; set; } = 2;
        public BarkoderARLocationType LocationType { get; set; }
        public BarkoderARHeaderShowMode HeaderShowMode { get; set; }
        public float HeaderHeight { get; set; } = 19f;
        public (byte B, byte G, byte R, byte A) HeaderTextColorSelected { get; set; }
        public (byte B, byte G, byte R, byte A) HeaderTextColorNonSelected { get; set; }
    }
}
