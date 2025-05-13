using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Barkoder.Xamarin
{
    // @interface BarkoderView : NSObject
    [BaseType(typeof(NSObject))]
    interface BarkoderView
    {
        // @property (readonly, nonatomic, strong) UIView * _Nonnull view;
        [Export("view", ArgumentSemantic.Strong)]
        UIView View { get; }

        // @property (nonatomic, strong) BKDConfig * _Nullable config;
        [NullAllowed, Export("config", ArgumentSemantic.Strong)]
        BarkoderConfig Config { get; set; }

        // -(void)startScanningWithCompletion:(void (^ _Nonnull)(DecoderPayload * _Nonnull))completion;
        [Export("startScanningWithCompletion:")]
        void StartScanning(Action<DecoderPayload> completion);

        [Export("scanImageWithCompletion:bkdConfig:completion:")]
        void ScanImage(UIImage image, BarkoderConfig bkdConfig, Action<DecoderPayload> completion);

        // -(void)startCamera;
        [Export("startCamera")]
        void StartCamera();

        // -(void)stopScanning;
        [Export("stopScanning")]
        void StopScanning();

        // -(void)pauseScanning;
        [Export("pauseScanning")]
        void PauseScanning();

        [Export("freezeScanning")]
        void FreezeScanning();

        [Export("unfreezeScanning")]
        void UnfreezeScanning();

        // -(void)isFlashAvailableWithCompletion:(void (^ _Nonnull)(BOOL))completion;
        [Export("isFlashAvailableWithCompletion:")]
        void IsFlashAvailableWithCompletion(Action<bool> completion);

        // -(void)setFlashEnabledWithArg:(BOOL)arg;
        [Export("setFlashEnabledWithArg:")]
        void SetFlashEnabledWithArg(bool arg);

        // -(void)getMaxZoomFactorWithCompletion:(void (^ _Nonnull)(float))completion;
        [Export("getMaxZoomFactorWithCompletion:")]
        void GetMaxZoomFactorWithCompletion(Action<float> completion);

        // -(void)setZoomFactorWithArg:(float)arg;
        [Export("setZoomFactorWithArg:")]
        void SetZoomFactorWithArg(float arg);

        // -(void)setLocationLineWidthWithArg:(float)arg;
        [Export("setLocationLineWidthWithArg:")]
        void SetLocationLineWidthWithArg(float arg);

        // -(NSString * _Nonnull)getLocationLineColorHex __attribute__((warn_unused_result("")));
        [Export("getLocationLineColorHex")]
        string LocationLineColorHex { get; }

        // -(void)setLocationLineColorWithArg:(NSString * _Nonnull)arg;
        [Export("setLocationLineColorWithArg:")]
        void SetLocationLineColorWithArg(string arg);

        // -(void)setRoiLineColorWithArg:(NSString * _Nonnull)arg;
        [Export("setRoiLineColorWithArg:")]
        void SetRoiLineColorWithArg(string arg);

        // -(void)setRoiLineWidthWithArg:(float)arg;
        [Export("setRoiLineWidthWithArg:")]
        void SetRoiLineWidthWithArg(float arg);

        // -(void)setRoiOverlayBackgroundColorWithArg:(NSString * _Nonnull)arg;
        [Export("setRoiOverlayBackgroundColorWithArg:")]
        void SetRoiOverlayBackgroundColorWithArg(string arg);

        // -(void)setCloseSessionOnResultEnabledWithArg:(BOOL)arg;
        [Export("setCloseSessionOnResultEnabledWithArg:")]
        void SetCloseSessionOnResultEnabledWithArg(bool arg);

        // -(void)setImageResultEnabledWithArg:(BOOL)arg;
        [Export("setImageResultEnabledWithArg:")]
        void SetImageResultEnabledWithArg(bool arg);

        // -(void)setLocationInImageResultEnabledWithArg:(BOOL)arg;
        [Export("setLocationInImageResultEnabledWithArg:")]
        void SetLocationInImageResultEnabledWithArg(bool arg);

        // -(void)setThresholdBetweenDuplicatesScansWithArg:(NSInteger)arg;
        [Export("setThresholdBetweenDuplicatesScansWithArg:")]
        void SetThresholdBetweenDuplicatesScansWithArg(nint arg);

        // -(void)setLocationInPreviewEnabledWithArg:(BOOL)arg;
        [Export("setLocationInPreviewEnabledWithArg:")]
        void SetLocationInPreviewEnabledWithArg(bool arg);

        // -(void)setPinchToZoomEnabledWithArg:(BOOL)arg;
        [Export("setPinchToZoomEnabledWithArg:")]
        void SetPinchToZoomEnabledWithArg(bool arg);

        // -(void)setRegionOfInterestVisibleWithArg:(BOOL)arg;
        [Export("setRegionOfInterestVisibleWithArg:")]
        void SetRegionOfInterestVisibleWithArg(bool arg);

        // -(void)setRegionOfInterestWithLeft:(NSInteger)left top:(NSInteger)top width:(NSInteger)width height:(NSInteger)height;
        [Export("setRegionOfInterestWithLeft:top:width:height:")]
        void SetRegionOfInterestWithLeft(nint left, nint top, nint width, nint height);

        // -(void)setBeepOnSuccessEnabledWithArg:(BOOL)arg;
        [Export("setBeepOnSuccessEnabledWithArg:")]
        void SetBeepOnSuccessEnabledWithArg(bool arg);

        // -(void)setVibrateOnSuccessEnabledWithArg:(BOOL)arg;
        [Export("setVibrateOnSuccessEnabledWithArg:")]
        void SetVibrateOnSuccessEnabledWithArg(bool arg);

        // @property (readonly, copy, nonatomic) NSString * _Nonnull encodingCharacterSet;
        [Export("encodingCharacterSet")]
        string EncodingCharacterSet { get; }

        // @property (readonly, nonatomic) NSInteger decodingSpeed;
        [Export("decodingSpeed")]
        DecodingSpeed DecodingSpeed { get; }

        // @property (readonly, nonatomic) NSInteger formattingType;
        [Export("formattingType")]
        FormattingType FormattingType { get; }

        // @property (readonly, nonatomic) NSInteger locationLineWidth;
        [Export("locationLineWidth")]
        nint LocationLineWidth { get; }

        // -(NSString * _Nonnull)getRoiLineColorHex __attribute__((warn_unused_result("")));
        [Export("getRoiLineColorHex")]
        string RoiLineColorHex { get; }

        [Export("getARSelectedLocationLineColor")]
        string ARSelectedLocationLineColor { get; }

        [Export("getARNonSelectedLocationLineColor")]
        string ARNonSelectedLocationLineColor { get; }

        [Export("getARHeaderHeight")]
        float ARHeaderHeight { get; }

        [Export("getARSelectedLocationWidth")]
        float ARSelectedLocationWidth { get; }

        [Export("getARNonSelectedLocationWidth")]
        float ARNonSelectedLocationWidth { get; }

        [Export("getScanningIndicatorColorHex")]
        string ScanningIndicatorColorHex { get; }

        [Export("getResultDisappereanceDelayMs")]
        nint ResultDisapperanceDelayMs { get; }

        [Export("getLocationTransitionSpeed")]
        float LocationTransitionSpeed { get; }

        [Export("getARDoubleTapToFreeze")]
        bool ARDoubleTapToFreeze { get; }

        [Export("getARHeaderMaxTextHeight")]
        float ARHeaderMaxTextHeight { get; }

        [Export("getARHeaderMinTextHeight")]
        float ARHeaderMinTextHeight { get; }

        [Export("getARSelectedHeaderTextColor")]
        string ARSelectedHeaderTextColor { get; }

        [Export("getARNonSelectedHeaderTextColor")]
        string ARNonSelectedHeaderTextColor { get; }

        [Export("getARHeaderHorizontalTextMargin")]
        float ARHeaderHorizontalTextMargin { get; }


        [Export("getARHeaderVerticalTextMargin")]
        float ARHeaderVerticalTextMargin { get; }

        [Export("getARHeaderTextFormat")]
        string ARHeaderTextFormat { get; }


        // @property (readonly, nonatomic) NSInteger roiLineWidth;
        [Export("roiLineWidth")]
        nint RoiLineWidth { get; }

        [Export("scanningIndicatorWidth")]
        nint ScanningIndicatorWidth { get; }

         [Export("scanningIndicatorAnimationMode")]
        nint ScanningIndicatorAnimationMode { get; }

        // -(NSString * _Nonnull)getRoiOverlayBackgroundColorHex __attribute__((warn_unused_result("")));
        [Export("getRoiOverlayBackgroundColorHex")]
        string RoiOverlayBackgroundColorHex { get; }

        // @property (readonly, nonatomic) BOOL closeSessionOnResultEnabled;
        [Export("closeSessionOnResultEnabled")]
        bool CloseSessionOnResultEnabled { get; }

        [Export("scanningIndicatorAlwaysVisible")]
        bool ScanningIndicatorAlwaysVisible { get; }

        // @property (readonly, nonatomic) BOOL imageResultEnabled;
        [Export("imageResultEnabled")]
        bool ImageResultEnabled { get; }

        // @property (readonly, nonatomic) BOOL locationInImageResultEnabled;
        [Export("locationInImageResultEnabled")]
        bool LocationInImageResultEnabled { get; }

        // -(NSArray<NSNumber *> * _Nonnull)getRegionOfInterest __attribute__((warn_unused_result("")));
        [Export("getRegionOfInterest")]
        NSNumber[] RegionOfInterest { get; }

        // @property (readonly, nonatomic) NSInteger threadsLimit;
        [Export("threadsLimit")]
        nint ThreadsLimit { get; }

        // -(void)setThreadsLimitWithThreadsLimit:(NSInteger)threadsLimit;
        [Export("setThreadsLimitWithThreadsLimit:")]
        void SetThreadsLimitWithThreadsLimit(nint threadsLimit);

        // @property (readonly, nonatomic) BOOL isLocationInPreviewEnabled;
        [Export("isLocationInPreviewEnabled")]
        bool IsLocationInPreviewEnabled { get; }

        // @property (readonly, nonatomic) BOOL isPinchToZoomEnabled;
        [Export("isPinchToZoomEnabled")]
        bool IsPinchToZoomEnabled { get; }

        // @property (readonly, nonatomic) BOOL isRegionOfInterestVisible;
        [Export("isRegionOfInterestVisible")]
        bool IsRegionOfInterestVisible { get; }

        // @property (readonly, nonatomic) BOOL isBeepOnSuccessEnabled;
        [Export("isBeepOnSuccessEnabled")]
        bool IsBeepOnSuccessEnabled { get; }

        // @property (readonly, nonatomic) BOOL isVibrateOnSuccessEnabled;
        [Export("isVibrateOnSuccessEnabled")]
        bool IsVibrateOnSuccessEnabled { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nonnull version;
        [Export("version")]
        string Version { get; }

        // @property (readonly, nonatomic) NSInteger msiChecksumType;
        [Export("msiChecksumType")]
        MsiChecksumType MsiChecksumType { get; }

        // -(void)setMsiChecksumTypeWithArg:(NSInteger)arg;
        [Export("setMsiChecksumTypeWithArg:")]
        void SetMsiChecksumTypeWithArg(MsiChecksumType arg);

        // @property (readonly, nonatomic) NSInteger code39ChecksumType;
        [Export("code39ChecksumType")]
        Code39ChecksumType Code39ChecksumType { get; }

        // -(void)setCode39ChecksumTypeWithArg:(NSInteger)arg;
        [Export("setCode39ChecksumTypeWithArg:")]
        void SetCode39ChecksumTypeWithArg(Code39ChecksumType arg);

        // @property (readonly, nonatomic) NSInteger code11ChecksumType;
        [Export("code11ChecksumType")]
        Code11ChecksumType Code11ChecksumType { get; }

        // -(void)setCode11ChecksumTypeWithArg:(NSInteger)arg;
        [Export("setCode11ChecksumTypeWithArg:")]
        void SetCode11ChecksumTypeWithArg(Code11ChecksumType arg);

        // -(void)setBarkoderResolutionWithArg:(NSInteger)arg;
        [Export("setBarkoderResolutionWithArg:")]
        void SetBarkoderResolutionWithArg(BarkoderResolution arg);

        // @property (readonly, nonatomic) NSInteger barkoderResolution;
        [Export("barkoderResolution")]
        BarkoderResolution BarkoderResolution { get; }


        [Export("setBarkoderARModeWithArg:")]
        void SetBarkoderARMode(nint arg);

        [Export("setBarkoderARHeaderShowModeWithArg:")]
        void SetBarkoderARHeaderShowMode(nint arg);

        [Export("setBarkoderARLocationTypeWithArg:")]
        void SetBarkoderARLocationType(nint arg);

        [Export("setBarkoderARoverlayRefreshWithArg:")]
        void SetBarkoderARoverlayRefresh(nint arg);

        [Export("setARNonSelectedLocationLineColorWithArg:")]
        void SetARNonSelectedLocationLineColor(string arg);

        [Export("setARNonSelectedHeaderTextColorWithArg:")]
        void SetARNonSelectedHeaderTextColor(string arg);

        [Export("setARSelectedHeaderTextColorWithArg:")]
        void SetARSelectedHeaderTextColor(string arg);

        [Export("setARSelectedLocationLineColorWithArg:")]
        void SetARSelectedLocationLineColor(string arg);

        [Export("setARSelectedLocationLineWidthWithArg:")]
        void SetARSelectedLocationLineWidth(float arg);

        [Export("setARNonSelectedLocationLineWidthWithArg:")]
        void SetARNonSelectedLocationLineWidth(float arg);

        [Export("setARHeaderMaxTextHeightWithArg:")]
        void SetARHeaderMaxTextHeight(float arg);

        [Export("setARHeaderMinTextHeightWithArg:")]
        void SetARHeaderMinTextHeight(float arg);

        [Export("setARHeaderHeightWithArg:")]
        void SetARHeaderHeight(float arg);

        [Export("setARHeaderVerticalTextMarginWithArg:")]
        void SetARHeaderVerticalTextMargin(float arg);

        [Export("setARHeaderHorizontalTextMarginWithArg:")]
        void SetARHeaderHorizontalTextMargin(float arg);

        [Export("setARDoubleTapToFreezeEnabledWithArg:")]
        void SetARDoubleTapToFreezeEnabled(bool arg);

        [Export("setResultDisappearanceDelayMsWithArg:")]
        void SetResultDisappearanceDelayMs(nint arg);

        [Export("setARLocationTransitionSpeedWithArg:")]
        void SetARLocationTransitionSpeed(float arg);

        [Export("setARHeaderTextFormatWithArg:")]
        void SetARHeaderTextFormat(string arg);



        // @property (readonly, nonatomic) NSInteger barkoderResolution;
        [Export("barkoderARMode")]
        BarkoderARMode BarkoderARMode { get; }

        [Export("barkoderARLocationType")]
        BarkoderARLocationType BarkoderARLocationType { get; }

        [Export("barkoderARHeaderShowMode")]
        BarkoderARHeaderShowMode BarkoderARHeaderShowMode { get; }


        [Export("barkoderAROverlayRefresh")]
        BarkoderAROverlayRefresh BarkoderAROverlayRefresh { get; }

        // -(void)setDecodingSpeedWithArg:(NSInteger)arg;
        [Export("setDecodingSpeedWithArg:")]
        void SetDecodingSpeedWithArg(DecodingSpeed arg);

        // -(void)setEncodingCharacterSetWithArg:(NSString * _Nonnull)arg;
        [Export("setEncodingCharacterSetWithArg:")]
        void SetEncodingCharacterSetWithArg(string arg);

        // -(void)setFormattingTypeWithArg:(NSInteger)arg;
        [Export("setFormattingTypeWithArg:")]
        void SetFormattingTypeWithArg(FormattingType arg);

        // -(void)setMaximumResultsCountWithArg:(NSInteger)arg;
        [Export("setMaximumResultsCountWithArg:")]
        void SetMaximumResultsCountWithArg(nint arg);

        // -(void)setDuplicatesDelayMsWithArg:(NSInteger)arg;
        [Export("setDuplicatesDelayMsWithArg:")]
        void SetDuplicatesDelayMsWithArg(nint arg);

        // -(void)setMulticodeCachingDurationWithArg:(NSInteger)arg;
        [Export("setMulticodeCachingDurationWithArg:")]
        void SetMulticodeCachingDurationWithArg(nint arg);

        // -(void)setMulticodeCachingEnabledWithArg:(BOOL)arg;
        [Export("setMulticodeCachingEnabledWithArg:")]
        void SetMulticodeCachingEnabledWithArg(bool arg);

        // -(void)setDatamatrixDpmModeEnabledWithArg:(BOOL)arg;
        [Export("setDatamatrixDpmModeEnabledWithArg:")]
        void SetDatamatrixDpmModeEnabledWithArg(bool arg);

        [Export("setIdDocumentMasterChecksumEnabledWithArg:")]
        void SetIdDocumentMasterChecksumEnabledWithArg(bool arg);

        [Export("setQRDpmModeEnabledWithArg:")]
        void SetQRDpmModeEnabledWithArg(bool arg);

        [Export("setQRMicroDpmModeEnabledWithArg:")]
        void SetQRMicroDpmModeEnabledWithArg(bool arg);

        [Export("setShowDuplicatesLocationWithArg:")]
        void SetShowDuplicatesLocation(bool arg);

        // -(void)setUpcEanDeblurEnabledWithArg:(BOOL)arg;
        [Export("setUpcEanDeblurEnabledWithArg:")]
        void SetUpcEanDeblurEnabledWithArg(bool arg);

        // -(void)setEnableVINRestrictionsWithArg:(BOOL)arg;
        [Export("setEnableVINRestrictionsWithArg:")]
        void SetEnableVINRestrictionsWithArg(bool arg);

        // -(void)setMisshaped1DEnabledWithArg:(BOOL)arg;
        [Export("setMisshaped1DEnabledWithArg:")]
        void SetMisshaped1DEnabledWithArg(bool arg);

        // -(void)setBarcodeThumbnailOnResultEnabledWithArg:(BOOL)arg;
        [Export("setBarcodeThumbnailOnResultEnabledWithArg:")]
        void SetBarcodeThumbnailOnResultEnabledWithArg(bool arg);

        // @property (readonly, nonatomic) BOOL barcodeThumbnailOnResultEnabled;
        [Export("barcodeThumbnailOnResultEnabled")]
        bool BarcodeThumbnailOnResultEnabled { get; }

        // @property (readonly, nonatomic) BOOL multicodeCachingEnabled;
        [Export("multicodeCachingEnabled")]
        bool MulticodeCachingEnabled { get; }

        // @property (readonly, nonatomic) BOOL datamatrixDpmModeEnabled;
        [Export("datamatrixDpmModeEnabled")]
        bool DatamatrixDpmModeEnabled { get; }

        [Export("idDocumentMasterChecksumEnabled")]
        bool IdDocumentMasterChecksumEnabled { get; }

        [Export("qrDpmModeEnabled")]
        bool QRDpmModeEnabled { get; }

        [Export("qrMicroDpmModeEnabled")]
        bool QRMicroDpmModeEnabled { get; }

        // @property (readonly, nonatomic) BOOL upcEanDeblurEnabled;
        [Export("upcEanDeblurEnabled")]
        bool UpcEanDeblurEnabled { get; }

        // @property (readonly, nonatomic) BOOL enableMisshaped1D;
        [Export("enableMisshaped1D")]
        bool EnableMisshaped1D { get; }

        // @property (readonly, nonatomic) NSInteger multicodeCachingDuration;
        [Export("multicodeCachingDuration")]
        nint MulticodeCachingDuration { get; }

        // @property (readonly, nonatomic) NSInteger maximumResultsCount;
        [Export("maximumResultsCount")]
        nint MaximumResultsCount { get; }

        // @property (readonly, nonatomic) NSInteger duplicatesDelayMs;
        [Export("duplicatesDelayMs")]
        nint DuplicatesDelayMs { get; }

        // @property (readonly, nonatomic) BOOL isImageResultEnabled;
        [Export("isImageResultEnabled")]
        bool IsImageResultEnabled { get; }

        // @property (readonly, nonatomic) BOOL isCloseSessionOnResultEnabled;
        [Export("isCloseSessionOnResultEnabled")]
        bool IsCloseSessionOnResultEnabled { get; }

        // @property (readonly, nonatomic) BOOL isLocationInImageResultEnabled;
        [Export("isLocationInImageResultEnabled")]
        bool IsLocationInImageResultEnabled { get; }

        // -(BOOL)isBarcodeTypeEnabledWithBarcodeType:(id)barcodeType __attribute__((warn_unused_result("")));
        [Export("isBarcodeTypeEnabledWithBarcodeType:")]
        bool IsBarcodeTypeEnabledWithBarcodeType(BarcodeType barcodeType);

        // -(void)setBarcodeTypeEnabledWithBarcodeType:(id)barcodeType enabled:(BOOL)enabled;
        [Export("setBarcodeTypeEnabledWithBarcodeType:enabled:")]
        void SetBarcodeTypeEnabledWithBarcodeType(BarcodeType barcodeType, bool enabled);

        // -(void)setBarcodeTypeLengthRangeWithBarcodeType:(id)barcodeType minValue:(NSInteger)minValue maxValue:(NSInteger)maxValue;
        [Export("setBarcodeTypeLengthRangeWithBarcodeType:minValue:maxValue:")]
        void SetBarcodeTypeLengthRangeWithBarcodeType(BarcodeType barcodeType, nint minValue, nint maxValue);

        // -(NSArray<NSNumber *> * _Nonnull)getBarcodeTypeLengthRangeWithBarcodeType:(id)barcodeType __attribute__((warn_unused_result("")));
        [Export("getBarcodeTypeLengthRangeWithBarcodeType:")]
        NSNumber[] GetBarcodeTypeLengthRangeWithBarcodeType(BarcodeType barcodeType);

        // -(void)configureBarkoderWithBarkoderConfigAsJsonString:(NSString * _Nonnull)barkoderConfigAsJsonString;
        [Export("configureBarkoderWithBarkoderConfigAsJsonString:")]
        void ConfigureBarkoder(string barkoderConfigAsJsonString);

        [Export("setCustomOptionWithOption:value:")]
        void SetCustomOption(string option, int value);

           // Scanning Indicator Animation Mode
        [Export("setScanningIndicatorAnimationModeWithArg:")]
        void SetScanningIndicatorAnimationMode(nint arg);

        // Scanning Indicator Color
        [Export("setScanningIndicatorColorHexWithArg:")]
        void SetScanningIndicatorColorHex(string arg);

        // Scanning Indicator Width
        [Export("setScanningIndicatorWidthWithArg:")]
        void SetScanningIndicatorWidth(float arg);

        // Scanning Indicator Always Visible
        [Export("setScanningIndicatorAlwaysVisibleWithArg:")]
        void SetScanningIndicatorAlwaysVisible(bool arg);

        [Export("setEnableCompositeWithArg:")]
        void SetEnableComposite(nint arg);

        [Export("setDynamicExposureWithArg:")]
        void SetDynamicExposure(nint arg);

        [Export("setCentricFocusAndExposureWithArg:")]
        void SetCentricFocusAndExposure(bool arg);

        [Export("setUPCEexpandToUPCAWithArg:")]
        void SetUPCEexpandToUPCA(bool arg);

        [Export("setUPCE1expandToUPCAWithArg:")]
        void SetUPCE1expandToUPCA(bool arg);

        [Export("setVideoStabilizationWithArg:")]
        void setVideoStabilizationWithArg(bool arg);

        [Export("setCameraWithArg:")]
        void setCamera(BarkoderCameraPosition arg);

    }

      // @interface DecoderPayload : NSObject
    [BaseType(typeof(NSObject))]
    interface DecoderPayload
    {
        // @property (copy, nonatomic) NSArray<DecoderResult *> * _Nonnull results;
        [Export("results", ArgumentSemantic.Copy)]
        BKDecoderResult[] Results { get; set; }

        // @property (copy, nonatomic) NSString * _Nonnull imageInBase64;
        [Export("imageInBase64")]
        string ImageInBase64 { get; set; }

        [Export("mainImageInBase64")]
        string MainImageInBase64 { get; set; }

    }


      [BaseType(typeof(NSObject))]
    public interface BKDecoderResult
    {
        // @property (readwrite, retain, nonatomic) NSString * barcodeTypeName;
        [Export("barcodeTypeName", ArgumentSemantic.Retain)]
        string BarcodeTypeName { get; set; }

        // @property (readwrite, retain, nonatomic) NSString * textualData;
        [Export("textualData", ArgumentSemantic.Retain)]
        string TextualData { get; set; }

        [Export ("extra", ArgumentSemantic.Retain)]
		NSDictionary Extra { get; set; }

    //         // New property for images (array of BKImageDescriptor)
    // // @property (readwrite, retain, nonatomic) BKImageDescriptor[] *images;
        [Export("images", ArgumentSemantic.Retain)]
        BKImageDescriptor[] Images { get; set; }
    }
    
    [BaseType(typeof(NSObject))]
    public interface BKImageDescriptor
{
    // @property (readwrite, retain, nonatomic) NSString *name;
    [Export("name", ArgumentSemantic.Retain)]
    string Name { get; set; }
    // @property (readwrite, retain, nonatomic) UIImage *image;
    [Export("image", ArgumentSemantic.Retain)]
    UIImage Image { get; set; }
    
}

    


    // @interface BarkoderFrameworkProxy_Swift_376 (BarkoderProxy)
    [Category]
    [BaseType(typeof(BarkoderView))]
    interface BarkoderProxy_BarkoderFrameworkProxy_Swift_376
    {
        // -(void)scanningFinished:(NSArray<DecoderResult *> * _Nonnull)decoderResults image:(UIImage * _Nullable)image;
        [Export("scanningFinished:thumbnails:image:")]
        void ScanningFinished(BKDecoderResult[] decoderResults, [NullAllowed] UIImage[] thumbnails, [NullAllowed] UIImage image);
    }

}