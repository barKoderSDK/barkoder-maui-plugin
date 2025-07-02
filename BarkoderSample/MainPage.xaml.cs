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
       
        //BKDView.SetImageResultEnabled(false);
        //BKDView.SetLocationInPreviewEnabled(true);
        //BKDView.SetRegionOfInterestVisible(true);
        BKDView.SetCloseSessionOnResultEnabled(false);
        BKDView.SetLocationInImageResultEnabled(true);
        //BKDView.SetImageResultEnabled(true);
        //BKDView.SetBarcodeThumbnailOnResultEnabled(true);
        //BKDView.SetVibrateOnSuccessEnabled(false);
        //BKDView.SetRegionOfInterest(5, 5, 90, 90);
        //BKDView.SetMaximumResultsCount(200);
        //BKDView.SetBeepOnSuccessEnabled(true);
        //BKDView.SetThresholdBetweenDuplicatesScans(5);
        //BKDView.SetShowDuplicatesLocation(true);
        //BKDView.LocationLineWidth = 30.3;
        BKDView.SetBarkoderARMode(Plugin.Maui.Barkoder.Enums.BarkoderARMode.InteractiveEnabled);
        BKDView.SetRoiLineColor("#00FF00");
        //BKDView.InitCameraProperties();
        Console.WriteLine("zoooooom: " + BKDView.MaxZoomFactor);
        //BKDView.RoiLineWidth = 20;
        BKDView.SetPinchToZoomEnabled(true);
        Console.WriteLine("zoooooom: " + BKDView.IsFlashAvailable);
        //BKDView.BarkoderARMode = Plugin.Maui.Barkoder.Enums.BarkoderARMode.InteractiveEnabled;
        //BKDView.BarkoderARHeaderShowMode = Plugin.Maui.Barkoder.Enums.BarkoderARHeaderShowMode.ALWAYS;
        //BKDView.BarkoderARLocationType = Plugin.Maui.Barkoder.Enums.BarkoderARLocationType.TIGHT;

        //BKDView.ARHeaderHeight = 50;


        //BKDView.ARDoubleTapToFreezeEnabled = false;
        //BKDView.ARHeaderTextFormat = "[barcode_type]";
        //BKDView.ARNonSelectedLocationLineColorHex = "#00FF00";
        //BKDView.ARSelectedLocationLineColorHex = "#FF0000";
        //BKDView.ARSelectedHeaderTextColorHex = "#00FF00";
        //BKDView.ARNonSelectedHeaderTextColorHex = "#FF0000";
        //BKDView.ARSelectedLocationLineWidth = 10.2;
        //BKDView.ARNonSelectedLocationLineWidth = 7.2;
        //BKDView.ResultDisappearanceDelayMs = 4000;
        //BKDView.ARLocationTransitionSpeed = 0.1;
        //BKDView.HeaderARHorizontalTextMargin = 25.2;
        //BKDView.HeaderARVerticalTextMargin = 65.2;
        //BKDView.SetARHeaderMaxTextHeight(10.2);


        //BKDView.SetBarkoderARMode(Plugin.Maui.Barkoder.Enums.BarkoderARMode.InteractiveDisabled);
        //BKDView.SetBarkoderARHeaderShowMode(Plugin.Maui.Barkoder.Enums.BarkoderARHeaderShowMode.ALWAYS);
        //BKDView.SetBarkoderARLocationType(Plugin.Maui.Barkoder.Enums.BarkoderARLocationType.BOUNDINGBOX);
        //BKDView.SetBarkoderARoverlayRefresh(Plugin.Maui.Barkoder.Enums.BarkoderAROverlayRefresh.SMOOTH);


        BKDView.SetARImageResultEnabled(true);
        BKDView.SetARBarcodeThumbnailOnResultEnabled(true);
        BKDView.SetPinchToZoomEnabled(true);
        BKDView.SetBarkoderResolution(Plugin.Maui.Barkoder.Enums.BarkoderResolution.FHD);
        BKDView.SetDecodingSpeed(Plugin.Maui.Barkoder.Enums.DecodingSpeed.Rigorous);
        BKDView.SetDecodingSpeed(Plugin.Maui.Barkoder.Enums.DecodingSpeed.Fast);
        BKDView.SetDecodingSpeed(Plugin.Maui.Barkoder.Enums.DecodingSpeed.Normal);
        BKDView.SetDecodingSpeed(Plugin.Maui.Barkoder.Enums.DecodingSpeed.Slow);
     
      
        //BKDView.SetARNonSelectedLocationLineColor("#00FF00");
        //BKDView.SetARSelectedLocationLineColor("#FF0000");
        //BKDView.SetARHeaderMaxTextHeight(30.2);
        //BKDView.SetARHeaderMinTextHeight(2.3);
        //BKDView.SetARHeaderHeight(40.2);
        //BKDView.SetARHeaderHorizontalTextMargin(10.3);
        //BKDView.SetARNonSelectedHeaderTextColor("#FF0000");
        //BKDView.SetARSelectedHeaderTextColor("#00FF00");
        //BKDView.SetARDoubleTapToFreezeEnabled(true);
        //BKDView.SetResultDisappearanceDelayMs(4000);
        //BKDView.SetARLocationTransitionSpeed(0.3);
        //BKDView.SetARSelectedLocationLineWidth(3.2);
        //BKDView.SetARNonSelectedLocationLineWidth(6.2);
        //BKDView.SetARHeaderTextFormat("[barcode_type]");






    }

    private async void OnPickImageButtonClicked(object sender, EventArgs e)
    {

        BKDView.UnfreezeScanning();
        //// Ensure the media plugin is initialized
        //await CrossMedia.Current.Initialize();

        //if (!CrossMedia.Current.IsPickPhotoSupported)
        //{
        //    // Gallery picking is not supported on this device
        //    await DisplayAlert("Error", "Gallery picking is not supported on this device.", "OK");
        //    return;
        //}

        //// Pick an image from the gallery
        //var mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
        //{
        //    PhotoSize = PhotoSize.Medium,
        //});

        //if (mediaFile == null)
        //    return; // User canceled

        //using (var memoryStream = new MemoryStream())
        //{
        //    await mediaFile.GetStream().CopyToAsync(memoryStream);
        //    byte[] imageBytes = memoryStream.ToArray();
        //    string base64String = Convert.ToBase64String(imageBytes);

        //    BKDView.ScanImage(base64String, this);
        //}

    }

    private void SetActiveBarcodeTypes()
    {
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.Ean13, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.UpcA, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.QR, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.QRMicro, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.Datamatrix, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.Code128, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.Maxicode, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.Msi, true);

        //BarkoderConfig barkoderConfig = new BarkoderConfig(
        //    closeSessionOnResultEnabled: true,
        //    imageResultEnabled: false,
        //    roiOverlayBackgroundColor: 555555,
        //    decoder: new DekoderConfig(
        //        ean13: new BarcodeConfig(true),
        //        code128: new BarcodeConfigWithLength(true, 0, 10),
        //        dotcode: new BarcodeConfig(true),
        //         postalIMB: new BarcodeConfig(true),
        //         postnet: new BarcodeConfig(true),
        //         idDocument: new BarcodeConfig(true)
        //        ));
        //BKDView.ConfigureBarkoder(barkoderConfig);

    }

    private void OnStartScanningBtnClicked(object sender, EventArgs e)
	{
        BKDView.StartScanning(this);

        Console.WriteLine("zoooooom current: " + BKDView.DecodingSpeed);

        Console.WriteLine("zoooooom: " + BKDView.IsFlashAvailable);
    }

    private void OnStopScanningBtnClicked(object sender, EventArgs e)
    {
        Console.WriteLine("Current Zoom Factor: " + BKDView.GetCurrentZoomFactor());

     
        //BKDView.StopScanning();
        //BKDView.FreezeScanning();
        BKDView.SetZoomFactor(3);

        Console.WriteLine("Is master checksum enabled: " + BKDView.IsIdDocumentMasterChecksumEnabled);

    }

    //public void DidFinishScanning(BarcodeResult[] result, ImageSource[] thumbnails, ImageSource originalImageSource)
    //{
    //    if (thumbnails.Length > 2)
    //    {
    //        ScannedImage.Source = thumbnails[0];
    //        ScannedImage2.Source = thumbnails[1];
    //        ScannedImage3.Source = thumbnails[2];

    //    }
    //    else if (thumbnails.Length > 1)
    //    {
    //        ScannedImage.Source = thumbnails[0];
    //        ScannedImage2.Source = thumbnails[1];
    //    }
    //    else if (thumbnails.Length > 0)
    //    {
    //        ScannedImage.Source = thumbnails[0];
    //    }

    //    if (originalImageSource != null)
    //    {
    //        ScannedImage4.Source = originalImageSource;
    //    }

    //    if (result.Length > 0)
    //    {
    //        TextualDataLbl.Text = result[0].TextualData ?? "No data";
    //        TextualDataLbl2.Text = BKDView.GetCurrentZoomFactor().ToString();

    //    }
    //}

    public void DidFinishScanning(BarcodeResult[] result, ImageSource originalImageSource)
    {
        if (result == null || result.Length == 0)
        {
            Console.WriteLine("No barcode detected.");
            TextualDataLbl.Text = "No barcode detected.";
            TextualDataLbl2.Text = "";
            return;
        }


        if (result[0] != null)
        {

            TextualDataLbl.Text = result[0].TextualData ?? "No data";
            TextualDataLbl2.Text = BKDView.GetCurrentZoomFactor().ToString();
            ScannedImage2.Source = originalImageSource;
        }
    }



}


