using Barkoder.Windows.Models;

namespace Barkoder.Windows.Config
{
    /// <summary>
    /// Configuration for AR (Augmented Reality) overlay mode.
    /// Mirrors BarkoderARConfig from Android/iOS SDKs.
    /// </summary>
    public class BarkoderARConfig
    {
        /// <summary>
        /// Overall AR behavior mode.
        /// </summary>
        public BarkoderARMode ARMode { get; set; } = BarkoderARMode.Off;

        /// <summary>
        /// Overlay color for selected barcodes (ARGB hex). Default: green #FF4FC778.
        /// </summary>
        public string SelectedLocationColor { get; set; } = "#FF4FC778";

        /// <summary>
        /// Overlay color for non-selected barcodes (ARGB hex). Default: red #FFCC0119.
        /// </summary>
        public string NonSelectedLocationColor { get; set; } = "#FFCC0119";

        /// <summary>
        /// Line width for selected barcode overlays.
        /// </summary>
        public float SelectedLocationLineWidth { get; set; } = 2.0f;

        /// <summary>
        /// Line width for non-selected barcode overlays.
        /// </summary>
        public float NonSelectedLocationLineWidth { get; set; } = 2.0f;

        /// <summary>
        /// Type of location overlay (none, tight polygon, or bounding box).
        /// </summary>
        public BarkoderARLocationType LocationType { get; set; } = BarkoderARLocationType.BoundingBox;

        /// <summary>
        /// When to show header text above barcodes.
        /// </summary>
        public BarkoderARHeaderShowMode HeaderShowMode { get; set; } = BarkoderARHeaderShowMode.OnSelected;

        /// <summary>
        /// Height of the header bar above the barcode overlay.
        /// </summary>
        public float HeaderHeight { get; set; } = 19.0f;

        /// <summary>
        /// Maximum font size for header text.
        /// </summary>
        public float HeaderMaxTextHeight { get; set; } = 20.0f;

        /// <summary>
        /// Minimum font size for header text.
        /// </summary>
        public float HeaderMinTextHeight { get; set; } = 7.0f;

        /// <summary>
        /// Header text color for selected barcodes (ARGB hex). Default: black.
        /// </summary>
        public string HeaderTextColorSelected { get; set; } = "#FF000000";

        /// <summary>
        /// Header text color for non-selected barcodes (ARGB hex). Default: white.
        /// </summary>
        public string HeaderTextColorNonSelected { get; set; } = "#FFFFFFFF";

        /// <summary>
        /// Horizontal margin inside the header bar.
        /// </summary>
        public float HeaderHorizontalTextMargin { get; set; } = 5.0f;

        /// <summary>
        /// Vertical margin inside the header bar.
        /// </summary>
        public float HeaderVerticalTextMargin { get; set; } = 3.0f;

        /// <summary>
        /// Header text format string. Supports placeholders: [barcode_text], [barcode_type].
        /// </summary>
        public string HeaderTextFormat { get; set; } = "[barcode_text]";

        /// <summary>
        /// Delay in milliseconds before expired results are removed from the overlay.
        /// </summary>
        public int ResultDisappearanceDelayMs { get; set; } = 800;

        /// <summary>
        /// Interpolation speed for overlay position transitions (0.1 - 1.0).
        /// </summary>
        public float LocationTransitionSpeed { get; set; } = 0.3f;

        /// <summary>
        /// Overlay refresh rate (smooth ~60fps or normal ~30fps).
        /// </summary>
        public BarkoderAROverlayRefresh OverlayRefresh { get; set; } = BarkoderAROverlayRefresh.Smooth;

        /// <summary>
        /// Enable double-tap to freeze/unfreeze scanning.
        /// </summary>
        public bool DoubleTapToFreezeEnabled { get; set; } = true;

        /// <summary>
        /// Capture image when a barcode is selected in AR mode.
        /// </summary>
        public bool ImageResultEnabled { get; set; } = false;

        /// <summary>
        /// Generate a thumbnail crop of selected barcodes.
        /// </summary>
        public bool BarcodeThumbnailOnResult { get; set; } = false;

        /// <summary>
        /// Maximum results per AR session (0 = unlimited).
        /// </summary>
        public int ResultLimit { get; set; } = 0;

        /// <summary>
        /// Continue scanning after result limit is reached (only for InteractiveDisabled mode).
        /// </summary>
        public bool ContinueScanningOnLimit { get; set; } = false;

        /// <summary>
        /// If true, emit results only when the AR session ends.
        /// </summary>
        public bool EmitResultsAtSessionEndOnly { get; set; } = false;

        /// <summary>
        /// Return only matched results from AR-selected output.
        /// </summary>
        public bool ReturnOnlyMatchedResults { get; set; } = false;

        /// <summary>
        /// Draw overlays only for matched results while AR is active.
        /// </summary>
        public bool DisplayOnlyMatchedResults { get; set; } = false;
    }
}
