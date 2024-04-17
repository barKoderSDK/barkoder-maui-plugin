using Plugin.Maui.Barkoder.Handlers;
using Plugin.Maui.Barkoder.Interfaces;
using Plugin.Maui.Barkoder.Controls;

namespace BarkoderSample
{
	public class BarkoderExampleViewModel: IBarkoderDelegate
	{
        BarkoderView BKDView;

		public BarkoderExampleViewModel(BarkoderView barkoderView)
		{
            BKDView = barkoderView;

            // TODO: - Configuration goes here or on the ContentPage
        }

        public void StartScanning()
        {
            BKDView.StartScanning(this);
        }


        public void DidFinishScanning(BarcodeResult[] result)
        {
            // TODO: - Handle results here and/or pass to ContentPage 
        }

    }
}

