# BarkoderNET — Windows Integration Guide for MAUI

## What You Get

The `BarkoderNET` NuGet package contains everything needed for barcode decoding on Windows:

```
BarkoderNET.nupkg
├── lib/net8.0/
│   └── BarkoderNET.dll          ← Managed .NET 8 wrapper (your C# API)
├── runtimes/
│   ├── win-x64/native/
│   │   ├── BarkoderWrapper.dll  ← C interop layer
│   │   ├── Barkoder.dll         ← Native decoding engine
│   │   └── libiconv.dll         ← Character encoding support
│   └── win-arm64/native/
│       ├── BarkoderWrapper.dll
│       ├── Barkoder.dll
│       └── libiconv.dll
```

The OCR variant (`BarkoderNET.OCR.nupkg`) additionally includes `onnxruntime.dll` and OCR model files.

## Step 1 — Add the NuGet Package (Windows Only)

In your MAUI `.csproj`, add a conditional reference so it only applies to Windows:

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">
    <PackageReference Include="BarkoderNET" Version="1.7.0" />
</ItemGroup>
```

If you received `.nupkg` files instead of a NuGet feed, create a local source:

```bash
# Put the .nupkg file in a folder, e.g. C:\LocalNuGet\
nuget add BarkoderNET.1.7.0.nupkg -source C:\LocalNuGet

# Then in your nuget.config or csproj:
dotnet add package BarkoderNET --version 1.7.0 --source C:\LocalNuGet
```

Or add a `nuget.config` at the solution root:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="local" value="C:\LocalNuGet" />
  </packageSources>
</configuration>
```

## Step 2 — Platform-Specific Service Pattern

Since iOS/Android use native SDK views while Windows uses BarkoderNET for decoding only, use a platform-specific service:

```csharp
// Shared interface (in your shared project)
public interface IBarcodeDecoder
{
    List<BarcodeResult> Decode(byte[] pixels, int width, int height);
    void Dispose();
}
```

### Windows Implementation

```csharp
// Platforms/Windows/WindowsBarcodeDecoder.cs
#if WINDOWS
using BarkoderNET;

public class WindowsBarcodeDecoder : IBarcodeDecoder, IDisposable
{
    private readonly Config _config;

    public WindowsBarcodeDecoder(string licenseKey)
    {
        var result = Config.InitializeWithLicenseKey(licenseKey);
        if (result.Configuration == null)
            throw new Exception($"SDK init failed: {result.ErrorMessage}");
        
        _config = result.Configuration;
        
        // Configure decoders
        _config.MaximumResultsCount = 10;
        _config.DecodingSpeed = DecodingSpeed.Normal;
        _config.Formatting = Formatting.Automatic;
        
        _config.SetEnabledDecoders(new List<DecoderType>
        {
            DecoderType.QR, DecoderType.QRMicro,
            DecoderType.Code128, DecoderType.Code39,
            DecoderType.PDF417, DecoderType.Datamatrix,
            DecoderType.Ean13, DecoderType.Ean8,
            DecoderType.UpcA, DecoderType.UpcE,
            // add what you need
        });
    }

    public List<BarcodeResult> Decode(byte[] pixels, int width, int height)
    {
        // DecodeImageMemory expects raw pixel data
        var results = Barkoder.DecodeImageMemory(
            _config, pixels, width, height, BKColorFormat.Grayscale);
        
        // Map BarkoderNET results to your shared model
        return results.Select(r => new BarcodeResult
        {
            Type = r.BarcodeTypeName,
            Data = r.TextualData,
            // ... map other fields
        }).ToList();
    }

    public void Dispose()
    {
        _config?.Dispose();
    }
}
#endif
```

Register with DI in `MauiProgram.cs`:

```csharp
#if WINDOWS
builder.Services.AddSingleton<IBarcodeDecoder>(
    _ => new WindowsBarcodeDecoder("your-license-key"));
#endif
```

## Step 3 — Feeding Camera Frames

You handle the camera. The SDK expects raw pixel bytes. Common scenarios:

### From a `SoftwareBitmap` (WinUI camera capture)

```csharp
// Convert SoftwareBitmap to grayscale byte array
public static byte[] SoftwareBitmapToGrayscale(SoftwareBitmap bitmap)
{
    // Ensure Gray8 format
    if (bitmap.BitmapPixelFormat != BitmapPixelFormat.Gray8)
    {
        bitmap = SoftwareBitmap.Convert(bitmap, BitmapPixelFormat.Gray8);
    }

    var buffer = new byte[bitmap.PixelWidth * bitmap.PixelHeight];
    bitmap.CopyToBuffer(buffer.AsBuffer());
    return buffer;
}

// Then decode:
var pixels = SoftwareBitmapToGrayscale(frame);
var results = decoder.Decode(pixels, frame.PixelWidth, frame.PixelHeight);
```

### From a BGRA byte array

If you have BGRA data (e.g., from `WriteableBitmap`), pass it directly:

```csharp
var results = Barkoder.DecodeImageMemory(
    config, bgraBytes, width, height, BKColorFormat.BGRA);
```

### From an image file (for testing)

