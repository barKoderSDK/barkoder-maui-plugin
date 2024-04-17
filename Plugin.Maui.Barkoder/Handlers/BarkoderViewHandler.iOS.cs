#if IOS
using Barkoder.Xamarin;
using Microsoft.Maui.Handlers;
using UIKit;
using BarkoderSDK = Barkoder.Xamarin;
using Plugin.Maui.Barkoder.Interfaces;

using BarcodeResult = Plugin.Maui.Barkoder.Handlers.BarcodeResult;

using CommonBarkoderResolution = Plugin.Maui.Barkoder.Enums.BarkoderResolution;
using iOSBarkoderResolution = Barkoder.Xamarin.BarkoderResolution;

using CommonDecodingSpeed = Plugin.Maui.Barkoder.Enums.DecodingSpeed;
using iOSDecodingSpeed = Barkoder.Xamarin.DecodingSpeed;

using CommonFormattingType = Plugin.Maui.Barkoder.Enums.FormattingType;
using iOSFormattingType = Barkoder.Xamarin.FormattingType;

using CommonMsiChecksumType = Plugin.Maui.Barkoder.Enums.MsiChecksumType;
using iOSMsiChecksumType = Barkoder.Xamarin.MsiChecksumType;

using CommonCode11ChecksumType = Plugin.Maui.Barkoder.Enums.Code11ChecksumType;
using iOSCode11ChecksumType = Barkoder.Xamarin.Code11ChecksumType;

using CommonCode39ChecksumType = Plugin.Maui.Barkoder.Enums.Code39ChecksumType;
using iOSCode39ChecksumType = Barkoder.Xamarin.Code39ChecksumType;

using CommonBarcodeType = Plugin.Maui.Barkoder.Enums.BarcodeType;
using BarcodeType = Barkoder.Xamarin.BarcodeType;

namespace Plugin.Maui.Barkoder.Controls;

public partial class BarkoderViewHandler : ViewHandler<BarkoderView, UIView>
{
    private BarkoderSDK.BarkoderView? BKDView;

    protected override UIView CreatePlatformView()
    {
        this.BKDView = new BarkoderSDK.BarkoderView();

        return this.BKDView.View;
    }

