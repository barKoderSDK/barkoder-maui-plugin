
## Methods

### StartCamera()
Starts the camera for barcode scanning.

**Syntax:**
```csharp
public void StartCamera()
```

### StartScanning()
Starts scanning for barcodes.

#### Parameters

`barkoderDelegate`: The delegate that will handle barcode scanning events. This delegate must implement the `IBarkoderDelegate` interface.

**Syntax:**
```csharp
public void StartScanning(IBarkoderDelegate barkoderDelegate)
```

### StopScanning()
Stops the barcode scanning process.

**Syntax:**
```csharp
public void StopScanning()
```

### PauseScanning()
Pauses the barcode scanning process.

**Syntax:**
```csharp
public void PauseScanning()
```

### SetFlashEnabled()
Sets the flash (torch) on or off for barcode scanning.

#### Parameters

`enabled`: True to enable the flash, false to disable it.

**Syntax:**
```csharp
public void SetFlashEnabled(bool enabled)
```

### SetZoomFactor()
Sets the zoom factor for the camera used in barcode scanning.

#### Parameters

`zoomFactor`: The zoom factor to set.

**Syntax:**
```csharp
public void SetZoomFactor(float zoomFactor)
```

### SetPinchToZoomEnabled()
Enables or disables pinch-to-zoom functionality for the camera used in barcode scanning.

#### Parameters

`enabled`: True to enable pinch-to-zoom, false to disable it.

**Syntax:**
```csharp
public void SetPinchToZoomEnabled(bool enabled)
```

### SetRegionOfInterestVisible()
Sets the visibility of the region of interest (ROI) overlay for barcode scanning.

#### Parameters

`visible`: True to make the ROI visible, false to hide it.

**Syntax:**
```csharp
public void SetRegionOfInterestVisible(bool visible)
```

### SetCloseSessionOnResultEnabled()
Sets whether the session should be closed upon detecting a result during barcode scanning.

#### Parameters

`enabled`: True to close the session on result detection, false otherwise

**Syntax:**
```csharp
public void SetCloseSessionOnResultEnabled(bool enabled)
```

### SetImageResultEnabled()
Enables or disables the display of image results during barcode scanning.

#### Parameters

`enabled`: True to enable image result display, false to disable it.

**Syntax:**
```csharp
public void SetImageResultEnabled(bool enabled)
```

### SetLocationInPreviewEnabled()
Enables or disables displaying the barcode location in the camera preview.

#### Parameters

`enabled`: True to display the location, false to hide it.

**Syntax:**
```csharp
public void SetLocationInPreviewEnabled(bool enabled)
```

### SetLocationInImageResultEnabled()
Enables or disables displaying the barcode location in the image result.

#### Parameters

`enabled`: True to display the location, false to hide it.

**Syntax:**
```csharp
public void SetLocationInImageResultEnabled(bool enabled)
```

### SetBeepOnSuccessEnabled()
Enables or disables the beep sound on successful barcode scanning.

#### Parameters

`enabled`: True to enable beep sound, false to disable it.

**Syntax:**
```csharp
public void SetBeepOnSuccessEnabled(bool enabled)
```

### SetVibrateOnSuccessEnabled()
Enables or disables the vibration on successful barcode scanning.

#### Parameters

`enabled`: True to enable vibration, false to disable it.

**Syntax:**
```csharp
public void SetVibrateOnSuccessEnabled(bool enabled)
```

### SetLocationLineColor()
Sets the color of the line indicating the barcode location in the camera preview.

#### Parameters

`hexColor`: The hexadecimal representation of the color.

**Syntax:**
```csharp
public void SetLocationLineColor(string hexColor)
```

### SetRoiLineColor()
Sets the color of the line indicating the region of interest (ROI) in the camera preview.

#### Parameters

`hexColor`: The hexadecimal representation of the color.

**Syntax:**
```csharp
public void SetRoiLineColor(string hexColor)
```

### SetRoiOverlayBackgroundColor()
Sets the background color of the region of interest (ROI) overlay in the camera preview.

#### Parameters

`hexColor`: The hexadecimal representation of the color.

**Syntax:**
```csharp
public void SetRoiOverlayBackgroundColor(string hexColor)
```

### SetRegionOfInterest()
Sets the region of interest (ROI) for barcode scanning within the camera preview.

#### Parameters

`left`: The left coordinate of the ROI.
`top`: The top coordinate of the ROI.
`width`: The width of the ROI.
`height`: The height of the ROI.

**Syntax:**
```csharp
public void SetRegionOfInterest(int left, int top, int width, int height)
```