```csharp
using System.Drawing;          // Windows only
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public static (byte[] pixels, int w, int h) LoadImageAsGrayscale(string path)
{
    using var bmp = new Bitmap(path);
    using var gray = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height),
                               PixelFormat.Format8bppIndexed);
    
    var data = gray.LockBits(new Rectangle(0, 0, gray.Width, gray.Height),
                             ImageLockMode.ReadOnly, gray.PixelFormat);
    
    var bytes = new byte[data.Stride * data.Height];
    Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
    gray.UnlockBits(data);
    
    return (bytes, gray.Width, gray.Height);
}
```

## API Quick Reference

### Initialization

```csharp
var result = Config.InitializeWithLicenseKey("license-key");
// result.Success      — true if license is valid
// result.ErrorMessage  — error description if failed  
// result.Configuration — Config object (non-null even if license invalid = demo mode)
```

### Config Properties

| Property | Type | Description |
|----------|------|-------------|
| `MaximumResultsCount` | `int` | Max barcodes to return per frame |
| `DecodingSpeed` | `DecodingSpeed` | `Fast`, `Normal`, `Slow`, `Rigorous` |
| `Formatting` | `Formatting` | `Disabled`, `Automatic`, `GS1`, `AAMVA`, `SADL` |
| `RegionOfInterest` | `Rect` | Scan area (left, top, width, height as %) |
| `DuplicatesDelayMs` | `int` | Suppress duplicate results within N ms |
| `EnableMisshaped1D` | `int` | 1 = detect damaged 1D barcodes |
| `EnableVINRestrictions` | `int` | 1 = validate VIN format |
| `EnableComposite` | `int` | 1 = enable composite barcodes |
| `UpcEanDeblur` | `int` | 1 = deblur UPC/EAN |
| `EncodingCharacterSet` | `string` | Override character encoding |

### Per-Decoder Config

Each decoder type has its own config accessible as a property:

```csharp
config.QR.Enabled = true;
config.QR.ExpectedCount = 1;
config.QR.DpmMode = 1;              // Direct Part Marking
config.QR.MultiPartMerge = true;

config.Code39.Enabled = true;
config.Code39.Checksum = Code39ChecksumType.Enabled;
config.Code39.SetLengthRange(4, 20);

config.Msi.Enabled = true;
config.Msi.Checksum = MsiChecksumType.Mod1010;

config.UpcE.ExpandToUPCA = true;

config.Datamatrix.DpmMode = 1;
```

### Decoding

```csharp
List<BaseResult> results = Barkoder.DecodeImageMemory(
    config, pixelData, width, height, colorFormat);
```

### BaseResult Properties

| Property | Type | Description |
|----------|------|-------------|
| `BarcodeType` | `BarcodeType` | Enum value of detected type |
| `BarcodeTypeName` | `string` | Human-readable type name |
| `TextualData` | `string` | Decoded text content |
| `BinaryData` | `byte[]` | Raw binary content |
| `CharacterSet` | `string` | Encoding used |
| `Location` | `BKPoint[4]` | Corner coordinates |
| `PolygonLocation` | `List<BKPoint>` | Detailed polygon outline |
| `IsGS1` | `bool` | GS1 formatted |
| `HasLinkage` | `bool` | Composite linkage flag |
| `IsCached` | `bool` | From multicode cache |
| `TimestampMs` | `ulong` | Detection timestamp |
| `GetExtraData()` | `Dictionary<string, string>` | Extra data (GS1 fields, ID document fields, etc.) |

### Supported Color Formats

| `BKColorFormat` | Description |
|-----------------|-------------|
| `Grayscale` | 1 byte per pixel (fastest) |
| `YUV` | YUV format |
| `BGRA` | 4 bytes per pixel (Windows-native) |

### Utility Methods

```csharp
string version = Barkoder.GetLibVersion();
bool busy = Barkoder.IsDecoderBusy();
bool valid = Barkoder.IsStringVINCompliant("1HGBH41JXMN109186", 1);
```

## Memory Management

`Config` and `BaseResult` hold native resources. Always dispose:

```csharp
using var config = Config.InitializeWithLicenseKey("key").Configuration;

var results = Barkoder.DecodeImageMemory(config, pixels, w, h, BKColorFormat.Grayscale);
foreach (var r in results)
{
    Console.WriteLine($"{r.BarcodeTypeName}: {r.TextualData}");
    r.Dispose();  // free native memory
}
```

## OCR Support

If using the `BarkoderNET.OCR` package:

```csharp
config.OCRText.Enabled = true;
config.SetCustomOption("enable_ocr_functionality", 1);
```

OCR model files are automatically copied to the output directory by the NuGet package.

## Troubleshooting

| Problem | Solution |
|---------|----------|
| `DllNotFoundException` | Ensure `BarkoderWrapper.dll`, `Barkoder.dll`, `libiconv.dll` are in the output folder. Check your RuntimeIdentifier matches (win-x64 or win-arm64). |
| License error but results are masked | SDK works in demo mode without a valid license — results have masked characters. Contact barKoder for a license key. |
| No results returned | Check `DecodingSpeed` (try `Slow`), verify the decoder type is enabled, ensure image has sufficient resolution. |
| `ObjectDisposedException` | You're accessing a `Config` or `BaseResult` after calling `Dispose()`. |
