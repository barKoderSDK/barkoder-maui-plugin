using System.Xml;
using System.Text.Json;
using Newtonsoft.Json;
using Plugin.Maui.Barkoder.Enums;

namespace Barkoder
{
    public class BarkoderConfig
    {
        public long locationLineColor;
        public int locationLineWidth;
        public long roiLineColor;
        public int roiLineWidth;
        public long roiOverlayBackgroundColor;
        public bool closeSessionOnResultEnabled;
        public bool imageResultEnabled;
        public bool locationInImageResultEnabled;
        public bool locationInPreviewEnabled;
        public bool pinchToZoomEnabled;
        public bool regionOfInterestVisible;
        public bool beepOnSuccessEnabled;
        public bool vibrateOnSuccessEnabled;
        public DekoderConfig decoder;

        public BarkoderConfig(
            long? locationLineColor = null,
            int? locationLineWidth = null,
            long? roiLineColor = null,
            int? roiLineWidth = null,
            long? roiOverlayBackgroundColor = null,
            bool? closeSessionOnResultEnabled = null,
            bool? imageResultEnabled = null,
            bool? locationInImageResultEnabled = null,
            bool? locationInPreviewEnabled = null,
            bool? pinchToZoomEnabled = null,
            bool? regionOfInterestVisible = null,
            bool? beepOnSuccessEnabled = null,
            bool? vibrateOnSuccessEnabled = null,
            DekoderConfig? decoder = null)
        {
            this.locationLineColor = locationLineColor ?? 4291559705;
            this.locationLineWidth = locationLineWidth ?? 2;
            this.roiLineColor = roiLineColor ?? 4291559705;
            this.roiLineWidth = roiLineWidth ?? 2;
            this.roiOverlayBackgroundColor = roiOverlayBackgroundColor ?? 4291559705;
            this.closeSessionOnResultEnabled = closeSessionOnResultEnabled ?? true;
            this.imageResultEnabled = imageResultEnabled ?? true;
            this.locationInImageResultEnabled = locationInImageResultEnabled ?? true;
            this.locationInPreviewEnabled = locationInPreviewEnabled ?? true;
            this.pinchToZoomEnabled = pinchToZoomEnabled ?? false;
            this.regionOfInterestVisible = regionOfInterestVisible ?? true;
            this.beepOnSuccessEnabled = beepOnSuccessEnabled ?? true;
            this.vibrateOnSuccessEnabled = vibrateOnSuccessEnabled ?? true;
            this.decoder = decoder ?? new DekoderConfig();
        }
    }

    public class DekoderConfig
    {
        [JsonProperty("Aztec")]
        public BarcodeConfig aztec;
        [JsonProperty("Aztec Compact")]
        public BarcodeConfig aztecCompact;
        [JsonProperty("QR")]
        public DatamatrixBarcodeConfig qr;
        [JsonProperty("QR Micro")]
        public DatamatrixBarcodeConfig qrMicro;
        [JsonProperty("Code 128")]
        public BarcodeConfigWithLength code128;
        [JsonProperty("Code 93")]
        public BarcodeConfigWithLength code93;
        [JsonProperty("Code 39")]
        public Code39BarcodeConfig code39;
        [JsonProperty("Codabar")]
        public BarcodeConfigWithLength codabar;
        [JsonProperty("Code 11")]
        public Code11BarcodeConfig code11;
        [JsonProperty("MSI")]
        public MSIBarcodeConfig msi;
        [JsonProperty("Upc-A")]
        public BarcodeConfig upcA;
        [JsonProperty("Upc-E")]
        public BarcodeConfig upcE;
        [JsonProperty("Upc-E1")]
        public BarcodeConfig upcE1;
        [JsonProperty("Ean-13")]
        public BarcodeConfig ean13;
        [JsonProperty("Ean-8")]
        public BarcodeConfig ean8;
        [JsonProperty("PDF 417")]
        public BarcodeConfig pdf417;
        [JsonProperty("PDF 417 Micro")]
        public BarcodeConfig pdf417Micro;
        [JsonProperty("Datamatrix")]
        public DatamatrixBarcodeConfig datamatrix;
        [JsonProperty("Code 25")]
        public BarcodeConfig code25;
        [JsonProperty("Interleaved 2 of 5")]
        public BarcodeConfig interleaved25;
        [JsonProperty("ITF 14")]
        public BarcodeConfig itf14;
        [JsonProperty("IATA 25")]
        public BarcodeConfig iata25;
        [JsonProperty("Matrix 25")]
        public BarcodeConfig matrix25;
        [JsonProperty("Datalogic 25")]
        public BarcodeConfig datalogic25;
        [JsonProperty("COOP 25")]
        public BarcodeConfig coop25;
        [JsonProperty("Code 32")]
        public BarcodeConfig code32;
        [JsonProperty("Telepen")]
        public BarcodeConfig telepen;
        [JsonProperty("Dotcode")]
        public BarcodeConfig dotcode;
        [JsonProperty("ID Document")]
        public BarcodeConfig idDocument;
        [JsonProperty("Databar 14")]
        public BarcodeConfig databar14;
        [JsonProperty("Databar Limited")]
        public BarcodeConfig databarLimited;
        [JsonProperty("Databar Expanded")]
        public BarcodeConfig databarExpanded;
        [JsonProperty("general")]
        public GeneralSettings general;

