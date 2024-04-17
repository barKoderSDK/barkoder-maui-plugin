
using Microsoft.Maui.Handlers;

namespace Plugin.Maui.Barkoder.Controls
{
    public partial class BarkoderViewHandler
    {
        public BarkoderViewHandler() : base(PropertyMapper, CommandMapper) { }

        public static IPropertyMapper<BarkoderView, BarkoderViewHandler> PropertyMapper =
            new PropertyMapper<BarkoderView, BarkoderViewHandler>(ViewHandler.ViewMapper)
            {
                [nameof(BarkoderView.LicenseKey)] = MapLicenseKey,
                [nameof(BarkoderView.RegionOfInterestVisible)] = MapRegionOfInterestVisible,
                [nameof(BarkoderView.IsFlashAvailableProperty)] = MapIsFlashAvailable,
                [nameof(BarkoderView.MaxZoomFactorProperty)] = MapMaxZoomFactor,
                [nameof(BarkoderView.LocationLineColorHexProperty)] = MapLocationLineColorHex,
                [nameof(BarkoderView.RoiLineColorHexProperty)] = MapRoiLineColorHex,
                [nameof(BarkoderView.RoiOverlayBackgroundColorHexProperty)] = MapRoiOverlayBackgroundColorHex,
                [nameof(BarkoderView.EncodingCharacterSetProperty)] = MapEncodingCharacterSet,
                [nameof(BarkoderView.LocationLineWidthProperty)] = MapLocationLineWidth,
                [nameof(BarkoderView.ImageResultEnabledProperty)] = MapImageResultEnabled,
                [nameof(BarkoderView.LocationInImageResultEnabledProperty)] = MapLocationInImageResultEnabled,
                [nameof(BarkoderView.LocationInPreviewEnabledProperty)] = MapLocationInPreviewEnabled,
                [nameof(BarkoderView.PinchToZoomEnabledProperty)] = MapPinchToZoomEnabled,
                [nameof(BarkoderView.BeepOnSuccessEnabledProperty)] = MapBeepOnSuccessEnabled,
                [nameof(BarkoderView.VibrateOnSuccessEnabledProperty)] = MapVibrateOnSuccessEnabled,
                [nameof(BarkoderView.CloseSessionOnResultEnabledProperty)] = MapCloseSessionOnResultEnabled,
                [nameof(BarkoderView.VersionProperty)] = MapVersion,
                [nameof(BarkoderView.BarkoderResolutionProperty)] = MapBarkoderResolution,
                [nameof(BarkoderView.DecodingSpeedProperty)] = MapDecodingSpeed,
                [nameof(BarkoderView.FormattingTypeProperty)] = MapFormattingType,
                [nameof(BarkoderView.MsiChecksumTypeProperty)] = MapMsiChecksumType,
                [nameof(BarkoderView.Code11ChecksumTypeProperty)] = MapCode11ChecksumType,
                [nameof(BarkoderView.Code39ChecksumTypeProperty)] = MapCode39ChecksumType,
                [nameof(BarkoderView.DatamatrixDpmModeEnabledProperty)] = MapDatamatrixDpmModeEnabled,
                [nameof(BarkoderView.UpcEanDeblurEnabledProperty)] = MapUpcEanDeblurEnabled,
                [nameof(BarkoderView.EnableMisshaped1DEnabledProperty)] = MapEnableMisshaped1DEnabled,
                [nameof(BarkoderView.BarcodeThumbnailOnResultEnabledProperty)] = MapBarcodeThumbnailOnResultEnabledProperty,
                [nameof(BarkoderView.MaximumResultsCountProperty)] = MapMaximumResultsCount,
                [nameof(BarkoderView.DuplicatesDelayMsProperty)] = MapDuplicatesDelayMs,
                [nameof(BarkoderView.VINRestrictionsEnabledProperty)] = MapVINRestrictionsEnabled,
                [nameof(BarkoderView.ThresholdBetweenDuplicatesScansProperty)] = MapThresholdBetweenDuplicatesScans,
                [nameof(BarkoderView.RegionOfInterestProperty)] = MapRegionOfInterest,
            };

