
namespace Plugin.Maui.Barkoder.Handlers
{
    public class BarcodeResult
    {
        public string TextualData { get; set; }
        public string BarcodeTypeName { get; set; }
        public string CharacterSet { get; set; }

        public BarcodeResult(string textualData, string barcodeTypeName, string characterSet)
        {
            TextualData = textualData;
            BarcodeTypeName = barcodeTypeName;
            CharacterSet = characterSet;
        }
    }
}