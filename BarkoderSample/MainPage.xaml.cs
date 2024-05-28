namespace BarkoderSample;

using Plugin.Maui.Barkoder.Handlers;
using Plugin.Maui.Barkoder.Interfaces;
using Barkoder;

public partial class MainPage : ContentPage, IBarkoderDelegate
{

    public MainPage()
	{
        InitializeComponent();

		SetUI();
    }

	private void SetUI()
	{
		//TitleLbl.Text = BKDView.Version;
    }

    private void OnStartScanningBtnClicked(object sender, EventArgs e)
	{
        BKDView.StartScanning(this);
    }

    // TODO: - For testing purposes only
	private void OnSecondBtnClicked(object sender, EventArgs e)
	{
        BKDView.SetRegionOfInterestVisible(true);
        BKDView.RoiLineWidth = 7;
        BKDView.SetCloseSessionOnResultEnabled(true);
        BKDView.SetImageResultEnabled(true);
        BKDView.SetLocationInImageResultEnabled(true);
        BKDView.SetLocationInPreviewEnabled(true);
        BKDView.SetBeepOnSuccessEnabled(true);
        BKDView.SetVibrateOnSuccessEnabled(true);
        BKDView.SetPinchToZoomEnabled(true);
        BKDView.SetLocationLineColor("#112233");
        BKDView.SetRoiLineColor("#445577");
        BKDView.SetRoiOverlayBackgroundColor("#002233");
        BKDView.LocationLineWidth = 10;
        BKDView.ImageResultEnabled = true;
        BKDView.SetLocationInImageResultEnabled(false);
        BKDView.LocationInPreviewEnabled = false;
        BKDView.PinchToZoomEnabled = false;
        BKDView.BeepOnSuccessEnabled = false;
        BKDView.VibrateOnSuccessEnabled = true;
        BKDView.CloseSessionOnResultEnabled = true;
        BKDView.SetRegionOfInterest(10, 10, 80, 80);
        BKDView.BarkoderResolution = Plugin.Maui.Barkoder.Enums.BarkoderResolution.High;
        BKDView.DecodingSpeed = Plugin.Maui.Barkoder.Enums.DecodingSpeed.Fast;
        BKDView.FormattingType = Plugin.Maui.Barkoder.Enums.FormattingType.GS1;
        BKDView.MsiChecksumType = Plugin.Maui.Barkoder.Enums.MsiChecksumType.Mod1110;
        BKDView.Code11ChecksumType = Plugin.Maui.Barkoder.Enums.Code11ChecksumType.Single;
        BKDView.Code39ChecksumType = Plugin.Maui.Barkoder.Enums.Code39ChecksumType.Disabled;
        BKDView.SetEncodingCharacterSet("string");

        BKDView.DatamatrixDpmModeEnabled = true;
        BKDView.UpcEanDeblurEnabled = true;
        BKDView.EnableMisshaped1DEnabled = true;
        BKDView.BarcodeThumbnailOnResultEnabled = true;

        BKDView.MaximumResultsCount = 10;
        BKDView.DuplicatesDelayMs = 20;

        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.QR, true);
        BKDView.CloseSessionOnResultEnabled = false;

        BKDView.ThresholdBetweenDuplicatesScans = 15;
        BKDView.VINRestrictionsEnabled = true;
    }

    private void OnThirdBtnClicked(object sender, EventArgs e)
    {
        // TODO: Random action
        SubtitleLbl.Text = BKDView.Version.ToString();

        BarkoderConfig barkoderConfig = new BarkoderConfig(
            closeSessionOnResultEnabled: true,
            imageResultEnabled: true,
            roiOverlayBackgroundColor: 555555,
            decoder: new DekoderConfig(
                qr: new BarcodeConfig(true),
                ean13: new BarcodeConfig(true),
                code128: new BarcodeConfigWithLength(true, 0, 10),
                dotcode: new BarcodeConfig(true)
                ));

        BKDView.ConfigureBarkoder(barkoderConfig);
    }

    public void DidFinishScanning(BarcodeResult[] result)
    {
        if (result.Length > 0)
        {
            SubtitleLbl.Text = result[0].TextualData;
        }
    }

    public void DidFinishScanning(BarcodeResult[] result, ImageSource originalImageSource)
    {
        if (result.Length > 0)
        {
            SubtitleLbl.Text = result[0].TextualData;
        }

        ResultImage.Source = originalImageSource;
    }

}