        public DekoderConfig(
            BarcodeConfig? aztec = null,
            BarcodeConfig? aztecCompact = null,
            DatamatrixBarcodeConfig? qr = null,
            DatamatrixBarcodeConfig? qrMicro = null,
            BarcodeConfigWithLength? code128 = null,
            BarcodeConfigWithLength? code93 = null,
            Code39BarcodeConfig? code39 = null,
            BarcodeConfigWithLength? codabar = null,
            Code11BarcodeConfig? code11 = null,
            MSIBarcodeConfig? msi = null,
            BarcodeConfig? upcA = null,
            BarcodeConfig? upcE = null,
            BarcodeConfig? upcE1 = null,
            BarcodeConfig? ean13 = null,
            BarcodeConfig? ean8 = null,
            BarcodeConfig? pdf417 = null,
            BarcodeConfig? pdf417Micro = null,
            DatamatrixBarcodeConfig? datamatrix = null,
            BarcodeConfig? code25 = null,
            BarcodeConfig? interleaved25 = null,
            BarcodeConfig? itf14 = null,
            BarcodeConfig? iata25 = null,
            BarcodeConfig? matrix25 = null,
            BarcodeConfig? datalogic25 = null,
            BarcodeConfig? coop25 = null,
            BarcodeConfig? code32 = null,
            BarcodeConfig? telepen = null,
            BarcodeConfig? dotcode = null,
            BarcodeConfig? idDocument = null,
            BarcodeConfig? databar14 = null,
            BarcodeConfig? databarLimited = null,
            BarcodeConfig? databarExpanded = null,

            GeneralSettings? general = null)
        {
            this.aztec = aztec ?? new BarcodeConfig();
            this.aztecCompact = aztecCompact ?? new BarcodeConfig();
            this.qr = qr ?? new DatamatrixBarcodeConfig();
            this.qrMicro = qrMicro ?? new DatamatrixBarcodeConfig();
            this.code128 = code128 ?? new BarcodeConfigWithLength();
            this.code93 = code93 ?? new BarcodeConfigWithLength();
            this.code39 = code39 ?? new Code39BarcodeConfig();
            this.codabar = codabar ?? new BarcodeConfigWithLength();
            this.code11 = code11 ?? new Code11BarcodeConfig();
            this.msi = msi ?? new MSIBarcodeConfig();
            this.upcA = upcA ?? new BarcodeConfig();
            this.upcE = upcE ?? new BarcodeConfig();
            this.upcE1 = upcE1 ?? new BarcodeConfig();
            this.ean13 = ean13 ?? new BarcodeConfig();
            this.ean8 = ean8 ?? new BarcodeConfig();
            this.pdf417 = pdf417 ?? new BarcodeConfig();
            this.pdf417Micro = pdf417Micro ?? new BarcodeConfig();
            this.datamatrix = datamatrix ?? new DatamatrixBarcodeConfig();
            this.code25 = code25 ?? new BarcodeConfig();
            this.interleaved25 = interleaved25 ?? new BarcodeConfig();
            this.itf14 = itf14 ?? new BarcodeConfig();
            this.iata25 = iata25 ?? new BarcodeConfig();
            this.matrix25 = matrix25 ?? new BarcodeConfig();
            this.datalogic25 = datalogic25 ?? new BarcodeConfig();
            this.coop25 = coop25 ?? new BarcodeConfig();
            this.code32 = code32 ?? new BarcodeConfig();
            this.telepen = telepen ?? new BarcodeConfig();
            this.dotcode = dotcode ?? new BarcodeConfig();
            this.idDocument = idDocument ?? new BarcodeConfig();
            this.databar14 = databar14 ?? new BarcodeConfig();
            this.databarLimited = databarLimited ?? new BarcodeConfig();
            this.databarExpanded = databarExpanded ?? new BarcodeConfig();
            this.general = general ?? new GeneralSettings();
        }
    }

