using System;
using Plugin.Maui.Barkoder.Handlers;

namespace Plugin.Maui.Barkoder.Interfaces;

public interface IBarkoderDelegate
{
    void DidFinishScanning(BarcodeResult[] result, ImageSource originalImageSource);
}

