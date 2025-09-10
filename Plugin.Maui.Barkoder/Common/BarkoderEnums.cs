namespace Plugin.Maui.Barkoder.Enums;

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
    Dotcode,
    IDDocument,
    Databar14,
    DatabarLimited,
    DatabarExpanded,
    PostalIMB,
    Postnet,
    Planet,
    AustralianPost,
    RoyalMail,
    KIX,
    JapanesePost,
    Maxicode
}

public enum BarkoderResolution
{
    HD,
    FHD,
    UHD
}


public enum BarkoderARMode
{
    OFF,
    InteractiveDisabled,
    InteractiveEnabled,
    NonInteractive
}

public enum BarkoderARHeaderShowMode
{
    NEVER,
    ALWAYS,
    ONSELECTED
}

public enum BarkoderARLocationType
{
    NONE,
    TIGHT,
    BOUNDINGBOX
}

public enum BarkoderAROverlayRefresh
{
    SMOOTH,
    NORMAL
}

public enum Code11ChecksumType
{
    Disabled,
    Single,
    Double
}

public enum BarkoderCameraPosition
{
    BACK,
    FRONT
}

public enum Code39ChecksumType
{
    Disabled,
    Enabled
}

public enum DecoderType
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
    Dotcode,
    IDDocument,
    Databar14,
    DatabarLimited,
    DatabarExpanded,
    PostalIMB,
    Postnet,
    Planet,
    AustralianPost,
    RoyalMail,
    KIX,
    JapanesePost,
    Maxicode
}

public enum DecodingSpeed
{
    Fast,
    Normal,
    Slow,
    Rigorous
}

public enum FormattingType
{
    Disabled,
    Automatic,
    GS1,
    AAMVA,
    SADL
}

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


