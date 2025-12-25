

namespace Plugin.Maui.Barkoder.Handlers
{
    public class BarcodeResult
    {
        public string TextualData { get; set; }
        public string BarcodeTypeName { get; set; }
        public Dictionary<string, object>? Extra { get; set; } // Updated to match React structure
        public string CharacterSet { get; set; }
        public List<ImageData> MrzImages { get; set; }
        public BarcodeLocation Location { get; set; }
        public string BinaryDataAsBase64 { get; set; }
        public string SadlImageAsBase64 { get; set; } = "";

        public BarcodeResult(string textualData, string barcodeTypeName, Dictionary<string, object>? extra, string characterSet, List<ImageData> mrzImages, BarcodeLocation location, string binaryDataAsBase64, string sadlImageAsBase64)
        {
            TextualData = textualData;
            BarcodeTypeName = barcodeTypeName;
            Extra = extra;
            CharacterSet = characterSet;

            MrzImages = mrzImages;
            Location = location;
            BinaryDataAsBase64 = binaryDataAsBase64;
            SadlImageAsBase64 = sadlImageAsBase64;
        }
    }

    public class ImageData
    {
        public string Name { get; set; }
        public ImageSource Image { get; set; }

        public ImageData(string name, ImageSource image)
        {
            Name = name;
            Image = image;
        }
    }
    public class BarcodeLocation
    {
        public List<BarcodePoint> Points { get; set; } = new();
        public string LocationName { get; set; }
    }

    public class BarcodePoint
    {
        public float X { get; set; }
        public float Y { get; set; }

        public BarcodePoint(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}