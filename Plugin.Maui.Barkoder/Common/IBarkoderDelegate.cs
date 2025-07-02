using System;
using Plugin.Maui.Barkoder.Handlers;

namespace Plugin.Maui.Barkoder.Interfaces;

public interface IBarkoderDelegate
{
    // Default 2-argument implementation (empty)
    void DidFinishScanning(BarcodeResult[] result, ImageSource originalImageSource)
    {
    }

    // Default 3-argument implementation (calls 2-arg version by default)
    void DidFinishScanning(BarcodeResult[] result, ImageSource[] thumbnails, ImageSource originalImageSource)
    {
        DidFinishScanning(result, originalImageSource);
    }

}

