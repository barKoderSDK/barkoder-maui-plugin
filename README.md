# barKoder .NET MAUI Barcode Scanner SDK

Integrate enterprise barcode scanning into **.NET MAUI applications** with `Plugin.Maui.Barkoder`. The plugin brings the native barKoder scanning engine to .NET MAUI projects and provides a XAML/C# integration path for Android and iOS applications, with Windows support available through barKoder's .NET MAUI offering where applicable.

barKoder is built for demanding mobile data-capture workflows including logistics, inventory, manufacturing, retail, automotive and identity verification.

## Quick links

- **.NET MAUI Barcode Scanner SDK:** [https://barkoder.com/barcode-scanner-sdk/frameworks/maui](https://barkoder.com/barcode-scanner-sdk/frameworks/maui)
- **NuGet package:** [https://www.nuget.org/packages/Plugin.Maui.Barkoder](https://www.nuget.org/packages/Plugin.Maui.Barkoder)
- **Installation guide:** [https://barkoder.com/docs/v1/maui/net-maui-installation](https://barkoder.com/docs/v1/maui/net-maui-installation)
- **Example:** [https://barkoder.com/docs/v1/maui/net-maui-example](https://barkoder.com/docs/v1/maui/net-maui-example)
- **API reference:** [https://barkoder.com/docs/v1/maui/net-maui-api-reference](https://barkoder.com/docs/v1/maui/net-maui-api-reference)
- **Full demo app:** [https://github.com/barKoderSDK/barkoder-net-maui-full-demo-app](https://github.com/barKoderSDK/barkoder-net-maui-full-demo-app)
- **Free trial:** [https://barkoder.com/trial](https://barkoder.com/trial)

## Key capabilities

barKoder is designed for production barcode capture workflows where speed and decode reliability matter. Depending on the license and configuration, the SDK supports capabilities such as:

- 30+ 1D and 2D barcode symbologies, including QR Code, Data Matrix, PDF417, Code 128, Code 39, EAN/UPC, Aztec, DotCode and GS1 formats
- [Direct Part Marking (DPM) scanning](https://barkoder.com/barcode-scanner-sdk/dpm) for difficult Data Matrix codes on metal, plastic and other industrial surfaces
- [Batch MultiScan](https://barkoder.com/barcode-scanner-sdk/batch-multiscan) for decoding multiple barcodes in a single camera view
- [VIN barcode scanning](https://barkoder.com/barcode-scanner-sdk/vin-scanning) for automotive workflows
- [MRZ scanning](https://barkoder.com/barcode-scanner-sdk/mrz) for passports, ID cards and travel documents
- Continuous scanning, image/gallery scanning and configurable regions of interest
- Advanced decoding for damaged, deformed, low-quality and blurry barcodes
- On-device scanning for normal mobile scanning workflows

For the complete feature set and platform-specific configuration options, use the official documentation linked below.


## Installation

Install the package from NuGet:

```bash
dotnet add package Plugin.Maui.Barkoder
```

or add `Plugin.Maui.Barkoder` through your IDE's NuGet package manager.

Follow the [official .NET MAUI installation guide](https://barkoder.com/docs/v1/maui/net-maui-installation) for handler registration, platform permissions and project configuration.

## Add `BarkoderView` in XAML

Declare the barKoder namespace:

```xml
xmlns:barkoder="clr-namespace:Plugin.Maui.Barkoder.Controls;assembly=Plugin.Maui.Barkoder"
```

Then add the scanner view:

```xml
<barkoder:BarkoderView
    x:Name="barkoderView"
    LicenseKey="YOUR_LICENSE_KEY" />
```

Use the [MAUI example](https://barkoder.com/docs/v1/maui/net-maui-example) and [API reference](https://barkoder.com/docs/v1/maui/net-maui-api-reference) for current scanning, configuration and result-handling code.

## Windows

This repository contains Windows-related project material in addition to the mobile plugin. Check the [.NET MAUI product page](https://barkoder.com/barcode-scanner-sdk/frameworks/maui) or contact barKoder support for the current Windows package/distribution applicable to your project.

## Trial license

You can evaluate barKoder in your own application with a free trial license:

**[Get a free barKoder SDK trial](https://barkoder.com/trial)**

The SDK can be initialized without a valid license for integration testing, but decoded results may be partially masked or marked as unlicensed. Use a valid trial or production license for complete results and licensed functionality.

Do not publish a trial license in a production application or public source repository.


## Support

Need help with integration or testing?

- Documentation: [https://barkoder.com/docs/v1/home](https://barkoder.com/docs/v1/home)
- Technical support: [support@barkoder.com](mailto:support@barkoder.com)
- Sales and licensing: [sales@barkoder.com](mailto:sales@barkoder.com)

## License

See the `LICENSE` file in this repository for the terms applicable to the repository contents. Use of the barKoder SDK itself is subject to the applicable barKoder license agreement.
