using Barkoder.Windows.Models;

namespace Barkoder.Windows.Config
{
    /// <summary>
    /// Main configuration class for the Barkoder scanner.
    /// Mirrors the BarkoderConfig from Android/iOS SDKs.
    /// </summary>
    public class BarkoderConfig
    {
        /// <summary>
        /// License key for the Barkoder SDK.
        /// </summary>
        public string LicenseKey { get; }

        /// <summary>
        /// Decoder configuration (which barcode types are enabled, etc.).
        /// </summary>
        public DecoderConfig DecoderConfig { get; }

        /// <summary>
        /// AR (Augmented Reality) overlay configuration.
        /// </summary>
        public BarkoderARConfig ARConfig { get; } = new BarkoderARConfig();

        // ---- Scanning Behavior ----

        /// <summary>
        /// Automatically stop scanning after a successful decode.
        /// </summary>
        public bool CloseSessionOnResultEnabled { get; set; } = true;

        /// <summary>
        /// Play a beep sound on successful decode.
        /// </summary>
        public bool BeepOnSuccessEnabled { get; set; } = true;

        /// <summary>
        /// Vibrate on successful decode (if supported by device).
        /// </summary>
        public bool VibrateOnSuccessEnabled { get; set; } = false;

        /// <summary>
        /// Enable pinch-to-zoom gesture on camera preview.
        /// </summary>
        public bool PinchToZoomEnabled { get; set; } = false;

        /// <summary>
        /// Minimum time (in seconds) between duplicate barcode scans.
        /// </summary>
        public int ThresholdBetweenDuplicatesScans { get; set; } = 0;

        // ---- Region of Interest ----

        private float _roiLeft = 0f;
        private float _roiTop = 0f;
        private float _roiWidth = 100f;
        private float _roiHeight = 100f;

        /// <summary>
        /// Corner radius of the ROI rectangle.
        /// </summary>
        public float RoiCornerRadius { get; set; } = 0f;

        /// <summary>
        /// Whether the ROI rectangle is visible on the preview.
        /// </summary>
        public bool RegionOfInterestVisible { get; set; } = false;

        /// <summary>
        /// Set the Region of Interest as percentages (0-100).
        /// </summary>
        public void SetRegionOfInterest(float left, float top, float width, float height)
        {
            if (left < 0 || left > 100 || top < 0 || top > 100 ||
                width < 0 || width > 100 || height < 0 || height > 100)
                throw new ArgumentOutOfRangeException("ROI values must be between 0 and 100.");

            if (left + width > 100 || top + height > 100)
                throw new ArgumentOutOfRangeException("ROI must fit within 0-100 range.");

            _roiLeft = left;
            _roiTop = top;
            _roiWidth = width;
            _roiHeight = height;
        }

        /// <summary>
        /// Get the current Region of Interest (left, top, width, height) as percentages.
        /// </summary>
        public (float Left, float Top, float Width, float Height) GetRegionOfInterest()
            => (_roiLeft, _roiTop, _roiWidth, _roiHeight);

        // ---- Resolution ----

        /// <summary>
        /// Camera resolution preset.
        /// </summary>
        public BarkoderResolution BarkoderResolution { get; set; } = BarkoderResolution.HD;

        // ---- Camera ----

        /// <summary>
        /// Which camera to use (front/back).
        /// </summary>
        public BarkoderCameraPosition CameraPosition { get; set; } = BarkoderCameraPosition.Back;

        // ---- UI / Overlay Appearance ----

        /// <summary>
        /// Color of the ROI border line (ARGB hex string). Default: semi-transparent red.
        /// </summary>
        public string RoiLineColor { get; set; } = "#80FF0000";

        /// <summary>
        /// Width of the ROI border line in pixels.
        /// </summary>
        public float RoiLineWidth { get; set; } = 4.0f;

        /// <summary>
        /// Background color of the area outside the ROI (semi-transparent). Default: #66000000.
        /// </summary>
        public string RoiOverlayBackgroundColor { get; set; } = "#66000000";

        /// <summary>
        /// Color of the scanning indicator line. Default: semi-transparent red.
        /// </summary>
        public string ScanningIndicatorColor { get; set; } = "#80FF0000";

