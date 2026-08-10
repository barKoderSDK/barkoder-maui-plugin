using Barkoder.Windows.Models;

namespace Barkoder.Windows.Config
{
    /// <summary>
    /// Decoder-level configuration controlling which barcode types are enabled
    /// and their individual settings. Maps to BarkoderNET.Config properties.
    /// </summary>
    public class DecoderConfig
    {
        private readonly HashSet<DecoderType> _enabledDecoders = new();

        // ---- Decoder Enable/Disable ----

        public void EnableDecoder(DecoderType type) => _enabledDecoders.Add(type);

        public void DisableDecoder(DecoderType type) => _enabledDecoders.Remove(type);

        public void SetEnabledDecoders(params DecoderType[] types)
        {
            _enabledDecoders.Clear();
            foreach (var type in types)
                _enabledDecoders.Add(type);
        }

        public IReadOnlySet<DecoderType> GetEnabledDecoders() => _enabledDecoders;

        public bool IsDecoderEnabled(DecoderType type) => _enabledDecoders.Contains(type);

        // ---- General Decoding Settings ----

        /// <summary>
        /// Decoding speed preset. Default: Normal.
        /// </summary>
        public DecodingSpeed DecodingSpeed { get; set; } = DecodingSpeed.Normal;

        /// <summary>
        /// Result formatting mode. Default: Automatic.
        /// </summary>
        public FormattingType Formatting { get; set; } = FormattingType.Automatic;

        /// <summary>
        /// Maximum number of barcodes to return per frame. Default: 10.
        /// </summary>
        public int MaximumResultsCount { get; set; } = 10;

        /// <summary>
        /// Override character encoding for barcode data.
        /// </summary>
        public string EncodingCharacterSet { get; set; } = string.Empty;

        /// <summary>
        /// Suppress duplicate results within this many milliseconds. Default: 0 (disabled).
        /// </summary>
        public int DuplicatesDelayMs { get; set; } = 0;

        /// <summary>
        /// Enable detection of damaged/misshaped 1D barcodes. Default: false.
        /// </summary>
        public bool EnableMisshaped1D { get; set; } = false;

        /// <summary>
        /// Enable UPC/EAN deblurring for better decoding of blurry barcodes. Default: false.
        /// </summary>
        public bool UpcEanDeblur { get; set; } = false;

        /// <summary>
        /// Enable composite barcode decoding. Default: false.
        /// </summary>
        public bool CompositeCodesEnabled { get; set; } = false;

        /// <summary>
        /// Enable VIN format validation. Default: false.
        /// </summary>
        public bool EnableVINRestrictions { get; set; } = false;

        /// <summary>
        /// JSON array of strings used by the native decoder match filter.
        /// Example: ["9780201379624", "1234567890128"].
        /// </summary>
        public string MatchFilter { get; set; } = string.Empty;

        /// <summary>
        /// If true, return only results marked as matched by the decoder.
        /// </summary>
        public bool ReturnOnlyMatchedResults { get; set; } = false;

        /// <summary>
        /// ROI center mark mode used to control center-only scanning behavior.
        /// NONE disables center-only scanning. Default: NONE.
        /// </summary>
        public BarkoderRoiCenterMark RoiCenterMark { get; set; } = BarkoderRoiCenterMark.None;

        // ---- Per-Decoder Settings ----

        /// <summary>
        /// QR Direct Part Marking mode (0 = off, 1 = on). Default: 0.
        /// </summary>
        public int QrDpmMode { get; set; } = 0;

        /// <summary>
        /// QR multi-part (structured append) merge. Default: false.
        /// </summary>
        public bool QrMultiPartMerge { get; set; } = false;

        /// <summary>
        /// QR Micro DPM mode (0 = off, 1 = on). Default: 0.
        /// </summary>
        public int QrMicroDpmMode { get; set; } = 0;

        /// <summary>
        /// DataMatrix DPM mode (0 = off, 1 = on). Default: 0.
        /// </summary>
        public int DataMatrixDpmMode { get; set; } = 0;

        /// <summary>
        /// Code39 checksum mode. Default: Disabled.
        /// </summary>
        public Code39ChecksumType Code39Checksum { get; set; } = Code39ChecksumType.Disabled;

        /// <summary>
        /// Code11 checksum mode. Default: Disabled.
        /// </summary>
        public Code11ChecksumType Code11Checksum { get; set; } = Code11ChecksumType.Disabled;

        /// <summary>
        /// MSI checksum mode. Default: Disabled.
        /// </summary>
        public MsiChecksumType MsiChecksum { get; set; } = MsiChecksumType.Disabled;

        /// <summary>
        /// Expand UPC-E to UPC-A in results. Default: false.
        /// </summary>
        public bool UpcEExpandToUPCA { get; set; } = false;

        /// <summary>
        /// Expand UPC-E1 to UPC-A in results. Default: false.
        /// </summary>
        public bool UpcE1ExpandToUPCA { get; set; } = false;
    }
}