    public class GeneralSettings
    {
        [JsonProperty("maxThreads")]
        public int ThreadsLimit;
        [JsonProperty("decodingSpeed")]
        public DecodingSpeed DecodingSpeed;
        [JsonProperty("roi_x")]
        public int RoiX;
        [JsonProperty("roi_y")]
        public int RoiY;
        [JsonProperty("roi_w")]
        public int RoiWidth;
        [JsonProperty("roi_h")]
        public int RoiHeight;
        [JsonProperty("formattingType")]
        public FormattingType FormattingType;
        [JsonProperty("encodingCharacterSet")]
        public string EncodingCharacterSet;
        [JsonProperty("upcEanDeblur")]
        public bool UpcEanDeblur;
        [JsonProperty("enableMisshaped1D")]
        public bool EnableMisshaped1D;

        public GeneralSettings(
            int? threadsLimit = null,
            DecodingSpeed? decodingSpeed = null,
            int? roiX = null,
            int? roiY = null,
            int? roiWidth = null,
            int? roiHeight = null,
            FormattingType? formattingType = null,
            string? encodingCharacterSet = null,
            bool? upcEanDeblur = null,
            bool? enableMisshaped1D = null
         )
        {
            this.ThreadsLimit = threadsLimit ?? 2;
            this.DecodingSpeed = decodingSpeed ?? DecodingSpeed.Normal;
            this.RoiX = roiX ?? 5;
            this.RoiY = roiY ?? 5;
            this.RoiWidth = roiWidth ?? 90;
            this.RoiHeight = roiHeight ?? 90;
            this.FormattingType = formattingType ?? FormattingType.Disabled;
            this.EncodingCharacterSet = encodingCharacterSet ?? "";
            this.UpcEanDeblur = upcEanDeblur ?? false;
            this.EnableMisshaped1D = enableMisshaped1D ?? false;
        }
    }

    public class BarcodeConfig
    {
        public bool enabled;

        public BarcodeConfig(bool? enabled = null)
        {
            this.enabled = enabled ?? false;
        }
    }

    public class BarcodeConfigWithLength : BarcodeConfig
    {
        public int minLength;
        public int maxLength;

        public BarcodeConfigWithLength(
            bool? enabled = null,
            int? minLength = null,
            int? maxLenght = null
            )
        {
            this.enabled = enabled ?? false;
            this.maxLength = maxLenght ?? 0;
            this.minLength = minLength ?? 0;
        }
    }

    public class Code39BarcodeConfig : BarcodeConfigWithLength
    {
        public Code39ChecksumType checksum;

        public Code39BarcodeConfig(
            bool? enabled = null,
            int? minLength = null,
            int? maxLenght = null,
            Code39ChecksumType? checksum = null
            )
        {
            this.enabled = enabled ?? false;
            this.maxLength = maxLenght ?? 0;
            this.minLength = minLength ?? 0;
            this.checksum = checksum ?? Code39ChecksumType.Disabled;
        }
    }

    public class Code11BarcodeConfig : BarcodeConfigWithLength
    {
        public Code11ChecksumType checksum;

        public Code11BarcodeConfig(
            bool? enabled = null,
            int? minLength = null,
            int? maxLenght = null,
            Code11ChecksumType? checksum = null
            )
        {
            this.enabled = enabled ?? false;
            this.maxLength = maxLenght ?? 0;
            this.minLength = minLength ?? 0;
            this.checksum = checksum ?? Code11ChecksumType.Disabled;
        }
    }

    public class MSIBarcodeConfig : BarcodeConfigWithLength
    {
        public MsiChecksumType checksum;

        public MSIBarcodeConfig(
            bool? enabled = null,
            int? minLength = null,
            int? maxLenght = null,
            MsiChecksumType? checksum = null
            )
        {
            this.enabled = enabled ?? false;
            this.maxLength = maxLenght ?? 0;
            this.minLength = minLength ?? 0;
            this.checksum = checksum ?? MsiChecksumType.Disabled;
        }
    }

    public class DatamatrixBarcodeConfig : BarcodeConfigWithLength
    {
        int dpmMode;

        public DatamatrixBarcodeConfig(
            bool? enabled = null,
            int? minLength = null,
            int? maxLenght = null,
            int? dpmMode = null
            )
        {
            this.enabled = enabled ?? false;
            this.maxLength = maxLenght ?? 0;
            this.minLength = minLength ?? 0;
            this.dpmMode = dpmMode ?? 0;
        }
    }

}

