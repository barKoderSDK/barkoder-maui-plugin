namespace Barkoder.Windows.Models
{
    /// <summary>
    /// Camera position selection.
    /// </summary>
    public enum BarkoderCameraPosition
    {
        Back = 0,
        Front = 1
    }

    /// <summary>
    /// Camera resolution presets.
    /// </summary>
    public enum BarkoderResolution
    {
        HD = 0,    // 1280x720
        FHD = 1,   // 1920x1080
        UHD = 2    // 3840x2160
    }

    /// <summary>
    /// Supported barcode decoder types.
    /// Maps 1:1 to BarkoderNET.DecoderType.
    /// </summary>
    public enum DecoderType
    {
        Aztec,
        AztecCompact,
        QR,
        QRMicro,
        Code128,
        Code93,
        Code39,
        Codabar,
        Code11,
        Msi,
        UPCa,
        UPCe,
        UPCe1,
        EAN13,
        EAN8,
        PDF417,
        PDF417Micro,
        DataMatrix,
        Code25,
        Interleaved2of5,
        ITF14,
        IATA25,
        Matrix25,
        Datalogic25,
        COOP25,
        Code32,
        Telepen,
        DotCode,
        IDDocument,
        Databar14,
        DatabarLimited,
        DatabarExpanded,
        PostalIMB,
        Postnet,
        Planet,
        AustralianPost,
        RoyalMail,
        KIX,
        JapanesePost,
        MaxiCode,
        OCRText
    }

    /// <summary>
    /// Decoding speed presets. Maps to BarkoderNET.DecodingSpeed.
    /// </summary>
    public enum DecodingSpeed
    {
        Fast = 0,
        Normal = 1,
        Slow = 2,
        Rigorous = 3
    }

    /// <summary>
    /// Result formatting mode. Maps to BarkoderNET.Formatting.
    /// </summary>
    public enum FormattingType
    {
        Disabled = 0,
        Automatic = 1,
        GS1 = 2,
        AAMVA = 3,
        SADL = 4,
        BCBP = 5
    }

    /// <summary>
    /// Parsing helpers for FormattingType.
    /// </summary>
    public static class FormattingTypeParser
    {
        public static FormattingType FromString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return FormattingType.Automatic;

            foreach (FormattingType formatting in Enum.GetValues(typeof(FormattingType)))
            {
                if (formatting.ToString().Equals(value.Trim(), StringComparison.OrdinalIgnoreCase))
                    return formatting;
            }

            return FormattingType.Automatic;
        }
    }

    /// <summary>
    /// State of the barcode scanner.
    /// </summary>
    public enum BarkoderState
    {
        Idle,
        CameraReady,
        Scanning,
        Paused,
        Stopped,
        Error
    }

    /// <summary>
    /// Scanning indicator animation type.
    /// </summary>
    public enum ScanningIndicatorAnimation
    {
        None = 0,
        Default = 1,
        Fading = 2
    }

    /// <summary>
    /// MSI checksum types. Maps to BarkoderNET.MsiConfig.ChecksumType.
    /// </summary>
    public enum MsiChecksumType
    {
        Disabled,
        Mod10,
        Mod11,
        Mod1010,
        Mod1110,
        Mod11IBM,
        Mod1110IBM
    }

    /// <summary>
    /// Code11 checksum types. Maps to BarkoderNET.Code11Config.ChecksumType.
    /// </summary>
    public enum Code11ChecksumType
    {
        Disabled,
        Single,
        Double
    }

    /// <summary>
    /// Code39 checksum types. Maps to BarkoderNET.Code39Config.ChecksumType.
    /// </summary>
    public enum Code39ChecksumType
    {
        Disabled,
        Enabled
    }

    /// <summary>
    /// AR (Augmented Reality) overlay mode.
    /// </summary>
    public enum BarkoderARMode
    {
        /// <summary>AR is disabled.</summary>
        Off = 0,
        /// <summary>AR on; new barcodes are NOT auto-selected.</summary>
        InteractiveDisabled = 1,
        /// <summary>AR on; new barcodes are auto-selected.</summary>
        InteractiveEnabled = 2,
        /// <summary>AR on; fully automatic, no user interaction.</summary>
        NonInteractive = 3,
        /// <summary>AR on; selection and filtering are driven by decoder match filter results.</summary>
        MatchFilter = 4
    }

    /// <summary>
    /// Type of location overlay drawn around barcodes in AR mode.
    /// </summary>
    public enum BarkoderARLocationType
    {
        /// <summary>No location overlay drawn.</summary>
        None = 0,
        /// <summary>Tight polygon around the barcode.</summary>
        Tight = 1,
        /// <summary>Axis-aligned bounding rectangle.</summary>
        BoundingBox = 2
    }

    /// <summary>
    /// When to show the header text above barcodes in AR mode.
    /// </summary>
    public enum BarkoderARHeaderShowMode
    {
        /// <summary>Never show header text.</summary>
        Never = 0,
        /// <summary>Always show header text.</summary>
        Always = 1,
        /// <summary>Show header only on selected barcodes.</summary>
        OnSelected = 2
    }

    /// <summary>
    /// Overlay refresh rate for AR mode.
    /// </summary>
    public enum BarkoderAROverlayRefresh
    {
        /// <summary>Smooth refresh (~60 FPS).</summary>
        Smooth = 0,
        /// <summary>Normal refresh (~30 FPS).</summary>
        Normal = 1
    }

    /// <summary>
    /// ROI center mark behavior for center-only scanning.
    /// NONE disables center-only scan and normal ROI scanning is used.
    /// </summary>
    public enum BarkoderRoiCenterMark
    {
        None = 0,
        Crosshair = 1,
        Point = 2
    }

    /// <summary>
    /// Parsing helpers for BarkoderRoiCenterMark.
    /// </summary>
    public static class BarkoderRoiCenterMarkParser
    {
        public static BarkoderRoiCenterMark FromString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BarkoderRoiCenterMark.None;

            foreach (BarkoderRoiCenterMark mark in Enum.GetValues(typeof(BarkoderRoiCenterMark)))
            {
                if (mark.ToString().Equals(value.Trim(), StringComparison.OrdinalIgnoreCase))
                    return mark;
            }

            return BarkoderRoiCenterMark.None;
        }
    }
}