### SetBarkoderResolution()
Sets the resolution for barcode scanning.

#### Parameters

`resolution`: The resolution to be set.

**Syntax:**
```csharp
public void SetBarkoderResolution(BarkoderResolution resolution)
```

### SetDecodingSpeed()
Sets the decoding speed for barcode scanning.

#### Parameters

`decodingSpeed`: The decoding speed to be set.

**Syntax:**
```csharp
public void SetDecodingSpeed(DecodingSpeed decodingSpeed)
```

### SetFormattingType()
Sets the formatting type for barcode scanning.

#### Parameters

`formattingType`: The formatting type to be set.

**Syntax:**
```csharp
public void SetFormattingType(FormattingType formattingType)
```

### SetMsiChecksumType()
Sets the checksum type for MSI barcodes.

#### Parameters

`msiChecksumType`: The MSI checksum type to be set.

**Syntax:**
```csharp
public void SetMsiChecksumType(MsiChecksumType msiChecksumType)
```

### SetCode11ChecksumType()
Sets the checksum type for Code 11 barcodes.

#### Parameters

`code11ChecksumType`: The Code 11 checksum type to be set.

**Syntax:**
```csharp
public void SetCode11ChecksumType(Code11ChecksumType code11ChecksumType)
```

### SetCode39ChecksumType()
Sets the checksum type for Code 39 barcodes.

#### Parameters

`code39ChecksumType`: The Code 39 checksum type to be set.

**Syntax:**
```csharp
public void SetCode39ChecksumType(Code39ChecksumType code39ChecksumType)
```

### SetEncodingCharacterSet()
Sets the encoding character set for barcode scanning.

#### Parameters

`encodingCharacterSet`: The encoding character set to be set.

**Syntax:**
```csharp
public void SetEncodingCharacterSet(string encodingCharacterSet)
```

### SetDatamatrixDpmModeEnabled()
Enables or disables the (DPM) mode for Datamatrix barcodes.

#### Parameters

`enabled`: True to enable DPM mode, false to disable it.

**Syntax:**
```csharp
public void SetDatamatrixDpmModeEnabled(bool enabled)
```

### SetUpcEanDeblurEnabled()
Enables or disables deblurring for UPC/EAN barcodes.

#### Parameters

`enabled`: True to enable deblurring, false to disable it.

**Syntax:**
```csharp
public void SetUpcEanDeblurEnabled(bool enabled)
```

### SetEnableMisshaped1DEnabled()
Enables or disables the decoding of misshaped 1D barcodes.

#### Parameters

`enabled`: True to enable decoding, false to disable it.

**Syntax:**
```csharp
public void SetEnableMisshaped1DEnabled(bool enabled)
```

### SetBarcodeThumbnailOnResultEnabled()
Enables or disables the display of barcode thumbnails in the scan results.

#### Parameters

`enabled`: True to enable barcode thumbnails, false to disable them.

**Syntax:**
```csharp
public void SetBarcodeThumbnailOnResultEnabled(bool enabled)
```

### SetMaximumResultsCount()
Sets the maximum number of results to be returned from barcode scanning.

#### Parameters

`maximumResultsCount`: The maximum number of results to return.

**Syntax:**
```csharp
public void SetMaximumResultsCount(int maximumResultsCount)
```

### SetDuplicatesDelayMs()
Sets the delay in milliseconds for considering duplicate barcodes during scanning.

#### Parameters

`duplicatesDelayMs`: The delay in milliseconds for duplicate detection.

**Syntax:**
```csharp
public void SetDuplicatesDelayMs(int duplicatesDelayMs)
```

### SetBarcodeTypeEnabled()
Enables or disables the specified barcode type for scanning.

#### Parameters

`barcodeType`: The barcode type to enable or disable.
`enabled`: True to enable the barcode type, false to disable it.

**Syntax:**
```csharp
public void SetBarcodeTypeEnabled(BarcodeType barcodeType, bool enabled)
```

### SetEnableVINRestrictions()
Sets whether Vehicle Identification Number (VIN) restrictions are enabled for scanning.

#### Parameters

`enabled`: True to enable VIN restrictions, false to disable them.

**Syntax:**
```csharp
public void SetEnableVINRestrictions(bool enabled)
```

### SetThresholdBetweenDuplicatesScans()
Sets the threshold between duplicate scans in milliseconds.

#### Parameters

`thresholdBetweenDuplicatesScans`: The threshold between duplicate scans in milliseconds.