    private static void MapLicenseKey(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            handler.BKDView.Config = new BarkoderSDK.BarkoderConfig(view.LicenseKey);
            handler.BKDView.Config.DecoderConfig = new DecoderConfig();
        }
    }

    private static void MapIsFlashAvailable(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            handler.BKDView.IsFlashAvailableWithCompletion((isAvailable) =>
            {
                view.IsFlashAvailable = isAvailable;
            });
        }
    }

    private static void MapMaxZoomFactor(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            handler.BKDView.GetMaxZoomFactorWithCompletion((MaxZoomFactor) =>
            {
                view.MaxZoomFactor = MaxZoomFactor;
            });
        }
    }

    private static void MapRoiLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.RoiLineColorHex = handler.BKDView.RoiLineColorHex;
        }
    }

    private static void MapLocationLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.LocationLineColorHex = handler.BKDView.RoiLineColorHex;
        }
    }

    private static void MapRoiOverlayBackgroundColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.RoiOverlayBackgroundColorHex = handler.BKDView.RoiOverlayBackgroundColorHex;
        }
    }

    private static void MapEncodingCharacterSet(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.EncodingCharacterSet = handler.BKDView.EncodingCharacterSet;
        }
    }

    private static void MapLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.LocationLineWidth = handler.BKDView.LocationLineWidth;
        }
    }

    private static void MapImageResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ImageResultEnabled = handler.BKDView.ImageResultEnabled;
        }
    }

    private static void MapLocationInImageResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.LocationInImageResultEnabled = handler.BKDView.LocationInImageResultEnabled;
        }
    }

    private static void MapLocationInPreviewEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.LocationInPreviewEnabled = handler.BKDView.IsLocationInPreviewEnabled;
        }
    }

    private static void MapPinchToZoomEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.PinchToZoomEnabled = handler.BKDView.IsPinchToZoomEnabled;
        }
    }

    private static void MapBeepOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.BeepOnSuccessEnabled = handler.BKDView.IsBeepOnSuccessEnabled;
        }
    }

    private static void MapVibrateOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.VibrateOnSuccessEnabled = handler.BKDView.IsVibrateOnSuccessEnabled;
        }
    }

    private static void MapCloseSessionOnResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.CloseSessionOnResultEnabled = handler.BKDView.CloseSessionOnResultEnabled;
        }
    }

    private static void MapVersion(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.Version = handler.BKDView.Version;
        }
    }

    private static void MapBarkoderResolution(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            iOSBarkoderResolution barkoderResolution = handler.BKDView.BarkoderResolution;

            if (barkoderResolution == iOSBarkoderResolution.High)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.High;
            }
            else if (barkoderResolution == iOSBarkoderResolution.Normal)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.Normal;
            }
        }

    }

    private static void MapDecodingSpeed(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            //iOSDecodingSpeed decodingSpeed = handler.BKDView.DecodingSpeed;

            //if (decodingSpeed == iOSDecodingSpeed.Slow)
            //{
            //    view.DecodingSpeed = CommonDecodingSpeed.Slow;
            //}
            //else if (decodingSpeed == iOSDecodingSpeed.Normal)
            //{
            //    view.DecodingSpeed = CommonDecodingSpeed.Normal;
            //}
            //else if (decodingSpeed == iOSDecodingSpeed.Fast)
            //{
            //    view.DecodingSpeed = CommonDecodingSpeed.Fast;
            //}
        }
    }

    private static void MapFormattingType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            iOSFormattingType formattingType = handler.BKDView.FormattingType;

            if (formattingType == iOSFormattingType.AAMVA)
            {
                view.FormattingType = CommonFormattingType.AAMVA;
            }
            else if (formattingType == iOSFormattingType.Automatic)
            {
                view.FormattingType = CommonFormattingType.Automatic;
            }
            else if (formattingType == iOSFormattingType.Disabled)
            {
                view.FormattingType = CommonFormattingType.Disabled;
            }
            else if (formattingType == iOSFormattingType.GS1)
            {
                view.FormattingType = CommonFormattingType.GS1;
            }
        }
    }


    private static void MapCode39ChecksumType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            iOSCode39ChecksumType code39ChecksumType = handler.BKDView.Code39ChecksumType;

            if (code39ChecksumType == iOSCode39ChecksumType.disabled)
            {
                view.Code39ChecksumType = CommonCode39ChecksumType.Disabled;
            }
            else if (code39ChecksumType == iOSCode39ChecksumType.enabled)
            {
                view.Code39ChecksumType = CommonCode39ChecksumType.Enabled;
            }
        }
    }

    private static void MapCode11ChecksumType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            iOSCode11ChecksumType code11ChecksumType = handler.BKDView.Code11ChecksumType;

            if (code11ChecksumType == iOSCode11ChecksumType.disabled)
            {
                view.Code11ChecksumType = CommonCode11ChecksumType.Disabled;
            }
            else if (code11ChecksumType == iOSCode11ChecksumType.disabled)
            {
                view.Code11ChecksumType = CommonCode11ChecksumType.Single;
            }
        }
    }

    private static void MapMsiChecksumType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            iOSMsiChecksumType msiChecksumType = handler.BKDView.MsiChecksumType;

            if (msiChecksumType == iOSMsiChecksumType.disabled)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Disabled;
            }
            else if (msiChecksumType == iOSMsiChecksumType.mod10)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod10;
            }
            else if (msiChecksumType == iOSMsiChecksumType.mod11)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod11;
            }
            else if (msiChecksumType == iOSMsiChecksumType.mod1010)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod1010;
            }
            else if (msiChecksumType == iOSMsiChecksumType.mod1110)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod1110;
            }
            else if (msiChecksumType == iOSMsiChecksumType.mod1110IBM)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod1110IBM;
            }
            else if (msiChecksumType == iOSMsiChecksumType.mod11IBM)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod11IBM;
            }
        }
    }

    private static void MapDatamatrixDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.DatamatrixDpmModeEnabled = handler.BKDView.DatamatrixDpmModeEnabled;
        }
    }

    private static void MapUpcEanDeblurEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.UpcEanDeblurEnabled = handler.BKDView.UpcEanDeblurEnabled;
        }
    }

    private static void MapEnableMisshaped1DEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.EnableMisshaped1DEnabled = handler.BKDView.EnableMisshaped1D;
        }
    }

    private static void MapBarcodeThumbnailOnResultEnabledProperty(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.BarcodeThumbnailOnResultEnabled = handler.BKDView.BarcodeThumbnailOnResultEnabled;
        }
    }

    private static void MapMaximumResultsCount(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.MaximumResultsCount = (int)handler.BKDView.MaximumResultsCount;
        }
    }

    private static void MapDuplicatesDelayMs(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.DuplicatesDelayMs = (int)handler.BKDView.DuplicatesDelayMs;
        }
    }

    private static void MapVINRestrictionsEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null) && (handler.BKDView.Config.DecoderConfig != null))
        {
            view.VINRestrictionsEnabled = handler.BKDView.Config.DecoderConfig.EnableVINRestrictions;
        }
    }

    private static void MapThresholdBetweenDuplicatesScans(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ThresholdBetweenDuplicatesScans = (int)handler.BKDView.Config.ThresholdBetweenDuplicatesScans;
        }
    }

    private static void MapStartCamera(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.BKDView?.StartCamera();
    }

    private static void MapRegionOfInterest(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            Foundation.NSNumber[] Rect = handler.BKDView.RegionOfInterest;

            if (Rect.Count() == 4)
            {
                view.RegionOfInterest = (
                    (int)Rect[0],
                    (int)Rect[1],
                    (int)Rect[2],
                    (int)Rect[3]
                );
            }
        }
    }


    private static void MapStartScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is IBarkoderDelegate barkoderDelegate)
        {
            handler.BKDView?.StartScanning((completion) =>
            {
                // SDK Results
                DecoderResult[] results = completion.Results;

                // Maui wrapper results
                BarcodeResult[] barcodeResults = new BarcodeResult[results.Length];

                // Converting from SDK results -> Maui wrapper results
                for (int i = 0; i < results.Length; i++)
                {
                    BarcodeResult barcodeResult = new BarcodeResult(results[i].TextualData, results[i].BarcodeTypeName, "");
                    barcodeResults[i] = barcodeResult;
                }

                barkoderDelegate?.DidFinishScanning(barcodeResults);
            });
        }
    }

    private static void MapStopScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.BKDView?.StopScanning();
    }

    private static void MapPauseScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.BKDView?.PauseScanning();
    }

    private static void MapRegionOfInterestVisible(BarkoderViewHandler handler, BarkoderView view)
    {
        handler.BKDView?.SetRegionOfInterestVisibleWithArg(view.RegionOfInterestVisible);
    }

    private static void MapFlashEnable(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetFlashEnabledWithArg(enabled);
        }
    }

    private static void MapSetZoomFactor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is float zoomFactor)
        {
            handler.BKDView?.SetZoomFactorWithArg(zoomFactor);
        }
    }

    private static void MapSetVibrateOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetVibrateOnSuccessEnabledWithArg(enabled);
        }
    }

    private static void MapSetBeepOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetBeepOnSuccessEnabledWithArg(enabled);
        }
    }

    private static void MapSetLocationInImageResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetLocationInImageResultEnabledWithArg(enabled);
        }
    }

    private static void MapSetLocationInPreviewEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetLocationInPreviewEnabledWithArg(enabled);
        }
    }

    private static void MapSetCloseSessionOnResult(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetCloseSessionOnResultEnabledWithArg(enabled);
        }
    }

    private static void MapSetImageResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetImageResultEnabledWithArg(enabled);
        }
    }

    private static void MapSetRoiLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is float width) && (handler.BKDView != null))
        {
            handler.BKDView.SetRoiLineWidthWithArg(width);
        }
    }

    private static void MapSetRegionOfInterestVisible(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool visible)
        {
            handler.BKDView?.SetRegionOfInterestVisibleWithArg(visible);
        }
    }

    private static void MapSetPinchToZoomEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetPinchToZoomEnabledWithArg(enabled);
        }
    }

    private static void MapSetRoiLineColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor)
        {
            handler.BKDView?.SetRoiLineColorWithArg(hexColor);
        }
    }

    private static void MapSetLocationLineColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor)
        {
            handler.BKDView?.SetLocationLineColorWithArg(hexColor);
        }
    }

    private static void MapSetRoiOverlayBackgroundColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor)
        {
            handler.BKDView?.SetRoiOverlayBackgroundColorWithArg(hexColor);
        }
    }

    private static void MapSetLocationLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is float width) && (handler.BKDView != null))
        {
            handler.BKDView.SetLocationLineWidthWithArg(width);
        }
    }

    private static void MapSetRegionOfInterest(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int[] roi) && (handler.BKDView != null))
        {
            try
            {
                handler.BKDView.SetRegionOfInterestWithLeft(roi[0], roi[1], roi[2], roi[3]);
            }
            catch
            {
                // TODO: - Handle errors
            }
        }
    }

    private static void MapSetBarkoderResolution(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonBarkoderResolution barkoderResolution) && (handler.BKDView != null))
        {
            switch (barkoderResolution)
            {
                case CommonBarkoderResolution.High:
                    {
                        handler.BKDView.SetBarkoderResolutionWithArg(iOSBarkoderResolution.High);
                        break;
                    }
                case CommonBarkoderResolution.Normal:
                    {
                        handler.BKDView.SetBarkoderResolutionWithArg(iOSBarkoderResolution.Normal);
                        break;
                    }
            }
        }
    }

    private static void MapSetDecodingSpeed(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonDecodingSpeed decodingSpeed) && (handler.BKDView != null))
        {
            switch (decodingSpeed)
            {
                case CommonDecodingSpeed.Slow:
                    {
                        //handler.BKDView.SetDecodingSpeedWithArg(iOSDecodingSpeed.Slow);
                        break;
                    }
                case CommonDecodingSpeed.Normal:
                    {
                        //handler.BKDView.SetDecodingSpeedWithArg(iOSDecodingSpeed.Normal);
                        break;
                    }
                case CommonDecodingSpeed.Fast:
                    {
                        //handler.BKDView.SetDecodingSpeedWithArg(iOSDecodingSpeed.Fast);
                        break;
                    }
            }
        }
    }


    private static void MapSetFormattingType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonFormattingType formattingType) && (handler.BKDView != null))
        {
            switch (formattingType)
            {
                case CommonFormattingType.Automatic:
                    {
                        //handler.BKDView.SetFormattingTypeWithArg(iOSFormattingType.Automatic);
                        break;
                    }
                case CommonFormattingType.Disabled:
                    {
                        //handler.BKDView.SetFormattingTypeWithArg(iOSFormattingType.Disabled);
                        break;
                    }
                case CommonFormattingType.AAMVA:
                    {
                        //handler.BKDView.SetFormattingTypeWithArg(iOSFormattingType.AAMVA);
                        break;
                    }
                case CommonFormattingType.GS1:
                    {
                        //handler.BKDView.SetFormattingTypeWithArg(iOSFormattingType.GS1);
                        break;
                    }
            }
        }
    }

    private static void MapSetCode39ChecksumType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonCode39ChecksumType code39ChecksumType) && (handler.BKDView != null))
        {
            switch (code39ChecksumType)
            {
                case CommonCode39ChecksumType.Disabled:
                    {
                        handler.BKDView.SetCode39ChecksumTypeWithArg(iOSCode39ChecksumType.disabled);
                        break;
                    }
                case CommonCode39ChecksumType.Enabled:
                    {
                        handler.BKDView.SetCode39ChecksumTypeWithArg(iOSCode39ChecksumType.enabled);
                        break;
                    }
            }
        }
    }

    private static void MapSetCode11ChecksumType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonCode11ChecksumType code11ChecksumType) && (handler.BKDView != null))
        {
            switch (code11ChecksumType)
            {
                case CommonCode11ChecksumType.Disabled:
                    {
                        handler.BKDView.SetCode11ChecksumTypeWithArg(iOSCode11ChecksumType.disabled);
                        break;
                    }
                case CommonCode11ChecksumType.Double:
                    {
                        // TODO: - Missing double enum on iOS side
                        break;
                    }
                case CommonCode11ChecksumType.Single:
                    {
                        handler.BKDView.SetCode11ChecksumTypeWithArg(iOSCode11ChecksumType.single);
                        break;
                    }
            }
        }
    }

    private static void MapSetMsiChecksumType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonMsiChecksumType msiChecksumType) && (handler.BKDView != null))
        {
            switch (msiChecksumType)
            {
                case CommonMsiChecksumType.Disabled:
                    {
                        handler.BKDView.SetMsiChecksumTypeWithArg(iOSMsiChecksumType.disabled);
                        break;
                    }
                case CommonMsiChecksumType.Mod10:
                    {
                        handler.BKDView.SetMsiChecksumTypeWithArg(iOSMsiChecksumType.mod10);
                        break;
                    }
                case CommonMsiChecksumType.Mod1010:
                    {
                        handler.BKDView.SetMsiChecksumTypeWithArg(iOSMsiChecksumType.mod1010);
                        break;
                    }
                case CommonMsiChecksumType.Mod11:
                    {
                        handler.BKDView.SetMsiChecksumTypeWithArg(iOSMsiChecksumType.mod11);
                        break;
                    }
                case CommonMsiChecksumType.Mod1110:
                    {
                        handler.BKDView.SetMsiChecksumTypeWithArg(iOSMsiChecksumType.mod1110);
                        break;
                    }
                case CommonMsiChecksumType.Mod1110IBM:
                    {
                        handler.BKDView.SetMsiChecksumTypeWithArg(iOSMsiChecksumType.mod1110IBM);
                        break;
                    }
                case CommonMsiChecksumType.Mod11IBM:
                    {
                        handler.BKDView.SetMsiChecksumTypeWithArg(iOSMsiChecksumType.mod11IBM);
                        break;
                    }
            }
        }
    }

    private static void MapSetEncodingCharacterSet(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string encodingCharacterSet) && (handler.BKDView != null))
        {
            handler.BKDView.SetEncodingCharacterSetWithArg(encodingCharacterSet);
        }
    }

    private static void MapSetDatamatrixDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetDatamatrixDpmModeEnabledWithArg(enabled);
        }
    }

    private static void MapSetUpcEanDeblurEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetUpcEanDeblurEnabledWithArg(enabled);
        }
    }

    private static void MapSetEnableMisshaped1DEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetMisshaped1DEnabledWithArg(enabled);
        }
    }

    private static void MapSetBarcodeThumbnailOnResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetBarcodeThumbnailOnResultEnabledWithArg(enabled);
        }
    }
    private static void MapSetMaximumResultsCount(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int maximumResultsCount) && (handler.BKDView != null))
        {
            handler.BKDView.SetMaximumResultsCountWithArg(maximumResultsCount);
        }
    }

    private static void MapSetDuplicatesDelayMs(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int duplicatesDelayMs) && (handler.BKDView != null))
        {
            handler.BKDView.SetDuplicatesDelayMsWithArg(duplicatesDelayMs);
        }
    }

    private static void MapSetBarcodeTypeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is BarcodeTypeEventArgs barcodeTypeEventArgs) && (handler.BKDView != null) && (handler.BKDView.Config?.DecoderConfig != null))
        {
            switch (barcodeTypeEventArgs.BarcodeType)
            {
                case Enums.BarcodeType.Aztec:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.aztec, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.AztecCompact:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.aztecCompact, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.QR:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.qr, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.QRMicro:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.qrMicro, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Code128:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.code128, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Code93:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.code93, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Code39:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.code39, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Codabar:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.codabar, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Code11:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.code11, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Msi:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.msi, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.UpcA:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.upcA, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.UpcE:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.upcE, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.UpcE1:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.upcE1, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Ean13:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.ean13, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Ean8:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.ean8, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.PDF417:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.pdf417, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.PDF417Micro:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.pdf417Micro, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Datamatrix:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.datamatrix, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Code25:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.code25, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Interleaved25:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.interleaved25, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Itf14:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.itf14, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Matrix25:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.matrix25, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Datalogic25:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.datalogic25, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Coop25:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.coop25, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Code32:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.code32, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Telepen:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.telepen, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Dotcode:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.dotcode, barcodeTypeEventArgs.Enabled);
                    break;
            }
        }
    }

    private static void MapSetEnableVINRestrictions(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null) && (handler.BKDView.Config?.DecoderConfig != null))
        {
            handler.BKDView.SetEnableVINRestrictionsWithArg(enabled);
        }
    }

    private static void MapSetThresholdBetweenDuplicatesScans(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int thresholdBetweenDuplicatesScans) && (handler.BKDView != null) && (handler.BKDView.Config != null) && (handler.BKDView.Config?.DecoderConfig != null))
        {
            handler.BKDView.SetThresholdBetweenDuplicatesScansWithArg(thresholdBetweenDuplicatesScans);
        }
    }

    private static void MapSetBarcodeTypeLengthRange(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is BarcodeRangeEventArg barcodeRangeEventArg) && (handler.BKDView != null))
        {
            switch (barcodeRangeEventArg.BarcodeType)
            {
                case CommonBarcodeType.Code128:
                    {
                        handler.BKDView.SetBarcodeTypeLengthRangeWithBarcodeType(BarcodeType.code128, barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case CommonBarcodeType.Code93:
                    {
                        handler.BKDView.SetBarcodeTypeLengthRangeWithBarcodeType(BarcodeType.code93, barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case CommonBarcodeType.Code39:
                    {
                        handler.BKDView.SetBarcodeTypeLengthRangeWithBarcodeType(BarcodeType.code39, barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case CommonBarcodeType.Codabar:
                    {
                        handler.BKDView.SetBarcodeTypeLengthRangeWithBarcodeType(BarcodeType.codabar, barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case CommonBarcodeType.Code11:
                    {
                        handler.BKDView.SetBarcodeTypeLengthRangeWithBarcodeType(BarcodeType.code11, barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case CommonBarcodeType.Msi:
                    {
                        handler.BKDView.SetBarcodeTypeLengthRangeWithBarcodeType(BarcodeType.msi, barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    private static void MapConfigureBarkoder(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string configAsJsonString) && (handler.BKDView != null))
        {
            handler.BKDView.ConfigureBarkoder(configAsJsonString);
        }
    }

}
#endif