namespace BarkoderSample;

using Plugin.Maui.Barkoder.Handlers;
using Plugin.Maui.Barkoder.Interfaces;
using Barkoder;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Text;

public partial class MainPage : ContentPage, IBarkoderDelegate
{

    public MainPage()
	{
        InitializeComponent();

        _ = ExecuteAfterDelayAsync();
    }

    private async Task ExecuteAfterDelayAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(0.5)); 

        SetUI();
        SetBarkoderSettings();
        SetActiveBarcodeTypes();
    }

    private void SetUI()
	{
        Title = $"Barkoder Sample (v{BKDView.Version})";
    }

    private async void SetBarkoderSettings()
    {
        BKDView.SetImageResultEnabled(true);
        BKDView.SetLocationInPreviewEnabled(true);
        BKDView.SetRegionOfInterestVisible(true);
        BKDView.SetCloseSessionOnResultEnabled(true);
        BKDView.SetVibrateOnSuccessEnabled(false);
        BKDView.SetRegionOfInterest(5, 5, 90, 90);
        BKDView.SetQRDpmModeEnabled(true);
        BKDView.SetQRMicroDpmModeEnabled(true);
        BKDView.SetDatamatrixDpmModeEnabled(false);
        BKDView.SetIdDocumentMasterChecksumEnabled(true);
      



    }

    private async void scanFromGallery(object sender, EventArgs e)
    {
        // Ensure the media plugin is initialized
        await CrossMedia.Current.Initialize();

        if (!CrossMedia.Current.IsPickPhotoSupported)
        {
            // Gallery picking is not supported on this device
            await DisplayAlert("Error", "Gallery picking is not supported on this device.", "OK");
            return;
        }

        // Pick an image from the gallery
        var mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
        {
            PhotoSize = PhotoSize.Medium,
        });

        if (mediaFile == null)
            return; // User canceled

        using (var memoryStream = new MemoryStream())
        {
            await mediaFile.GetStream().CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);

            BKDView.ScanImage(base64String, this);
        }
    }

    private void SetActiveBarcodeTypes()
    {
        //BarkoderConfig barkoderConfig = new BarkoderConfig(
        //    closeSessionOnResultEnabled: true,
        //    imageResultEnabled: false,
        //    roiOverlayBackgroundColor: 555555,
        //    decoder: new DekoderConfig(
        //        qr: new BarcodeConfig(true),
        //        ean13: new BarcodeConfig(true),
        //        code128: new BarcodeConfigWithLength(true, 0, 10),
        //        dotcode: new BarcodeConfig(true)
        //        ));
        //BKDView.ConfigureBarkoder(barkoderConfig);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.Ean13, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.IDDocument, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.UpcA, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.QR, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.QRMicro, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.Datamatrix, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.Code128, true);


    }

    private void OnStartScanningBtnClicked(object sender, EventArgs e)
	{
        BKDView.StartScanning(this);
        Console.WriteLine("Is master checksum enabled: " + BKDView.IsIdDocumentMasterChecksumEnabled);
     
    }

    public void DidFinishScanning(BarcodeResult[] result, ImageSource originalImageSource)
    {

        TextualDataLbl.Text = result[0].TextualData;
        //ScannedImage.Source = result[0].MrzImages[0].Image;
        //ScannedImage2.Source = result[0].MrzImages[1].Image;
        //ScannedImage3.Source = result[0].MrzImages[2].Image;
        //ScannedImage4.Source = result[0].MrzImages[3].Image;



    }

}


