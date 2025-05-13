#if IOS
using Barkoder.Xamarin;
using Microsoft.Maui.Handlers;
using UIKit;
using BarkoderSDK = Barkoder.Xamarin;
using Plugin.Maui.Barkoder.Interfaces;

using BarcodeResult = Plugin.Maui.Barkoder.Handlers.BarcodeResult;

using CommonBarkoderARMode = Plugin.Maui.Barkoder.Enums.BarkoderARMode;
using iOSBarkoderARMode = Barkoder.Xamarin.BarkoderARMode;

using CommonBarkoderResolution = Plugin.Maui.Barkoder.Enums.BarkoderResolution;
using iOSBarkoderResolution = Barkoder.Xamarin.BarkoderResolution;

using CommonBarkoderCameraPosition = Plugin.Maui.Barkoder.Enums.BarkoderCameraPosition;
using iOSBarkoderCameraPosition = Barkoder.Xamarin.BarkoderCameraPosition;

using CommonBarkoderLocationType = Plugin.Maui.Barkoder.Enums.BarkoderARLocationType;

using CommonBarkoderARHeaderShowMode = Plugin.Maui.Barkoder.Enums.BarkoderARHeaderShowMode;

using CommonBarkoderAROverlayRefresh = Plugin.Maui.Barkoder.Enums.BarkoderAROverlayRefresh;

using iOSBarkoderARLocationType = Barkoder.Xamarin.BarkoderARLocationType;

using iOSBarkoderARHeaderShowMode = Barkoder.Xamarin.BarkoderARHeaderShowMode;

using iOSBarkoderAROverlayRefresh = Barkoder.Xamarin.BarkoderAROverlayRefresh;


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
using ImageData = Plugin.Maui.Barkoder.Handlers.ImageData;

