namespace BarkoderSample;

using Plugin.Maui.Barkoder.Handlers;
using Plugin.Maui.Barkoder.Interfaces;
using Barkoder;
using System.Threading.Tasks;

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

    private void SetBarkoderSettings()
    {
        // These are optional settings, otherwise default values will be used
        BKDView.SetImageResultEnabled(true);
        BKDView.SetLocationInPreviewEnabled(true);
        BKDView.SetRegionOfInterestVisible(true);
        BKDView.SetCloseSessionOnResultEnabled(true);

        BKDView.SetRegionOfInterest(5, 5, 90, 90);
    }

    private void SetActiveBarcodeTypes()
    {
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.Ean13, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.UpcA, true);
        BKDView.SetBarcodeTypeEnabled(Plugin.Maui.Barkoder.Enums.BarcodeType.QR, true);
    }

    private void OnStartScanningBtnClicked(object sender, EventArgs e)
    {
        BKDView.StartScanning(this);
    }

    public void DidFinishScanning(BarcodeResult[] result, ImageSource originalImageSource)
    {
        if (result.Length > 0)
        {
            TextualDataLbl.Text = result[0].TextualData;
        }
    }

}