**Syntax:**
```csharp
public void SetThresholdBetweenDuplicatesScans(int thresholdBetweenDuplicatesScans)
```

### IsBarcodeTypeEnabled()
Checks if a specific barcode type is enabled.

#### Parameters

`barcode`: The barcode type to check.

**Syntax:**
```csharp
public bool IsBarcodeTypeEnabled(BarcodeType barcode)
```

### SetBarcodeTypeLengthRange()
Sets the length range for a specified barcode type.

#### Parameters

`barcodeType`: The type of barcode.
`min`: The minimum length of the barcode.
`max`: The maximum length of the barcode.

**Syntax:**
```csharp
public void SetBarcodeTypeLengthRange(BarcodeType barcodeType, int min, int max)
```

## Properties

### Version
Gets barKoder SDK version.

**Syntax:**
```csharp
public bool Version { get; } 
```

### RegionOfInterestVisible Property
Gets or sets a value indicating whether the region of interest (ROI) overlay is visible.

**Syntax:**
```csharp
public bool RegionOfInterestVisible { get; set; } 
```

### IsFlashAvailable
Gets or sets a value indicating whether the flash is available.

**Syntax:**
```csharp
public bool IsFlashAvailable { get; set; }
```

### MaxZoomFactor
Gets or sets the maximum zoom factor

**Syntax:**
```csharp
public double MaxZoomFactor { get; set; }
```

### LocationLineColorHex
Gets or sets the hexadecimal representation of the location line color

**Syntax:**
```csharp
public string LocationLineColorHex { get; set; }
```

### RoiLineColorHex
Gets or sets the hexadecimal representation of the region of interest (ROI) line color.

**Syntax:**
```csharp
public string RoiLineColorHex { get; set; }
```

### RoiOverlayBackgroundColorHex
Gets or sets the hexadecimal representation of the region of interest (ROI) overlay background color.

**Syntax:**
```csharp
public string RoiOverlayBackgroundColorHex { get; set; }
```

### EncodingCharacterSet
Gets or sets the character set used for encoding.

**Syntax:**
```csharp
public string EncodingCharacterSet { get; set; }
```

### LocationLineWidth
Gets or sets the width of the location line.

**Syntax:**
```csharp
public double LocationLineWidth { get; set; }
```

### RoiLineWidth
Gets or sets the width of the region of interest (ROI) line.

**Syntax:**
```csharp
public double RoiLineWidth { get; set; }
```

### ImageResultEnabled
Gets or sets a value indicating whether image results are enabled.

**Syntax:**
```csharp
public bool ImageResultEnabled { get; set; }
```

### LocationInImageResultEnabled
Gets or sets a value indicating whether the location is displayed in image results.

**Syntax:**
```csharp
public bool LocationInImageResultEnabled { get; set; }
```

### LocationInPreviewEnabled
Gets or sets a value indicating whether the location is displayed in the camera preview.

**Syntax:**
```csharp
public bool LocationInPreviewEnabled { get; set; }
```

### PinchToZoomEnabled
Gets or sets a value indicating whether pinch-to-zoom functionality is enabled.

**Syntax:**
```csharp
public bool PinchToZoomEnabled { get; set; }
```

### BeepOnSuccessEnabled
Gets or sets a value indicating whether a beep sound is played on successful barcode scanning.

**Syntax:**
```csharp
public bool BeepOnSuccessEnabled { get; set; }
```

### VibrateOnSuccessEnabled
Gets or sets a value indicating whether vibration is enabled on successful barcode scanning.

**Syntax:**
```csharp
public bool VibrateOnSuccessEnabled { get; set; }
```

### CloseSessionOnResultEnabled
Gets or sets a value indicating whether the session is closed upon detecting a result during barcode scanning.

**Syntax:**
```csharp
public bool CloseSessionOnResultEnabled { get; set; }
```

### BarkoderResolution
Gets or sets the resolution for barcode scanning.

**Syntax:**
```csharp
public BarkoderResolution BarkoderResolution { get; set; }
```

### DecodingSpeed
Gets or sets the decoding speed for barcode scanning.

**Syntax:**
```csharp
public DecodingSpeed DecodingSpeed { get; set; }
```

### FormattingType
Gets or sets the formatting type for barcode scanning.

**Syntax:**
```csharp
public FormattingType FormattingType { get; set; }
```

### MsiChecksumType
Gets or sets the checksum type for MSI barcodes.

**Syntax:**
```csharp
public MsiChecksumType MsiChecksumType { get; set; }
```

### Code11ChecksumType
Gets or sets the checksum type for Code 11 barcodes.

