using ObjCRuntime;
using Foundation;
using UIKit;
using System;

namespace Barkoder.Xamarin
{
    // @interface BarkoderConfig : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface BarkoderConfig
    {
        // -(instancetype _Nonnull)initWithLicenseKey:(NSString * _Nonnull)licenseKey __attribute__((objc_designated_initializer));
        [Export("initWithLicenseKey:")]
        [DesignatedInitializer]
        IntPtr Constructor(string licenseKey);

        // @property (nonatomic) BOOL closeSessionOnResultEnabled;
        [Export("closeSessionOnResultEnabled")]
        bool CloseSessionOnResultEnabled { get; set; }

        // @property (nonatomic) BOOL vibrateOnSuccessEnabled;
        [Export("vibrateOnSuccessEnabled")]
        bool VibrateOnSuccessEnabled { get; set; }

        // @property (nonatomic) BOOL beepOnSuccessEnabled;
        [Export("beepOnSuccessEnabled")]
        bool BeepOnSuccessEnabled { get; set; }

        // @property (nonatomic) BOOL pinchToZoomEnabled;
        [Export("pinchToZoomEnabled")]
        bool PinchToZoomEnabled { get; set; }

        // @property (nonatomic) BOOL locationInPreviewEnabled;
        [Export("locationInPreviewEnabled")]
        bool LocationInPreviewEnabled { get; set; }

        // @property (nonatomic, strong) UIColor * _Nonnull roiLineColor;
        [Export("roiLineColor", ArgumentSemantic.Strong)]
        UIColor RoiLineColor { get; set; }

        // @property (nonatomic) float roiLineWidth;
        [Export("roiLineWidth")]
        float RoiLineWidth { get; set; }

        // @property (nonatomic, strong) UIColor * _Nonnull roiOverlayBackgroundColor;
        [Export("roiOverlayBackgroundColor", ArgumentSemantic.Strong)]
        UIColor RoiOverlayBackgroundColor { get; set; }

        // @property (nonatomic) BOOL regionOfInterestVisible;
        [Export("regionOfInterestVisible")]
        bool RegionOfInterestVisible { get; set; }

        // @property (nonatomic, strong) UIColor * _Nonnull locationLineColor;
        [Export("locationLineColor", ArgumentSemantic.Strong)]
        UIColor LocationLineColor { get; set; }

        // @property (nonatomic) float locationLineWidth;
        [Export("locationLineWidth")]
        float LocationLineWidth { get; set; }

        // @property (nonatomic) BOOL imageResultEnabled;
        [Export("imageResultEnabled")]
        bool ImageResultEnabled { get; set; }

        // @property (nonatomic) BOOL locationInImageResultEnabled;
        [Export("locationInImageResultEnabled")]
        bool LocationInImageResultEnabled { get; set; }

        // @property (nonatomic) NSInteger thresholdBetweenDuplicatesScans;
        [Export("thresholdBetweenDuplicatesScans")]
        nint ThresholdBetweenDuplicatesScans { get; set; }

        // @property (nonatomic, strong) DecoderConfig * _Nullable decoderConfig;
        [NullAllowed, Export("decoderConfig", ArgumentSemantic.Strong)]
        DecoderConfig DecoderConfig { get; set; }
    }

  

    // @interface DecoderConfig : NSObject
    [BaseType(typeof(NSObject))]
    public interface DecoderConfig
    {
        // @property (nonatomic) BOOL enableMisshaped1D;
        [Export("enableMisshaped1D")]
        bool EnableMisshaped1D { get; set; }

        // @property (nonatomic) BOOL upcEanDeblur;
        [Export("upcEanDeblur")]
        bool UpcEanDeblur { get; set; }

