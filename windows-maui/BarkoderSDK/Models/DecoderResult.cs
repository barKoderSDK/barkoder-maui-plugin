using Microsoft.Maui.Controls;

namespace Barkoder.Windows.Models
{
    /// <summary>
    /// Represents the location of a detected barcode in the image.
    /// Contains the four corner points of the barcode.
    /// </summary>
    public class BarcodeLocation
    {
        public PointF TopLeft { get; set; }
        public PointF TopRight { get; set; }
        public PointF BottomLeft { get; set; }
        public PointF BottomRight { get; set; }

        public BarcodeLocation()
        {
            TopLeft = new PointF();
            TopRight = new PointF();
            BottomLeft = new PointF();
            BottomRight = new PointF();
        }

        public BarcodeLocation(PointF topLeft, PointF topRight, PointF bottomRight, PointF bottomLeft)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
        }
    }

    /// <summary>
    /// A simple 2D point with float coordinates.
    /// </summary>
    public class PointF
    {
        public float X { get; set; }
        public float Y { get; set; }

        public PointF() { }
        public PointF(float x, float y) { X = x; Y = y; }
    }

    /// <summary>
    /// Represents a single barcode scan result, mapped from BarkoderNET.BaseResult.
    /// </summary>
    public class BarcodeResult
    {
        /// <summary>
        /// The decoded text/data from the barcode.
        /// </summary>
        public string TextualData { get; set; } = string.Empty;

        /// <summary>
        /// Raw binary content of the barcode.
        /// </summary>
        public byte[]? BinaryData { get; set; }

        /// <summary>
        /// The type of barcode that was detected.
        /// </summary>
        public DecoderType BarcodeType { get; set; }

        /// <summary>
        /// Human-readable name of the barcode type.
        /// </summary>
        public string BarcodeTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Character set used to decode the barcode data.
        /// </summary>
        public string CharacterSet { get; set; } = string.Empty;

        /// <summary>
        /// Location of the barcode in the image (corner points).
        /// </summary>
        public BarcodeLocation? Location { get; set; }

        /// <summary>
        /// Detailed polygon outline of the barcode.
        /// </summary>
        public List<PointF>? PolygonLocation { get; set; }

        /// <summary>
        /// Center point of the barcode location.
        /// </summary>
        public PointF? LocationCenter { get; set; }

        /// <summary>
        /// Whether the barcode is GS1 formatted.
        /// </summary>
        public bool IsGS1 { get; set; }

        /// <summary>
        /// Whether the barcode has composite linkage.
        /// </summary>
        public bool HasLinkage { get; set; }

        /// <summary>
        /// Whether the result came from multicode cache.
        /// </summary>
        public bool IsCached { get; set; }

        /// <summary>
        /// Whether the result is from template matching.
        /// </summary>
        public bool IsTemplateMatching { get; set; }

        /// <summary>
        /// Whether the result matched the configured match filter.
        /// </summary>
        public bool IsMatched { get; set; }

        /// <summary>
        /// Detection timestamp in milliseconds.
        /// </summary>
        public ulong TimestampMs { get; set; }

        /// <summary>
        /// Extra metadata associated with the result (GS1 fields, ID document fields, etc.).
        /// </summary>
        public Dictionary<string, string> Extra { get; set; } = new();

        /// <summary>
        /// MRZ zone locations extracted from native results (picture, signature, document, etc.).
        /// Each entry contains a zone name and its BarcodeLocation polygon.
        /// </summary>
        public List<(string Name, BarcodeLocation Location)>? MrzLocations { get; set; }

        /// <summary>
        /// Confidence score of the decode (0.0 - 1.0).
        /// </summary>
        public float Confidence { get; set; }

        /// <summary>
        /// Extracted sub-images for MRZ/ID document results (picture, signature, document, main).
        /// Each descriptor contains a name and an ImageSource.
        /// Populated only for IDDocument/MRZ barcode types.
        /// </summary>
        public List<BKImageDescriptor>? Images { get; set; }
    }

    /// <summary>
    /// Describes an extracted sub-image from an MRZ/ID document result.
    /// Matches BKImageDescriptor from Android/iOS SDKs.
    /// </summary>
    public class BKImageDescriptor
    {
        /// <summary>
        /// Zone name: "picture", "signature", "document", or "main".
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The extracted image as a MAUI ImageSource.
        /// </summary>
        public ImageSource? Image { get; set; }

        public BKImageDescriptor(string name, ImageSource? image)
        {
            Name = name;
            Image = image;
        }
    }
}