using CommonBarcodeType = Plugin.Maui.Barkoder.Enums.BarcodeType;
using BarcodeType = Barkoder.Xamarin.BarcodeType;
using Foundation;
using Microsoft.CSharp.RuntimeBinder;

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
        //if (handler.BKDView != null)
        //{
        //    handler.BKDView.IsFlashAvailableWithCompletion((isAvailable) =>
        //    {
        //        view.IsFlashAvailable = isAvailable;
        //    });
        //}
    }

    private static void MapMaxZoomFactor(BarkoderViewHandler handler, BarkoderView view)
    {
        //if (handler.BKDView != null)
        //{
        //    handler.BKDView.GetMaxZoomFactorWithCompletion((MaxZoomFactor) =>
        //    {
        //        view.MaxZoomFactor = MaxZoomFactor;
        //    });
        //}
    }

    private static void MapInitCameraProperties(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (handler.BKDView != null)
        {
            handler.BKDView.IsFlashAvailableWithCompletion((isAvailable) =>
            {
                view.IsFlashAvailable = isAvailable;
            });

            handler.BKDView.GetMaxZoomFactorWithCompletion((MaxZoomFactor) =>
            {
                view.MaxZoomFactor = MaxZoomFactor;
            });
        }
    }

      private static void MapScanningIndicatorAlwaysVisibleEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        //if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        //{
        //    view.ScanningIndicatorAlwaysVisibleEnabled = handler.BKDView.ScanningIndicatorAlwaysVisible;
        //}
    }

    private static void MapScanningIndicatorAnimationMode(BarkoderViewHandler handler, BarkoderView view)
    {
        //if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        //{
        //    view.ScanningIndicatorAnimationMode = (int)handler.BKDView.ScanningIndicatorAnimationMode;
        //}
    }

    private static void MapScanningIndicatorColor(BarkoderViewHandler handler, BarkoderView view)
    {
        //if (handler.BKDView != null)
        //{
        //    view.ScanningIndicatorLineColorHex = handler.BKDView.ScanningIndicatorColorHex;
        //}
    }

    private static void MapScanningIndicatorWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        //if (handler.BKDView != null)
        //{
        //    view.ScanningIndicatorLineWidth = handler.BKDView.ScanningIndicatorWidth;
        //}
    }

    private static void MapEnableComposite(BarkoderViewHandler handler, BarkoderView view)
    {
        
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
            view.LocationLineColorHex = handler.BKDView.LocationLineColorHex;
        }
    }

     private static void MapARNonSelectedLocationLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARNonSelectedLocationLineColorHex = handler.BKDView.ARNonSelectedLocationLineColor;
        }

    }

     private static void MapARSelectedLocationLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARSelectedLocationLineColorHex = handler.BKDView.ARSelectedLocationLineColor;
        }

    }

       private static void MapResultDisappearanceDelayMs(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ResultDisappearanceDelayMs = (int)handler.BKDView.ResultDisapperanceDelayMs;
        }
    }

         private static void MapARLocationTransitionSpeed(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARLocationTransitionSpeed = handler.BKDView.LocationTransitionSpeed;
        }
    }

    private static void MapARHeaderTextFormat(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARHeaderTextFormat = handler.BKDView.ARHeaderTextFormat;
        }
    }

    private static void MapARDoubleTapToFreezeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARDoubleTapToFreezeEnabled = handler.BKDView.ARDoubleTapToFreeze;
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

      private static void MapARSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARSelectedLocationLineWidth = handler.BKDView.ARSelectedLocationWidth;
        }
    }

      private static void MapARNonSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARNonSelectedLocationLineWidth = handler.BKDView.ARNonSelectedLocationWidth;
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

            if (barkoderResolution == iOSBarkoderResolution.FHD)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.FHD;
            }
            else if (barkoderResolution == iOSBarkoderResolution.HD)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.HD;
            }
            else if (barkoderResolution == iOSBarkoderResolution.UHD)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.UHD;
            }
        }

    }

    private static void MapBarkoderARMode(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            iOSBarkoderARMode barkoderARMode = handler.BKDView.BarkoderARMode;

            if (barkoderARMode == iOSBarkoderARMode.OFF)
            {
                view.BarkoderARMode = Enums.BarkoderARMode.OFF;
            }
            else if (barkoderARMode == iOSBarkoderARMode.INTERACTIVEDISABLED)
            {
                view.BarkoderARMode = Enums.BarkoderARMode.InteractiveDisabled;
            }
            else if (barkoderARMode == iOSBarkoderARMode.INTERACTIVEENABLED)
            {
                view.BarkoderARMode = Enums.BarkoderARMode.InteractiveEnabled;
            }
            else if (barkoderARMode == iOSBarkoderARMode.NONINTERACTIVE)
            {
                view.BarkoderARMode = Enums.BarkoderARMode.NonInteractive;
            }
        }

    }

      private static void MapBarkoderARHeaderShowMode(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            iOSBarkoderARHeaderShowMode barkoderHeaderShowMode = handler.BKDView.BarkoderARHeaderShowMode;

            if (barkoderHeaderShowMode == iOSBarkoderARHeaderShowMode.NEVER)
            {
                view.BarkoderARHeaderShowMode = Enums.BarkoderARHeaderShowMode.NEVER;
            }
            else if (barkoderHeaderShowMode == iOSBarkoderARHeaderShowMode.ALWAYS)
            {
                view.BarkoderARHeaderShowMode = Enums.BarkoderARHeaderShowMode.ALWAYS;
            }
            else if (barkoderHeaderShowMode == iOSBarkoderARHeaderShowMode.ONSELECTED)
            {
                view.BarkoderARHeaderShowMode = Enums.BarkoderARHeaderShowMode.ONSELECTED;
            }        
        }

    }

    private static void MapBarkoderARLocationType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            iOSBarkoderARLocationType barkoderLocationType = handler.BKDView.BarkoderARLocationType;

            if (barkoderLocationType == iOSBarkoderARLocationType.NONE)
            {
                view.BarkoderARLocationType = Enums.BarkoderARLocationType.NONE;
            }
            else if (barkoderLocationType == iOSBarkoderARLocationType.TIGHT)
            {
                view.BarkoderARLocationType = Enums.BarkoderARLocationType.TIGHT;
            }
            else if (barkoderLocationType == iOSBarkoderARLocationType.BOUNDINGBOX)
            {
                view.BarkoderARLocationType = Enums.BarkoderARLocationType.BOUNDINGBOX;
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

    private static void MapIdDocumentMasterChecksumEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.IdDocumentMasterCheckSumEnabled = handler.BKDView.DatamatrixDpmModeEnabled;
        }
    }

    private static void MapDatamatrixDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.DatamatrixDpmModeEnabled = handler.BKDView.DatamatrixDpmModeEnabled;
        }
    }

    private static void MapQRDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.QRDpmModeEnabled = handler.BKDView.QRDpmModeEnabled;
        }
    }

    private static void MapQRMicroDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            // Correctly use the accessible property here
            view.QRMIcroDpmModeEnabled = handler.BKDView.QRMicroDpmModeEnabled;
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

     private static void MapARHeaderHeight(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARHeaderHeight = handler.BKDView.ARHeaderHeight;
        }

    }

      private static void MapARHeaderMaxTextHeight(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARHeaderMaxTextHeight = handler.BKDView.ARHeaderMaxTextHeight;
        }
    }

      private static void MapARHeaderMinTextHeight(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARHeaderMinTextHeight = handler.BKDView.ARHeaderMinTextHeight;
        }
    }

        private static void MapARSelectedHeaderTextColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARSelectedHeaderTextColorHex = handler.BKDView.ARSelectedHeaderTextColor;
        }
    }


        private static void MapARNonSelectedHeaderTextColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARNonSelectedHeaderTextColorHex = handler.BKDView.ARNonSelectedHeaderTextColor;
        }
    }

           private static void MapARHeaderVerticalTextMargin(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.HeaderARVerticalTextMargin = handler.BKDView.ARHeaderVerticalTextMargin;
        }
    }

           private static void MapARHeaderHorizontalTextMargin(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.HeaderARHorizontalTextMargin = handler.BKDView.ARHeaderHorizontalTextMargin;
        }

    }


    private static void MapDuplicatesDelayMs(BarkoderViewHandler handler, BarkoderView view)
    {
        //if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        //{
        //    view.DuplicatesDelayMs = (int)handler.BKDView.DuplicatesDelayMs;
        //}
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
                BKDecoderResult[] results = completion.Results;

                // Maui wrapper results
                BarcodeResult[] barcodeResults = new BarcodeResult[results.Length];

                for (int i = 0; i < results.Length; i++)
                {
                    List<ImageData> mrzImages = new List<ImageData>();

                    // Convert NSDictionary to Dictionary<string, object>
                    NSDictionary extra = results[i].Extra;
                    Dictionary<string, object>? extraDict = null;

                    if (extra != null && extra.Count > 0)
                    {
                        extraDict = new Dictionary<string, object>();
                        foreach (var key in extra.Keys)
                        {
                            if (key is NSString nsKey)
                            {
                                NSObject? value = extra.ObjectForKey(nsKey);
                                if (value != null)
                                {
                                    // Handle value types accordingly
                                    if (value is NSString stringValue)
                                    {
                                        extraDict[nsKey.ToString()] = stringValue.ToString();
                                    }
                                    else if (value is NSNumber numberValue)
                                    {
                                        extraDict[nsKey.ToString()] = numberValue.ToString(); // Convert number to string
                                    }
                                    else
                                    {
                                        extraDict[nsKey.ToString()] = value.ToString(); // Fallback for unknown types
                                    }
                                }
                            }
                        }
                    }

                    // Convert Images
                    if (results[i].Images != null)
                    {
                        foreach (var image in results[i].Images)
                        {
                            ImageSource? imageSource = UIImageToImageSource(image.Image);
                            if (imageSource != null)
                            {
                                mrzImages.Add(new ImageData(image.Name, imageSource));
                            }
                        }
                    }

                    // Create BarcodeResult for each scanned result
                    BarcodeResult barcodeResult = new BarcodeResult(
                        results[i].TextualData,
                        results[i].BarcodeTypeName,
                        extraDict, // Pass the converted dictionary here
                        "", // Provide the CharacterSet
                        mrzImages
                    );

                    barcodeResults[i] = barcodeResult;
                }

                // Pass results back to the delegate
                barkoderDelegate?.DidFinishScanning(barcodeResults, Base64ToImageSource(completion.ImageInBase64));
            });
        }
    }






    private static void MapScanImage(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Ensure `arg3` is cast to `dynamic` to access its properties.
        try
        {
            dynamic arguments = arg3;

            if (arguments.barkoderDelegate is IBarkoderDelegate barkoderDelegate)
            {
                Console.WriteLine("MapScanImage called with base64Image.");

                string base64Image = arguments.base64Image;

                // Create UIImage from base64 string
                UIImage image = ConvertBase64ToUIImage(base64Image);

                // Call the scanImage method, just like startScanning, and use the completion handler
                handler.BKDView?.ScanImage(image, handler.BKDView?.Config, (completion) =>
                {
                    // SDK Results
                    BKDecoderResult[] results = completion.Results;

                    // Maui wrapper results
                    BarcodeResult[] barcodeResults = new BarcodeResult[results.Length];

                    // Converting from SDK results -> Maui wrapper results
                    for (int i = 0; i < results.Length; i++)
                    {
                        List<ImageData> mrzImages = new List<ImageData>();
                        // Convert NSDictionary to Dictionary<string, object>
                        NSDictionary extra = results[i].Extra;
                        Dictionary<string, object>? extraDict = null;

                        if (extra != null && extra.Count > 0)
                        {
                            extraDict = new Dictionary<string, object>();
                            foreach (var key in extra.Keys)
                            {
                                if (key is NSString nsKey)
                                {
                                    NSObject? value = extra.ObjectForKey(nsKey);
                                    if (value != null)
                                    {
                                        // Handle value types accordingly
                                        if (value is NSString stringValue)
                                        {
                                            extraDict[nsKey.ToString()] = stringValue.ToString();
                                        }
                                        else if (value is NSNumber numberValue)
                                        {
                                            extraDict[nsKey.ToString()] = numberValue.ToString(); // Convert number to string
                                        }
                                        else
                                        {
                                            extraDict[nsKey.ToString()] = value.ToString(); // Fallback for unknown types
                                        }
                                    }
                                }
                            }
                        }

                        // Assuming you have a similar structure to access images
                        if (results[i].Images != null)
                        {
                            foreach (var image in results[i].Images)
                            {
                                ImageSource? imageSource = UIImageToImageSource(image.Image);
                                if (imageSource != null)
                                {
                                    mrzImages.Add(new ImageData(image.Name, imageSource));
                                }
                            }
                        }

                        // Create BarcodeResult for each scanned result and convert UIImage to ImageSource
                        BarcodeResult barcodeResult = new BarcodeResult(
                            results[i].TextualData,
                            results[i].BarcodeTypeName,
                            extraDict,
                            "", // Provide the CharacterSet
                            mrzImages
                        );

                        barcodeResults[i] = barcodeResult;
                    }

                    // Pass results back to the delegate
                    barkoderDelegate?.DidFinishScanning(barcodeResults, Base64ToImageSource(completion.ImageInBase64));
                });
            }
        }
        catch (RuntimeBinderException e)
        {
            Console.WriteLine("MapScanImage: Argument type is incorrect or missing required properties.");
            Console.WriteLine(e.Message);
        }
    }



    private static UIImage ConvertBase64ToUIImage(string base64Image)
    {
        try
        {
            // Decode base64 string to byte array
            byte[] imageData = Convert.FromBase64String(base64Image);

            // Create NSData from byte array
            NSData data = NSData.FromArray(imageData);

            // Create UIImage from NSData
            return UIImage.LoadFromData(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting base64 string to UIImage: {ex.Message}");
            return null;
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

    private static void MapFreezeScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.BKDView?.FreezeScanning();
    }


    private static void MapUnfreezeScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.BKDView?.UnfreezeScanning();
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

    private static void MapSetScanningIndicatorAlwaysVisibleEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetScanningIndicatorAlwaysVisible(enabled);
        }

    }

    private static void MapSetShowDuplicatesLocation(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetShowDuplicatesLocation(enabled);
        }

    }

    private static void MapSetScanningIndicatorAnimationMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int animationMode) && (handler.BKDView != null))
        {
            handler.BKDView.SetScanningIndicatorAnimationMode(animationMode);
        }

    }

    private static void MapSetScanningIndicatorColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor)
        {
            handler.BKDView?.SetScanningIndicatorColorHex(hexColor);
        }
    }

    private static void MapSetScanningIndicatorWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int indicatorWidth) && (handler.BKDView != null))
        {
            handler.BKDView.SetScanningIndicatorWidth(indicatorWidth);
        }
    }

    private static void MapSetEnableComposite(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int enableComposite) && (handler.BKDView != null))
        {
            handler.BKDView.SetEnableComposite(enableComposite);
        }
    }

    public static void MapSetCustomOption(BarkoderViewHandler handler, BarkoderView view, object? argument)
    {
        if (handler.BKDView != null && handler.BKDView.Config != null && argument is ValueTuple<string, int> customOption)
        {
            var (optionName, optionValue) = customOption; // Deconstruct the tuple
            Console.WriteLine($"Setting custom option: {optionName} = {optionValue}");
            handler.BKDView.SetCustomOption(optionName, optionValue);
            // Assuming BKDView has a method to handle custom options
           
        }
        else
        {
            Console.WriteLine("MapSetCustomOption: BKDView or Config is null, or argument is invalid.");
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
     private static void MapSetARSelectedLocationColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string color) && (handler.BKDView != null))
        {
            handler.BKDView.SetARSelectedLocationLineColor(color);
        }
    }

      private static void MapSetARNonSelectedLocationColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string color) && (handler.BKDView != null))
        {
            handler.BKDView.SetARNonSelectedLocationLineColor(color);
        }

    }

         private static void MapSetARLocationTransitionSpeed(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double transitionSpeed) && (handler.BKDView != null))
        {
            handler.BKDView.SetARLocationTransitionSpeed((float)transitionSpeed);
        }
    }

    private static void MapSetDoubleTapToFreezeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool doubleTapToFreez) && (handler.BKDView != null))
        {
            handler.BKDView.SetARDoubleTapToFreezeEnabled(doubleTapToFreez);
        }
    }

    private static void MapSetARHeaderTextFormat(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string headerFormatText) && (handler.BKDView != null))
        {
            handler.BKDView.SetARHeaderTextFormat(headerFormatText);
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

     private static void MapSetARSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double locationWidth) && (handler.BKDView != null))
        {
            handler.BKDView.SetARSelectedLocationLineWidth((float)locationWidth);
        }
    }

     private static void MapSetARNonSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double locationWidth) && (handler.BKDView != null))
        {
            handler.BKDView.SetARNonSelectedLocationLineWidth((float)locationWidth);
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
                case CommonBarkoderResolution.FHD:
                    {
                        handler.BKDView.SetBarkoderResolutionWithArg(iOSBarkoderResolution.FHD);
                        break;
                    }
                case CommonBarkoderResolution.HD:
                    {
                        handler.BKDView.SetBarkoderResolutionWithArg(iOSBarkoderResolution.HD);
                        break;
                    }
                case CommonBarkoderResolution.UHD:
                    {
                        handler.BKDView.SetBarkoderResolutionWithArg(iOSBarkoderResolution.UHD);
                        break;
                    }
            }
        }
    }

    private static void MapSetBarkoderARMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonBarkoderARMode barkoderARMode) && (handler.BKDView != null))
        {
            switch (barkoderARMode)
            {
                case CommonBarkoderARMode.OFF:
                    {
                        handler.BKDView.SetBarkoderARMode(0);
                        break;
                    }
                case CommonBarkoderARMode.InteractiveDisabled:
                    {
                        handler.BKDView.SetBarkoderARMode(1);
                        break;
                    }
                case CommonBarkoderARMode.InteractiveEnabled:
                    {
                        handler.BKDView.SetBarkoderARMode(2);
                        break;
                    }
                case CommonBarkoderARMode.NonInteractive:
                    {
                        handler.BKDView.SetBarkoderARMode(3);
                        break;
                    }
            }
        }

    }

     private static void MapSetBarkoderARHeaderShowMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonBarkoderARHeaderShowMode barkoderARHeaderShowMode) && (handler.BKDView != null))
        {
            switch (barkoderARHeaderShowMode)
            {
                case CommonBarkoderARHeaderShowMode.NEVER:
                    {
                        handler.BKDView.SetBarkoderARHeaderShowMode(0);
                        break;
                    }
                case CommonBarkoderARHeaderShowMode.ONSELECTED:
                    {
                        handler.BKDView.SetBarkoderARHeaderShowMode(2);
                        break;
                    }
                case CommonBarkoderARHeaderShowMode.ALWAYS:
                    {
                        handler.BKDView.SetBarkoderARHeaderShowMode(1);
                        break;
                    }

            }
        }
    }

      private static void MapSetBarkoderARLocationType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonBarkoderLocationType barkoderARLocationType) && (handler.BKDView != null))
        {
            switch (barkoderARLocationType)
            {
                case CommonBarkoderLocationType.NONE:
                    {
                        handler.BKDView.SetBarkoderARLocationType(0);
                        break;
                    }
                case CommonBarkoderLocationType.TIGHT:
                    {
                        handler.BKDView.SetBarkoderARLocationType(1);
                        break;
                    }
                case CommonBarkoderLocationType.BOUNDINGBOX:
                    {
                        handler.BKDView.SetBarkoderARLocationType(2);
                        break;
                    }
            
            }
        }

    }

    private static void MapSetBarkoderARoverlayRefresh(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonBarkoderAROverlayRefresh barkoderAROverlayRefresh) && (handler.BKDView != null))
        {
            switch (barkoderAROverlayRefresh)
            {
                case CommonBarkoderAROverlayRefresh.NORMAL:
                    {
                        handler.BKDView.SetBarkoderARoverlayRefresh(0);
                        break;
                    }
                case CommonBarkoderAROverlayRefresh.SMOOTH:
                    {
                        handler.BKDView.SetBarkoderARoverlayRefresh(1);
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
                        handler.BKDView.SetDecodingSpeedWithArg(iOSDecodingSpeed.Slow);
                        break;
                    }
                case CommonDecodingSpeed.Normal:
                    {
                        handler.BKDView.SetDecodingSpeedWithArg(iOSDecodingSpeed.Normal);
                        break;
                    }
                case CommonDecodingSpeed.Fast:
                    {
                        handler.BKDView.SetDecodingSpeedWithArg(iOSDecodingSpeed.Fast);
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

    private static void MapSetIdDocumentMasterChecksumEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetIdDocumentMasterChecksumEnabledWithArg(enabled);
            view.IsIdDocumentMasterChecksumEnabled = enabled;
        }
    }

    private static void MapSetDatamatrixDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetDatamatrixDpmModeEnabledWithArg(enabled);
            view.IsDatamatrixDpmModeEnabled = enabled;
        }
    }

    private static void MapSetQRDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetQRDpmModeEnabledWithArg(enabled);
            view.IsQRDpmModeEnabled = enabled;
        }
    }

    private static void MapSetQRMicroDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetQRMicroDpmModeEnabledWithArg(enabled);
            view.IsQRMicroDpmModeEnabled = enabled;
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

     private static void MapSetARHeaderHeight(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double headerHeight) && (handler.BKDView != null))
        {
            handler.BKDView.SetARHeaderHeight((float)headerHeight);
        }
    
}

     private static void MapSetARHeaderMaxTextHeight(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double maxTextHeight) && (handler.BKDView != null))
        {
            handler.BKDView.SetARHeaderMaxTextHeight((float)maxTextHeight);
        }
    }


     private static void MapSetARHeaderMinTextHeight(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double minTextHeight) && (handler.BKDView != null))
        {
            handler.BKDView.SetARHeaderMinTextHeight((float)minTextHeight);
        }
    }

     private static void MapSetARSelectedHeaderTextColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string color) && (handler.BKDView != null))
        {
            handler.BKDView.SetARSelectedHeaderTextColor(color);
        }
    }

     private static void MapSetARNonSelectedHeaderTextColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {

        if ((arg3 is string color) && (handler.BKDView != null))
        {
            handler.BKDView.SetARNonSelectedHeaderTextColor(color);
        }
    }

       private static void MapSetARHeaderVerticalTextMargin(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double verticalTextMargin) && (handler.BKDView != null))
        {
            handler.BKDView.SetARHeaderVerticalTextMargin((float)verticalTextMargin);
        }
    }

       private static void MapSetARHeaderHorizontalTextMargin(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double horizontalTextMargin) && (handler.BKDView != null))
        {
            handler.BKDView.SetARHeaderHorizontalTextMargin((float)horizontalTextMargin);
        }
    }

         private static void MapSetResultDisappearanceDelayMs(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int disappearanceDelayMs) && (handler.BKDView != null))
        {
            handler.BKDView.SetResultDisappearanceDelayMs(disappearanceDelayMs);
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
                case Enums.BarcodeType.IDDocument:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.idDocument, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Databar14:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.databar14, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.DatabarLimited:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.databarLimited, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.DatabarExpanded:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.databarExpanded, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.PostalIMB:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.postalIMB, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Postnet:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.postnet, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.Planet:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.planet, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.AustralianPost:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.australianPost, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.RoyalMail:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.royalMail, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.KIX:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.kix, barcodeTypeEventArgs.Enabled);
                    break;
                case Enums.BarcodeType.JapanesePost:
                    handler.BKDView.SetBarcodeTypeEnabledWithBarcodeType(BarcodeType.japanesePost, barcodeTypeEventArgs.Enabled);
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

    private static void MapSetDynamicExposure(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int dynamicExposure) && (handler.BKDView != null))
        {
            handler.BKDView.SetDynamicExposure(dynamicExposure);
        }
    }

    private static void MapSetCamera(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonBarkoderCameraPosition barkoderPosition) && (handler.BKDView != null))
        {
            switch (barkoderPosition)
            {
                case CommonBarkoderCameraPosition.BACK:
                    {
                        handler.BKDView.setCamera(BarkoderCameraPosition.BACK);
                        break;
                    }
                case CommonBarkoderCameraPosition.FRONT:
                    {
                        handler.BKDView.setCamera(BarkoderCameraPosition.FRONT);
                        break;
                    }
               
            }
        }


    }

    private static void MapSetCentricFocusAndExposure(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool centricFocus) && (handler.BKDView != null))
        {
            handler.BKDView.SetCentricFocusAndExposure(centricFocus);
        }
    }

    private static void MapSetUPCE1expandToUPCA(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetUPCE1expandToUPCA(enabled);
        }
    }

    private static void MapSetUPCEexpandToUPCA(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetUPCEexpandToUPCA(enabled);
        }
    }

    private static void MapSetVideoStabilization(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.setVideoStabilizationWithArg(enabled);
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

    private static ImageSource Base64ToImageSource(String imageInBase64format)
    {
        byte[] imageBytes = Convert.FromBase64String(imageInBase64format);

        MemoryStream stream = new MemoryStream(imageBytes);

        ImageSource imageSource = ImageSource.FromStream(() => stream);

        return imageSource;
    }

    public static ImageSource UIImageToImageSource(UIImage image)
    {
        if (image == null)
            return null;

        return ImageSource.FromStream(() => image.AsPNG().AsStream());
    }

}
#endif