        // @property (nonatomic) BOOL enableVINRestrictions;
        [Export("enableVINRestrictions")]
        bool EnableVINRestrictions { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull aztec;
        [Export("aztec", ArgumentSemantic.Strong)]
        SymbologyConfig Aztec { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull aztecCompact;
        [Export("aztecCompact", ArgumentSemantic.Strong)]
        SymbologyConfig AztecCompact { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull qr;
        [Export("qr", ArgumentSemantic.Strong)]
        SymbologyConfig Qr { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull qrMicro;
        [Export("qrMicro", ArgumentSemantic.Strong)]
        SymbologyConfig QrMicro { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull code128;
        [Export("code128", ArgumentSemantic.Strong)]
        SymbologyConfig Code128 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull code93;
        [Export("code93", ArgumentSemantic.Strong)]
        SymbologyConfig Code93 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull code39;
        [Export("code39", ArgumentSemantic.Strong)]
        SymbologyConfig Code39 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull codabar;
        [Export("codabar", ArgumentSemantic.Strong)]
        SymbologyConfig Codabar { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull code11;
        [Export("code11", ArgumentSemantic.Strong)]
        SymbologyConfig Code11 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull msi;
        [Export("msi", ArgumentSemantic.Strong)]
        SymbologyConfig Msi { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull upcA;
        [Export("upcA", ArgumentSemantic.Strong)]
        SymbologyConfig UpcA { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull upcE;
        [Export("upcE", ArgumentSemantic.Strong)]
        SymbologyConfig UpcE { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull upcE1;
        [Export("upcE1", ArgumentSemantic.Strong)]
        SymbologyConfig UpcE1 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull ean8;
        [Export("ean8", ArgumentSemantic.Strong)]
        SymbologyConfig Ean8 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull ean13;
        [Export("ean13", ArgumentSemantic.Strong)]
        SymbologyConfig Ean13 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull ean13;
        [Export("postalIMB", ArgumentSemantic.Strong)]
        SymbologyConfig PostalIMB { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull ean13;
        [Export("postnet", ArgumentSemantic.Strong)]
        SymbologyConfig Postnet { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull ean13;
        [Export("planet", ArgumentSemantic.Strong)]
        SymbologyConfig Planet { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull ean13;
        [Export("australianPost", ArgumentSemantic.Strong)]
        SymbologyConfig AustralianPost { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull ean13;
        [Export("royalMail", ArgumentSemantic.Strong)]
        SymbologyConfig RoyalMail { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull ean13;
        [Export("kix", ArgumentSemantic.Strong)]
        SymbologyConfig KIX { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull ean13;
        [Export("japanesePost", ArgumentSemantic.Strong)]
        SymbologyConfig JapanesePost { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull pdf417;
        [Export("pdf417", ArgumentSemantic.Strong)]
        SymbologyConfig Pdf417 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull pdf417Micro;
        [Export("pdf417Micro", ArgumentSemantic.Strong)]
        SymbologyConfig Pdf417Micro { get; set; }

        // @property (nonatomic, strong) DatamatrixConfig * _Nonnull datamatrix;
        [Export("datamatrix", ArgumentSemantic.Strong)]
        DatamatrixConfig Datamatrix { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull code25;
        [Export("code25", ArgumentSemantic.Strong)]
        SymbologyConfig Code25 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull interleaved25;
        [Export("interleaved25", ArgumentSemantic.Strong)]
        SymbologyConfig Interleaved25 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull itf14;
        [Export("itf14", ArgumentSemantic.Strong)]
        SymbologyConfig Itf14 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull iata25;
        [Export("iata25", ArgumentSemantic.Strong)]
        SymbologyConfig Iata25 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull matrix25;
        [Export("matrix25", ArgumentSemantic.Strong)]
        SymbologyConfig Matrix25 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull datalogic25;
        [Export("datalogic25", ArgumentSemantic.Strong)]
        SymbologyConfig Datalogic25 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull coop25;
        [Export("coop25", ArgumentSemantic.Strong)]
        SymbologyConfig Coop25 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull code32;
        [Export("code32", ArgumentSemantic.Strong)]
        SymbologyConfig Code32 { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull telepen;
        [Export("telepen", ArgumentSemantic.Strong)]
        SymbologyConfig Telepen { get; set; }

        // @property (nonatomic, strong) SymbologyConfig * _Nonnull dotcode;
        [Export("dotcode", ArgumentSemantic.Strong)]
        SymbologyConfig Dotcode { get; set; }

        [Export("idDocument", ArgumentSemantic.Strong)]
        SymbologyConfig IDDocument { get; set; }

        [Export("databar14", ArgumentSemantic.Strong)]
        SymbologyConfig Databar14 { get; set; }

        [Export("databarLimited", ArgumentSemantic.Strong)]
        SymbologyConfig DatabarLimited { get; set; }

        [Export("databarExpanded", ArgumentSemantic.Strong)]
        SymbologyConfig DatabarExpanded { get; set; }

     


        // -(void)setEnabledDecodersWithSymbologies:(NSArray<NSNumber *> * _Nonnull)symbologies;
        [Export("setEnabledDecodersWithSymbologies:")]
        void SetEnabledDecoders(NSNumber[] symbologies);
    }

    // @interface SymbologyConfig : NSObject
    [BaseType(typeof(NSObject))]
    public interface SymbologyConfig
    {
        // @property (nonatomic) BOOL enabled;
        [Export("enabled")]
        bool Enabled { get; set; }
    }
    

    // @interface DatamatrixConfig : NSObject
    [BaseType(typeof(NSObject))]
    public interface DatamatrixConfig
    {
        // @property (nonatomic) BOOL enabled;
        [Export("enabled")]
        bool Enabled { get; set; }

        // @property (nonatomic) NSInteger dpmMode;
        [Export("dpmMode")]
        nint DpmMode { get; set; }
    }

    [BaseType(typeof(nint))]
    public enum BarkoderResolution
    {
        HD,
        FHD,
        UHD
    }

    [BaseType(typeof(nint))]
    public enum BarkoderARMode
    {
        OFF,
        INTERACTIVEDISABLED,
        INTERACTIVEENABLED,
        NONINTERACTIVE
    }

    [BaseType(typeof(nint))]
    public enum BarkoderARLocationType
    {
        NONE,
        TIGHT,
        BOUNDINGBOX
    }

    [BaseType(typeof(nint))]
    public enum BarkoderARHeaderShowMode
    {
        NEVER,
        ALWAYS,
        ONSELECTED
    }

    [BaseType(typeof(nint))]
    public enum BarkoderAROverlayRefresh
    {
        SMOOTH,
        NORMAL
    }

    [BaseType(typeof(nint))]
    public enum BarkoderCameraPosition
    {
        BACK,
        FRONT
    }


    [BaseType(typeof(nint))]
    public enum DecodingSpeed
    {
        Fast,
        Normal,
        Slow
    }

    [BaseType(typeof(nint))]
    public enum FormattingType
    {
        Disabled,
        Automatic,
        GS1,
        AAMVA
    }

    [BaseType(typeof(nint))]
    public enum BarcodeType
    {
        aztec,
        aztecCompact,
        qr,
        qrMicro,
        code128,
        code93,
        code39,
        codabar,
        code11,
        msi,
        upcA,
        upcE,
        upcE1,
        ean13,
        ean8,
        pdf417,
        pdf417Micro,
        datamatrix,
        code25,
        interleaved25,
        itf14,
        iata25,
        matrix25,
        datalogic25,
        coop25,
        code32,
        telepen,
        dotcode,
        idDocument,
        databar14,
        databarLimited,
        databarExpanded,
        postalIMB,
        postnet,
        planet,
        australianPost,
        royalMail,
        kix,
        japanesePost
    }

    [BaseType(typeof(int))]
    public enum DecoderType
    {
        Aztec = 0,
        AztecCompact = 1,
        Qr = 2,
        QrMicro = 3,
        Code128 = 4,
        Code93 = 5,
        Code39 = 6,
        Codabar = 7,
        Code11 = 8,
        Msi = 9,
        UpcA = 10,
        UpcE = 11,
        UpcE1 = 12,
        Ean13 = 13,
        Ean8 = 14,
        Pdf417 = 15,
        Pdf417Micro = 16,
        Datamatrix = 17,
        Code25 = 18,
        Interleaved25 = 19,
        Itf14 = 20,
        Iata25 = 21,
        Matrix25 = 22,
        Datalogic25 = 23,
        Coop25 = 24,
        Code32 = 25,
        Telepen = 26,
        Dotcode = 27,
        IDDocument = 28,
        Databar14 = 29,
        DatabarLimited = 30,
        DatabarExpanded = 31,
        PostalIMB = 32,
        Postnet = 33,
        Planet = 34,
        AustralianPost = 35,
        RoyalMail = 36,
        KIX = 37,
        JapanesePost
    }

    [BaseType(typeof(nint))]
    enum MsiChecksumType
    {
        disabled,
        mod10,
        mod11,
        mod1010,
        mod1110,
        mod11IBM,
        mod1110IBM
    }

    [BaseType(typeof(nint))]
    enum Code39ChecksumType
    {
        disabled,
        enabled
    }

    [BaseType(typeof(nint))]
    enum Code11ChecksumType
    {
        disabled,
        single,
        //@double
    }




}