**Syntax:**
```csharp
public Code11ChecksumType Code11ChecksumType { get; set; }
```

### Code39ChecksumType
Gets or sets the checksum type for Code 39 barcodes.

**Syntax:**
```csharp
public Code39ChecksumType Code39ChecksumType { get; set; }
```

### DatamatrixDpmModeEnabled
Gets or sets a value indicating whether Direct Part Marking (DPM) mode is enabled for Datamatrix barcodes.

**Syntax:**
```csharp
public bool DatamatrixDpmModeEnabled { get; set; }
```

### UpcEanDeblurEnabled
Gets or sets a value indicating whether deblurring is enabled for UPC/EAN barcodes.

**Syntax:**
```csharp
public bool UpcEanDeblurEnabled { get; set; }
```

### EnableMisshaped1DEnabled
Gets or sets a value indicating whether decoding of misshaped 1D barcodes is enabled.

**Syntax:**
```csharp
public bool EnableMisshaped1DEnabled { get; set; }
```

### BarcodeThumbnailOnResultEnabled
Gets or sets a value indicating whether barcode thumbnails are enabled in the scan results.

**Syntax:**
```csharp
public bool BarcodeThumbnailOnResultEnabled { get; set; }
```

### MaximumResultsCount
Gets or sets the maximum number of results to be returned from barcode scanning.

**Syntax:**
```csharp
public int MaximumResultsCount { get; set; }
```

### DuplicatesDelayMs
Gets or sets the delay in milliseconds for considering duplicate barcodes during scanning.

**Syntax:**
```csharp
public int DuplicatesDelayMs { get; set; }
```

### VINRestrictionsEnabled
Gets or sets a value indicating whether Vehicle Identification Number (VIN) restrictions are enabled for scanning.

**Syntax:**
```csharp
public bool VINRestrictionsEnabled { get; set; }
```

### ThresholdBetweenDuplicatesScans
Gets or sets the threshold between duplicate scans in seconds.

**Syntax:**
```csharp
public int ThresholdBetweenDuplicatesScans { get; set; }
```

### RegionOfInterest
Gets or sets the region of interest (ROI) for barcode scanning.

**Syntax:**
```csharp
public (int, int, int, int) RegionOfInterest { get; set; }
```

## Models, enums, interfaces

### BarcodeResult

- `textualData`: The textual data encoded in the barcode.
- `barcodeTypeName`: The type name of the barcode.
- `characterSet`: The character set used in encoding the barcode.


```csharp
    public class BarcodeResult
    {
        public string TextualData { get; set; }
        public string BarcodeTypeName { get; set; }
        public string CharacterSet { get; set; }

        public BarcodeResult(string textualData, string barcodeTypeName, string characterSet)
        {
            TextualData = textualData;
            BarcodeTypeName = barcodeTypeName;
            CharacterSet = characterSet;
        }
    }
```

### IBarkoderDelegate

Represents a delegate for handling barcode scanning events. Called when barcode scanning has finished.

```csharp
public interface IBarkoderDelegate
{
    void DidFinishScanning(BarcodeResult[] result);
}
```

### BarcodeType

Represents the types of barcodes that are supported by barKoder.

```csharp
public enum BarcodeType
{
    Aztec,
    AztecCompact,
    QR,
    QRMicro,
    Code128,
    Code93,
    Code39,
    Codabar,
    Code11,
    Msi,
    UpcA,
    UpcE,
    UpcE1,
    Ean13,
    Ean8,
    PDF417,
    PDF417Micro,
    Datamatrix,
    Code25,
    Interleaved25,
    Itf14,
    Uata25,
    Matrix25,
    Datalogic25,
    Coop25,
    Code32,
    Telepen,
    Dotcode
}
```

### BarkoderResolution

```csharp
public enum BarkoderResolution
{
    Normal,
    High
}
```

### DecodingSpeed

```csharp
public enum DecodingSpeed
{
    Fast,
    Normal,
    Slow
}
```

### FormattingType

```csharp
public enum FormattingType
{
    Disabled,
    Automatic,
    GS1,
    AAMVA
}
```

### Code11ChecksumType

```csharp
public enum Code11ChecksumType
{
    Disabled,
    Single,
    Double
}
```

### Code39ChecksumType

```csharp
public enum Code39ChecksumType
{
    Disabled,
    Enabled
}
```

### MsiChecksumType

```csharp
public enum MsiChecksumType
{
    Disabled,
    Mod10,
    Mod11,
    Mod1010,
    Mod1110,
    Mod11IBM,
    Mod1110IBM
}
```
