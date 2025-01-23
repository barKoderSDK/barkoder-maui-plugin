

namespace Plugin.Maui.Barkoder.Handlers
{
    public class BarcodeResult
    {
        public string TextualData { get; set; }
        public string BarcodeTypeName { get; set; }
        public Dictionary<string, object>? Extra { get; set; } // Updated to match React structure
        public string CharacterSet { get; set; }
        public List<ImageData> MrzImages { get; set; }

        public BarcodeResult(string textualData, string barcodeTypeName, Dictionary<string, object>? extra, string characterSet, List<ImageData> mrzImages)
        {
            TextualData = textualData;
            BarcodeTypeName = barcodeTypeName;
            Extra = extra;
            CharacterSet = characterSet;
            MrzImages = mrzImages;
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
}