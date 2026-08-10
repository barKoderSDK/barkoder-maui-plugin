# Barkoder Windows SDK

Barcode scanner SDK for Windows, with an API surface matching the Android and iOS Barkoder SDKs.

## Project Structure

```
windows-maui/
├── BarkoderSDK.sln
├── BarkoderSDK/                    # Core SDK library
│   ├── BarkoderView.cs             # Main scanner (startScanning/stopScanning)
│   ├── Camera/
│   │   ├── ICameraProvider.cs      # Camera abstraction interface
│   │   └── CameraManager.cs       # Internal camera management
│   ├── Config/
│   │   ├── BarkoderConfig.cs       # Main configuration class
│   │   └── DecoderConfig.cs        # Decoder-specific settings
│   ├── Decoder/
│   │   └── BarcodeDecoder.cs       # ZXing.Net-based barcode decoder
│   ├── Models/
│   │   ├── Enums.cs                # BarkoderState, DecoderType, etc.
│   │   ├── BarcodeResult.cs        # Scan result model (DecoderResult.cs)
│   │   └── Delegates.cs            # Callback signatures
│   └── Utils/
│       └── BarkoderHelper.cs       # Static utilities & templates
```

## Quick Start

```csharp
using Barkoder.Windows;
using Barkoder.Windows.Config;
using Barkoder.Windows.Models;

// 1. Create config
var config = new BarkoderConfig("YOUR_LICENSE_KEY");

// 2. Enable barcode types
config.DecoderConfig.SetEnabledDecoders(
    DecoderType.QR,
    DecoderType.Code128,
    DecoderType.EAN13
);

// 3. Configure behavior
config.BeepOnSuccessEnabled = true;
config.CloseSessionOnResultEnabled = false;
config.BarkoderResolution = BarkoderResolution.FHD;

// 4. Create scanner view
var bkdView = new BarkoderView();
bkdView.Config = config;

// 5. Set camera provider (platform-specific implementation)
bkdView.SetCameraProvider(myCameraProvider);

// 6. Start scanning
await bkdView.StartScanning((results, thumbnails, resultImage) =>
{
    foreach (var result in results)
    {
        Console.WriteLine($"[{result.BarcodeTypeName}] {result.TextualData}");
    }
});

// 7. Stop when done
await bkdView.StopScanning();
```

## API Reference

### BarkoderView (Main Class)

| Method | Description |
|--------|-------------|
| `StartScanning(callback)` | Start scanning with result callback |
| `StopScanning()` | Stop scanning and release camera |
| `PauseScanning()` | Pause decoding (camera stays alive) |
| `ResumeScanning()` | Resume after pause |
| `StartCameraAsync()` | Start camera preview without decoding |
| `SetCamera(position)` | Switch front/back camera |
| `SetZoomFactor(factor)` | Set camera zoom |
| `SetFlashEnabled(enabled)` | Toggle flash/torch |
| `CaptureImageAsync()` | Capture current frame as PNG bytes |
| `SetCameraProvider(provider)` | Set the platform camera implementation |

### BarkoderConfig

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `CloseSessionOnResultEnabled` | bool | true | Auto-stop after decode |
| `BeepOnSuccessEnabled` | bool | true | Beep on success |
| `VibrateOnSuccessEnabled` | bool | true | Vibrate on success |
| `PinchToZoomEnabled` | bool | false | Pinch-to-zoom gesture |
| `BarkoderResolution` | enum | HD | Camera resolution |
| `ImageResultEnabled` | bool | false | Include frame in result |
| `BarcodeThumbnailOnResult` | bool | true | Crop barcode thumbnails |
| `RegionOfInterestVisible` | bool | true | Show ROI overlay |
| `LocationInPreviewEnabled` | bool | false | Show barcode location |
| `ThresholdBetweenDuplicatesScans` | int | 5 | Seconds between duplicate scans |
| `PowerSavingMode` | int | 0 | Power saving (0-4) |
| `ThreadsLimit` | int | cores/2 | Max decoder threads |

### DecoderConfig

```csharp
config.DecoderConfig.SetEnabledDecoders(DecoderType.QR, DecoderType.Code128);
config.DecoderConfig.EnableDecoder(DecoderType.EAN13);
config.DecoderConfig.DisableDecoder(DecoderType.QR);
bool isEnabled = config.DecoderConfig.IsDecoderEnabled(DecoderType.QR);
```

### BarcodeResult

| Property | Type | Description |
|----------|------|-------------|
| `TextualData` | string | Decoded barcode content |
| `BarcodeType` | DecoderType | Barcode format enum |
| `BarcodeTypeName` | string | Human-readable format name |
| `Location` | BarcodeLocation | Corner points in the image |
| `Extra` | Dictionary | Metadata (GS1, MRZ, etc.) |
| `Confidence` | float | Decode confidence (0-1) |

### BarkoderHelper (Static Utilities)

```csharp
// Scan from raw pixel data
BarkoderHelper.ScanImage(bgra8Data, width, height, config, callback);

// Apply a predefined template
BarkoderHelper.ApplyConfigSettingsFromTemplate(config, BarkoderConfigTemplate.QR);

// Load config from JSON
BarkoderHelper.ApplyConfigSettingsFromJson(config, jsonString);

// Load config from file
await BarkoderHelper.ApplyConfigSettingsFromFile(config, "config.json");
```

### ICameraProvider (Camera Abstraction)

To use the SDK, implement `ICameraProvider` for your UI framework:

```csharp
public class MyMediaCaptureProvider : ICameraProvider
{
    public bool IsInitialized { get; private set; }
    public double CurrentFps { get; private set; }

    public async Task InitializeAsync(BarkoderCameraPosition position, BarkoderResolution resolution) { ... }
    public async Task StartFrameDeliveryAsync(Action<byte[], int, int> frameCallback) { ... }
    public async Task StopFrameDeliveryAsync() { ... }
    // ... etc.
}
```

## Supported Barcode Types

| 1D Codes | 2D Codes |
|----------|----------|
| Code128 | QR |
| Code93 | DataMatrix |
| Code39 | PDF417 |
| Code11 | Aztec |
| Codabar | MicroQR |
| EAN-13 | MicroPDF417 |
| EAN-8 | MaxiCode |
| UPC-A | DotCode |
| UPC-E | |
| Interleaved 2of5 | |
| ITF-14 | |
| GS1 Databar | |

## Architecture

The SDK follows the same layered architecture as the Android and iOS SDKs:

1. **BarkoderView** — Main API entry point, manages scanning lifecycle
2. **BarkoderConfig** — All scanner settings (resolution, ROI, decoders, behavior)
3. **CameraManager** — Delegates to `ICameraProvider` for platform camera access
4. **BarcodeDecoder** — Frame processing and barcode detection via ZXing.Net
5. **BarkoderHelper** — Utility methods for templates, image scanning, JSON config