        public static CommandMapper<BarkoderView, BarkoderViewHandler> CommandMapper = new(ViewCommandMapper)
        {
            [nameof(BarkoderView.StartCameraRequested)] = MapStartCamera,
            [nameof(BarkoderView.StartScanningRequested)] = MapStartScanning,
            [nameof(BarkoderView.StopScanningRequested)] = MapStopScanning,
            [nameof(BarkoderView.PauseScanningRequested)] = MapPauseScanning,
            [nameof(BarkoderView.FlashEnableRequested)] = MapFlashEnable,
            [nameof(BarkoderView.SetZoomFactorRequested)] = MapSetZoomFactor,
            [nameof(BarkoderView.SetPinchToZoomEnabledRequested)] = MapSetPinchToZoomEnabled,
            [nameof(BarkoderView.SetRegionOfInterestVisibleRequested)] = MapSetRegionOfInterestVisible,
            [nameof(BarkoderView.SetRoiLineWidthRequested)] = MapSetRoiLineWidth,
            [nameof(BarkoderView.SetCloseSessionOnResultEnabledRequested)] = MapSetCloseSessionOnResult,
            [nameof(BarkoderView.SetImageResultEnabledRequested)] = MapSetImageResultEnabled,
            [nameof(BarkoderView.SetLocationInPreviewEnabledRequested)] = MapSetLocationInPreviewEnabled,
            [nameof(BarkoderView.SetLocationInImageResultEnabledRequested)] = MapSetLocationInImageResultEnabled,
            [nameof(BarkoderView.SetBeepOnSuccessEnabledRequested)] = MapSetBeepOnSuccessEnabled,
            [nameof(BarkoderView.SetVibrateOnSuccessEnabledRequested)] = MapSetVibrateOnSuccessEnabled,
            [nameof(BarkoderView.SetLocationLineColorRequested)] = MapSetLocationLineColor,
            [nameof(BarkoderView.SetRoiLineColorRequested)] = MapSetRoiLineColor,
            [nameof(BarkoderView.SetRoiOverlayBackgroundColorRequested)] = MapSetRoiOverlayBackgroundColor,
            [nameof(BarkoderView.SetLocationLineWidthRequested)] = MapSetLocationLineWidth,
            [nameof(BarkoderView.SetRegionOfInterestRequested)] = MapSetRegionOfInterest,
            [nameof(BarkoderView.SetBarkoderResolutionRequested)] = MapSetBarkoderResolution,
            [nameof(BarkoderView.SetDecodingSpeedRequested)] = MapSetDecodingSpeed,
            [nameof(BarkoderView.SetFormattingTypeRequested)] = MapSetFormattingType,
            [nameof(BarkoderView.SetMsiChecksumTypeRequested)] = MapSetMsiChecksumType,
            [nameof(BarkoderView.SetCode11ChecksumTypeRequested)] = MapSetCode11ChecksumType,
            [nameof(BarkoderView.SetCode39ChecksumTypeRequested)] = MapSetCode39ChecksumType,
            [nameof(BarkoderView.SetEncodingCharacterSetRequested)] = MapSetEncodingCharacterSet,
            [nameof(BarkoderView.SetDatamatrixDpmModeEnabledRequested)] = MapSetDatamatrixDpmModeEnabled,
            [nameof(BarkoderView.SetUpcEanDeblurEnabledRequested)] = MapSetUpcEanDeblurEnabled,
            [nameof(BarkoderView.SetEnableMisshaped1DEnabledRequested)] = MapSetEnableMisshaped1DEnabled,
            [nameof(BarkoderView.SetBarcodeThumbnailOnResultEnabledRequested)] = MapSetBarcodeThumbnailOnResultEnabled,
            [nameof(BarkoderView.SetMaximumResultsCountRequested)] = MapSetMaximumResultsCount,
            [nameof(BarkoderView.SetDuplicatesDelayMsRequested)] = MapSetDuplicatesDelayMs,
            [nameof(BarkoderView.SetBarcodeTypeEnabledRequested)] = MapSetBarcodeTypeEnabled,
            [nameof(BarkoderView.SetEnableVINRestrictionsRequested)] = MapSetEnableVINRestrictions,
            [nameof(BarkoderView.SetThresholdBetweenDuplicatesScansRequested)] = MapSetThresholdBetweenDuplicatesScans,
            [nameof(BarkoderView.SetBarcodeTypeLengthRangeRequested)] = MapSetBarcodeTypeLengthRange,
            [nameof(BarkoderView.ConfigureBarkoderRequested)] = MapConfigureBarkoder,
        };
    }
}