        /// <summary>
        /// Width of the scanning indicator line.
        /// </summary>
        public float ScanningIndicatorWidth { get; set; } = 4.0f;

        /// <summary>
        /// Animation type for the scanning indicator.
        /// </summary>
        public ScanningIndicatorAnimation ScanningIndicatorAnimation { get; set; } = ScanningIndicatorAnimation.Default;

        /// <summary>
        /// Whether the scanning indicator is always visible (not just during scanning).
        /// </summary>
        public bool ScanningIndicatorAlwaysVisible { get; set; } = false;

        // ---- Location Display ----

        /// <summary>
        /// Color used to draw the detected barcode location outline. Default: red.
        /// </summary>
        public string LocationLineColor { get; set; } = "#FFFF0000";

        /// <summary>
        /// Width of the barcode location outline.
        /// </summary>
        public float LocationLineWidth { get; set; } = 4.0f;

        /// <summary>
        /// Whether to show the barcode location on the camera preview.
        /// </summary>
        public bool LocationInPreviewEnabled { get; set; } = true;

        /// <summary>
        /// Whether to include barcode location overlay in the result image.
        /// </summary>
        public bool LocationInImageResultEnabled { get; set; } = false;

        // ---- Result Image ----

        /// <summary>
        /// Whether to capture the full camera frame as part of the result.
        /// </summary>
        public bool ImageResultEnabled { get; set; } = false;

        /// <summary>
        /// Whether to generate a cropped thumbnail of each barcode.
        /// </summary>
        public bool BarcodeThumbnailOnResult { get; set; } = true;

        // ---- Performance ----

        /// <summary>
        /// Whether to show duplicate barcode locations on the preview.
        /// </summary>
        public bool ShowDuplicateLocations { get; set; } = true;

        /// <summary>
        /// Initial zoom factor for the camera.
        /// </summary>
        public float InitialZoom { get; set; } = 1.0f;

        /// <summary>
        /// Whether the flash/torch is enabled at startup.
        /// </summary>
        public bool InitialFlashEnabled { get; set; } = false;

        /// <summary>
        /// Maximum result limit per session (0 = unlimited).
        /// </summary>
        public int ResultLimit { get; set; } = 0;

        /// <summary>
        /// Continue scanning after result limit is reached.
        /// </summary>
        public bool ContinueScanningOnLimit { get; set; } = false;

        /// <summary>
        /// Only emit results when the session ends.
        /// </summary>
        public bool EmitResultAtSessionEndOnly { get; set; } = false;

        private int _threadsLimit = 2;

        /// <summary>
        /// Maximum number of threads used for decoding.
        /// </summary>
        public int ThreadsLimit
        {
            get => _threadsLimit;
            set
            {
                if (value < 1 || value > Environment.ProcessorCount)
                    throw new ArgumentOutOfRangeException(nameof(ThreadsLimit),
                        $"Must be between 1 and {Environment.ProcessorCount}.");
                _threadsLimit = value;
            }
        }

        /// <summary>
        /// Power saving mode level (0 = disabled, 1-4 = increasing power savings).
        /// </summary>
        public int PowerSavingMode
        {
            get => _powerSavingMode;
            set
            {
                if (value < 0 || value > 4)
                    throw new ArgumentOutOfRangeException(nameof(PowerSavingMode), "Must be between 0 and 4.");
                _powerSavingMode = value;
            }
        }
        private int _powerSavingMode = 0;

        /// <summary>
        /// Enable caching for multi-code scanning sessions.
        /// </summary>
        public bool MulticodeCachingEnabled { get; set; } = false;

        /// <summary>
        /// Duration (in milliseconds) to cache multi-code results.
        /// </summary>
        public int MulticodeCachingDuration { get; set; } = 3000;

        /// <summary>
        /// Create a new BarkoderConfig with the given license key.
        /// </summary>
        public BarkoderConfig(string licenseKey)
        {
            LicenseKey = licenseKey ?? throw new ArgumentNullException(nameof(licenseKey));
            DecoderConfig = new DecoderConfig();
        }
    }
}
