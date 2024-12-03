#if ANDROID
using Microsoft.Maui.Handlers;
using Com.Barkoder;
using Android.Content;
using Android.Graphics;
using System.Drawing;

using CommonBarkoderResolution = Plugin.Maui.Barkoder.Enums.BarkoderResolution;
using AndroidBarkoderResolution = Com.Barkoder.Enums.BarkoderResolution;

using CommonDecodingSpeed = Plugin.Maui.Barkoder.Enums.DecodingSpeed;
using AndroidDecodingSpeed = Com.Barkoder.Barkoder.DecodingSpeed;

using CommonFormattingType = Plugin.Maui.Barkoder.Enums.FormattingType;
using AndroidFormattingType = Com.Barkoder.Barkoder.FormattingType;

using CommonMsiChecksumType = Plugin.Maui.Barkoder.Enums.MsiChecksumType;
using AndroidMsiChecksumType = Com.Barkoder.Barkoder.MsiChecksumType;

using CommonCode11ChecksumType = Plugin.Maui.Barkoder.Enums.Code11ChecksumType;
using AndroidCode11ChecksumType = Com.Barkoder.Barkoder.Code11ChecksumType;

using BarcodeResult = Plugin.Maui.Barkoder.Handlers.BarcodeResult;
using ImageData = Plugin.Maui.Barkoder.Handlers.ImageData;

using CommonCode39ChecksumType = Plugin.Maui.Barkoder.Enums.Code39ChecksumType;
using AndroidCode39ChecksumType = Com.Barkoder.Barkoder.Code39ChecksumType;

using AndroidX.AppCompat.App;
using Plugin.Maui.Barkoder.Interfaces;
using Org.Json;
using Java.IO;
using Android.Util;
using Com.Barkoder.Interfaces;
using static Android.Service.Carrier.CarrierMessagingService;

namespace Plugin.Maui.Barkoder.Controls;

//Implement handler for Android platform here
public partial class BarkoderViewHandler : ViewHandler<BarkoderView, Android.Views.View>
{


    private Com.Barkoder.BarkoderView? BKDView;

    protected override Android.Views.View CreatePlatformView()
    {
        Context context = Context;

        this.BKDView = new Com.Barkoder.BarkoderView(context);

        return BKDView;
    }

    private static void MapLicenseKey(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            handler.BKDView.Config = new Com.Barkoder.BarkoderConfig(handler.BKDView.Context, view.LicenseKey, null);
        }
    }

    private static void MapIsFlashAvailable(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            AndroidBarkoderView AndroidBarkoderView = new AndroidBarkoderView(handler.BKDView);

            AndroidBarkoderView.isFlashAvailable((flashAvailable) =>
            {
                view.IsFlashAvailable = flashAvailable;
            });
        }
    }

    private static void MapMaxZoomFactor(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            AndroidBarkoderView AndroidBarkoderView = new AndroidBarkoderView(handler.BKDView);

            AndroidBarkoderView.getMaxZoomFactor((maxZoomFactor) =>
            {
                view.MaxZoomFactor = maxZoomFactor;
            });
        }
    }

    private static void MapRoiLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.RoiLineColorHex = Util.IntColorToHexColor(handler.BKDView.Config.RoiLineColor);
        }
    }

    private static void MapLocationLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.LocationLineColorHex = Util.IntColorToHexColor(handler.BKDView.Config.LocationLineColor);
        }
    }

    private static void MapRoiOverlayBackgroundColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.RoiOverlayBackgroundColorHex = Util.IntColorToHexColor(handler.BKDView.Config.RoiOverlayBackgroundColor);
        }
    }

    private static void MapEncodingCharacterSet(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.EncodingCharacterSet = handler.BKDView.Config.DecoderConfig.EncodingCharacterSet;
        }
    }

    private static void MapLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.LocationLineWidth = handler.BKDView.Config.LocationLineWidth;
        }
    }

    private static void MapImageResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ImageResultEnabled = handler.BKDView.Config.ImageResultEnabled;
        }
    }

    private static void MapLocationInImageResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.LocationInImageResultEnabled = handler.BKDView.Config.LocationInImageResultEnabled;
        }
    }

    private static void MapLocationInPreviewEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.LocationInPreviewEnabled = handler.BKDView.Config.LocationInPreviewEnabled;
        }
    }
    
    private static void MapPinchToZoomEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.PinchToZoomEnabled = handler.BKDView.Config.PinchToZoomEnabled;
        }
    }

    private static void MapBeepOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.BeepOnSuccessEnabled = handler.BKDView.Config.BeepOnSuccessEnabled;
        }
    }

    private static void MapVibrateOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.VibrateOnSuccessEnabled = handler.BKDView.Config.VibrateOnSuccessEnabled;
        }
    }

    private static void MapCloseSessionOnResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.CloseSessionOnResultEnabled = handler.BKDView.Config.CloseSessionOnResultEnabled;
        }
    }

    private static void MapVersion(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.Version = Com.Barkoder.Barkoder.Version;
        }
    }

    private static void MapBarkoderResolution(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidBarkoderResolution barkoderResolution = handler.BKDView.Config.BarkoderResolution;

            // Converting from android BarkoderResolution to CommonBarkoderResolution
            if (barkoderResolution == AndroidBarkoderResolution.High)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.High;
            }
            else if (barkoderResolution == AndroidBarkoderResolution.Normal)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.Normal;
            }
        }
    }

    private static void MapDecodingSpeed(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidDecodingSpeed decodingSpeed = handler.BKDView.Config.DecoderConfig.DecodingSpeed;

            if (decodingSpeed == AndroidDecodingSpeed.Slow)
            {
                view.DecodingSpeed = CommonDecodingSpeed.Slow;
            }
            else if (decodingSpeed == AndroidDecodingSpeed.Normal)
            {
                view.DecodingSpeed = CommonDecodingSpeed.Normal;
            }
            else if (decodingSpeed == AndroidDecodingSpeed.Fast)
            {
                view.DecodingSpeed = CommonDecodingSpeed.Fast;
            }
        }
    }

    private static void MapFormattingType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidFormattingType formattingType = handler.BKDView.Config.DecoderConfig.FormattingType;

            if (formattingType == AndroidFormattingType.Aamva)
            {
                view.FormattingType = CommonFormattingType.AAMVA;
            }
            else if (formattingType == AndroidFormattingType.Automatic)
            {
                view.FormattingType = CommonFormattingType.Automatic;
            }
            else if (formattingType == AndroidFormattingType.Disabled)
            {
                view.FormattingType = CommonFormattingType.Disabled;
            }
            else if (formattingType == AndroidFormattingType.Gs1)
            {
                view.FormattingType = CommonFormattingType.GS1;
            }
        }
    }

    private static void MapCode39ChecksumType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidCode39ChecksumType code39ChecksumType = handler.BKDView.Config.DecoderConfig.Code39.ChecksumType;

            if (code39ChecksumType == AndroidCode39ChecksumType.Disabled)
            {
                view.Code39ChecksumType = CommonCode39ChecksumType.Disabled;
            }
            else if (code39ChecksumType == AndroidCode39ChecksumType.Enabled)
            {
                view.Code39ChecksumType = CommonCode39ChecksumType.Enabled;
            }
        }
    }

    private static void MapCode11ChecksumType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidCode11ChecksumType code11ChecksumType = handler.BKDView.Config.DecoderConfig.Code11.ChecksumType;

            if (code11ChecksumType == AndroidCode11ChecksumType.Disabled)
            {
                view.Code11ChecksumType = CommonCode11ChecksumType.Disabled;
            }
            else if (code11ChecksumType == AndroidCode11ChecksumType.Double)
            {
                view.Code11ChecksumType = CommonCode11ChecksumType.Double;
            }
            else if (code11ChecksumType == AndroidCode11ChecksumType.Single)
            {
                view.Code11ChecksumType = CommonCode11ChecksumType.Single;
            }
        }
    }

    private static void MapMsiChecksumType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidMsiChecksumType msiChecksumType = handler.BKDView.Config.DecoderConfig.Msi.ChecksumType;

            if (msiChecksumType == AndroidMsiChecksumType.Disabled)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Disabled;
            }
            else if (msiChecksumType == AndroidMsiChecksumType.Mod10)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod10;
            }
            else if (msiChecksumType == AndroidMsiChecksumType.Mod11)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod11;
            }
            else if (msiChecksumType == AndroidMsiChecksumType.Mod1010)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod1010;
            }
            else if (msiChecksumType == AndroidMsiChecksumType.Mod1110)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod1110;
            }
            else if (msiChecksumType == AndroidMsiChecksumType.Mod1110IBM)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod1110IBM;
            }
            else if (msiChecksumType == AndroidMsiChecksumType.Mod11IBM)
            {
                view.MsiChecksumType = CommonMsiChecksumType.Mod11IBM;
            }
        }
    }

    private static void MapIdDocumentMasterChecksumEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            if (handler.BKDView.Config.DecoderConfig.IDDocument.MasterChecksumType == Com.Barkoder.Barkoder.StandardChecksumType.Enabled)
            {
                view.IdDocumentMasterCheckSumEnabled = true;
            }
            else
            {
                view.IdDocumentMasterCheckSumEnabled = false;
            }
        }
    }


    private static void MapDatamatrixDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.DatamatrixDpmModeEnabled = handler.BKDView.Config.DecoderConfig.Datamatrix.DpmMode;
        }
    }

    private static void MapQRDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.QRDpmModeEnabled = handler.BKDView.Config.DecoderConfig.Qr.DpmMode;
        }
    }

    private static void MapQRMicroDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.QRMIcroDpmModeEnabled = handler.BKDView.Config.DecoderConfig.QRMicro.DpmMode;
        }
    }

    private static void MapUpcEanDeblurEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.UpcEanDeblurEnabled = handler.BKDView.Config.DecoderConfig.UpcEanDeblur;
        }
    }

    private static void MapEnableMisshaped1DEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.EnableMisshaped1DEnabled = handler.BKDView.Config.DecoderConfig.EnableMisshaped1D;
        }
    }

    private static void MapBarcodeThumbnailOnResultEnabledProperty(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.BarcodeThumbnailOnResultEnabled = handler.BKDView.Config.ThumbnailOnResulEnabled;
        }
    }

    private static void MapMaximumResultsCount(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.MaximumResultsCount = handler.BKDView.Config.DecoderConfig.MaximumResultsCount;
        }
    }

    private static void MapDuplicatesDelayMs(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.DuplicatesDelayMs = handler.BKDView.Config.DecoderConfig.DuplicatesDelayMs;
        }
    }

    private static void MapMulticodeCachingDuration(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.DuplicatesDelayMs = Com.Barkoder.
                Barkoder.Config.GlobalOptionMulticodeCachingDuration;
        }
    }

    private static void MapVINRestrictionsEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.VINRestrictionsEnabled = handler.BKDView.Config.DecoderConfig.EnableVINRestrictions;
        }
    }

    private static void MapThresholdBetweenDuplicatesScans(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ThresholdBetweenDuplicatesScans = handler.BKDView.Config.ThresholdBetweenDuplicatesScans;
        }
    }

    private static void MapRegionOfInterest(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            Com.Barkoder.Barkoder.BKRect Rect = handler.BKDView.Config.RegionOfInterest;

            view.RegionOfInterest = (
                (int)Rect.Left,
                (int)Rect.Top,
                (int)Rect.Height,
                (int)Rect.Width
            );
        }
    }

    private static void MapStartCamera(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.BKDView?.StartCamera();
    }

    private static void MapStartScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is IBarkoderDelegate barkoderDelegate) && (handler.BKDView != null))
        {
            AndroidBarkoderView AndroidBarkoderView = new AndroidBarkoderView(barkoderDelegate, handler.BKDView);
            AndroidBarkoderView.StartScanning();
        }

    }

    private static void MapScanImage(BarkoderViewHandler handler, BarkoderView view, object? arg)
    {
        // Extract base64Image and barkoderDelegate from the anonymous object
        var arguments = (dynamic)arg;
        string base64Image = arguments.base64Image;
        IBarkoderDelegate barkoderDelegate = arguments.barkoderDelegate;

        if (handler.BKDView != null && !string.IsNullOrEmpty(base64Image))
        {
            // Decode base64 string to Bitmap
            byte[] imageBytes = Convert.FromBase64String(base64Image);
            Bitmap bitmap;

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                bitmap = BitmapFactory.DecodeStream(ms);  // Convert base64 string to Bitmap
            }

            Context context = handler.Context;

            AndroidBarkoderView AndroidBarkoderView = new AndroidBarkoderView(barkoderDelegate, handler.BKDView);
            AndroidBarkoderView.ScanImage(bitmap,handler.BKDView.Config, context);

        }
        else
        {
      
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
        if (handler.BKDView != null)
        {
            handler.BKDView.Config.RegionOfInterestVisible = true;
        }
    }

    private static void MapFlashEnable(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler.BKDView?.SetFlashEnabled(enabled);
        }
    }

    private static void MapSetZoomFactor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is float zoomFactor)
        {
            handler.BKDView?.SetZoomFactor(zoomFactor);
        }
    }

    private static void MapSetPinchToZoomEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            if (handler.BKDView != null) {
                handler.BKDView.Config.PinchToZoomEnabled = enabled;
            }
        }
    }

    private static void MapSetRegionOfInterestVisible(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool visible)
        {
            if (handler.BKDView != null)
            {
                handler.BKDView.Config.RegionOfInterestVisible = visible;
            }
        }
    }

    private static void MapSetRoiLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is float width)
        {
            if (handler.BKDView != null)
            {
                handler.BKDView.Config.RoiLineWidth = width;
            }
        }
    }

    private static void MapSetVibrateOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.VibrateOnSuccessEnabled = enabled;
        }
    }

    private static void MapSetBeepOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.BeepOnSuccessEnabled = enabled;
        }
    }

    private static void MapSetLocationInImageResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.LocationInImageResultEnabled = enabled;
        }
    }

    private static void MapSetLocationInPreviewEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.LocationInPreviewEnabled = enabled;
        }
    }

    private static void MapSetImageResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ImageResultEnabled = enabled;
        }
    }

    private static void MapSetCloseSessionOnResult(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.CloseSessionOnResultEnabled = enabled;
        }
    }

    private static void MapSetRoiLineColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string hexColor) && (handler.BKDView != null))
        {
            handler.BKDView.Config.RoiLineColor = Util.HexColorToIntColor(hexColor);
        }
    }

    private static void MapSetLocationLineColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string hexColor) && (handler.BKDView != null))
        {
            handler.BKDView.Config.LocationLineColor = Util.HexColorToIntColor(hexColor);
        }
    }

    private static void MapSetRoiOverlayBackgroundColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string hexColor) && (handler.BKDView != null))
        {
            handler.BKDView.Config.RoiOverlayBackgroundColor = Util.HexColorToIntColor(hexColor);
        }
    }

    private static void MapSetLocationLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is float width) && (handler.BKDView != null))
        {
            handler.BKDView.Config.LocationLineWidth = width;
        }
    }

    private static void MapSetThreadsLimit(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // TODO: - Setting threads limit
    }

    private static void MapSetRegionOfInterest(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int[] roi) && (handler.BKDView != null))
        {
            try
            {
                handler.BKDView.Config.SetRegionOfInterest(roi[0], roi[1], roi[2], roi[3]);
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
                        handler.BKDView.Config.BarkoderResolution = AndroidBarkoderResolution.High;
                        break;
                    }
                case CommonBarkoderResolution.Normal:
                    {
                        handler.BKDView.Config.BarkoderResolution = AndroidBarkoderResolution.Normal;
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
                        handler.BKDView.Config.DecoderConfig.DecodingSpeed = AndroidDecodingSpeed.Slow;
                        break;
                    }
                case CommonDecodingSpeed.Normal:
                    {
                        handler.BKDView.Config.DecoderConfig.DecodingSpeed = AndroidDecodingSpeed.Normal;
                        break;
                    }
                case CommonDecodingSpeed.Fast:
                    {
                        handler.BKDView.Config.DecoderConfig.DecodingSpeed = AndroidDecodingSpeed.Fast;
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
                        handler.BKDView.Config.DecoderConfig.FormattingType = AndroidFormattingType.Automatic;
                        break;
                    }
                case CommonFormattingType.Disabled:
                    {
                        handler.BKDView.Config.DecoderConfig.FormattingType = AndroidFormattingType.Disabled;
                        break;
                    }
                case CommonFormattingType.AAMVA:
                    {
                        handler.BKDView.Config.DecoderConfig.FormattingType = AndroidFormattingType.Aamva;
                        break;
                    }
                case CommonFormattingType.GS1:
                    {
                        handler.BKDView.Config.DecoderConfig.FormattingType = AndroidFormattingType.Gs1;
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
                        handler.BKDView.Config.DecoderConfig.Code39.ChecksumType = AndroidCode39ChecksumType.Disabled;
                        break;
                    }
                case CommonCode39ChecksumType.Enabled:
                    {
                        handler.BKDView.Config.DecoderConfig.Code39.ChecksumType = AndroidCode39ChecksumType.Enabled;
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
                        handler.BKDView.Config.DecoderConfig.Code11.ChecksumType = AndroidCode11ChecksumType.Disabled;
                        break;
                    }
                case CommonCode11ChecksumType.Double:
                    {
                        handler.BKDView.Config.DecoderConfig.Code11.ChecksumType = AndroidCode11ChecksumType.Double;
                        break;
                    }
                case CommonCode11ChecksumType.Single:
                    {
                        handler.BKDView.Config.DecoderConfig.Code11.ChecksumType = AndroidCode11ChecksumType.Single;
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
                        handler.BKDView.Config.DecoderConfig.Msi.ChecksumType = AndroidMsiChecksumType.Disabled;
                        break;
                    }
                case CommonMsiChecksumType.Mod10:
                    {
                        handler.BKDView.Config.DecoderConfig.Msi.ChecksumType = AndroidMsiChecksumType.Mod10;
                        break;
                    }
                case CommonMsiChecksumType.Mod1010:
                    {
                        handler.BKDView.Config.DecoderConfig.Msi.ChecksumType = AndroidMsiChecksumType.Mod1010;
                        break;
                    }
                case CommonMsiChecksumType.Mod11:
                    {
                        handler.BKDView.Config.DecoderConfig.Msi.ChecksumType = AndroidMsiChecksumType.Mod11;
                        break;
                    }
                case CommonMsiChecksumType.Mod1110:
                    {
                        handler.BKDView.Config.DecoderConfig.Msi.ChecksumType = AndroidMsiChecksumType.Mod1110;
                        break;
                    }
                case CommonMsiChecksumType.Mod1110IBM:
                    {
                        handler.BKDView.Config.DecoderConfig.Msi.ChecksumType = AndroidMsiChecksumType.Mod1110IBM;
                        break;
                    }
                case CommonMsiChecksumType.Mod11IBM:
                    {
                        handler.BKDView.Config.DecoderConfig.Msi.ChecksumType = AndroidMsiChecksumType.Mod11IBM;
                        break;
                    }
            }
        }
    }

    private static void MapSetEncodingCharacterSet(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string encodingCharacterSet) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.EncodingCharacterSet = encodingCharacterSet;
        }
    }

    private static void MapSetIdDocumentMasterChecksumEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            if(enabled)
            {
                handler.BKDView.Config.DecoderConfig.IDDocument.MasterChecksumType = Com.Barkoder.Barkoder.StandardChecksumType.Enabled;
            } else
            {
                handler.BKDView.Config.DecoderConfig.IDDocument.MasterChecksumType = Com.Barkoder.Barkoder.StandardChecksumType.Disabled;
            }
         
            view.IsIdDocumentMasterChecksumEnabled = enabled;
        }
    }

    private static void MapSetDatamatrixDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.Datamatrix.DpmMode = enabled;
            view.IsDatamatrixDpmModeEnabled = enabled;

        }
    }

    private static void MapSetQRDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.Qr.DpmMode = enabled;
            view.IsQRDpmModeEnabled = enabled;


        }
    }

    private static void MapSetQRMicroDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.QRMicro.DpmMode = enabled;
           
            view.IsQRMicroDpmModeEnabled = enabled;

        }
    }


    private static void MapSetUpcEanDeblurEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.UpcEanDeblur = enabled;
        }
    }

    private static void MapSetEnableMisshaped1DEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.EnableMisshaped1D = enabled;
        }
    }

    private static void MapSetBarcodeThumbnailOnResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.SetThumbnailOnResultEnabled(enabled);
        }
    }

    private static void MapSetMaximumResultsCount(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int maximumResultsCount) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.MaximumResultsCount = maximumResultsCount;
        }
    }

    private static void MapSetDuplicatesDelayMs(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int duplicatesDelayMs) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.DuplicatesDelayMs = duplicatesDelayMs;
        }
    }

    private static void MapSetMulticodeCachingDuration(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int multicodeCachingDuration) && (handler.BKDView != null))
        {
            // TODO: - Missing get method for android
        }
    }

    private static void MapSetBarcodeTypeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is BarcodeTypeEventArgs barcodeTypeEventArgs) && (handler.BKDView != null))
        {
            switch (barcodeTypeEventArgs.BarcodeType)
            {
                case Enums.BarcodeType.Aztec:
                    handler.BKDView.Config.DecoderConfig.Aztec.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.AztecCompact:
                    handler.BKDView.Config.DecoderConfig.AztecCompact.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.QR:
                    handler.BKDView.Config.DecoderConfig.Qr.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.QRMicro:
                    handler.BKDView.Config.DecoderConfig.QRMicro.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Code128:
                    handler.BKDView.Config.DecoderConfig.Code128.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Code93:
                    handler.BKDView.Config.DecoderConfig.Code93.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Code39:
                    handler.BKDView.Config.DecoderConfig.Code39.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Codabar:
                    handler.BKDView.Config.DecoderConfig.Codabar.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Code11:
                    handler.BKDView.Config.DecoderConfig.Code11.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Msi:
                    handler.BKDView.Config.DecoderConfig.Msi.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.UpcA:
                    handler.BKDView.Config.DecoderConfig.UpcA.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.UpcE:
                    handler.BKDView.Config.DecoderConfig.UpcE.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.UpcE1:
                    handler.BKDView.Config.DecoderConfig.UpcE1.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Ean13:
                    handler.BKDView.Config.DecoderConfig.Ean13.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Ean8:
                    handler.BKDView.Config.DecoderConfig.Ean8.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.PDF417:
                    handler.BKDView.Config.DecoderConfig.Pdf417.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.PDF417Micro:
                    handler.BKDView.Config.DecoderConfig.PDF417Micro.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Datamatrix:
                    handler.BKDView.Config.DecoderConfig.Datamatrix.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Code25:
                    handler.BKDView.Config.DecoderConfig.Code25.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Interleaved25:
                    handler.BKDView.Config.DecoderConfig.Interleaved25.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Itf14:
                    handler.BKDView.Config.DecoderConfig.Itf14.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Matrix25:
                    handler.BKDView.Config.DecoderConfig.Matrix25.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Datalogic25:
                    handler.BKDView.Config.DecoderConfig.Datalogic25.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Coop25:
                    handler.BKDView.Config.DecoderConfig.Coop25.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Code32:
                    handler.BKDView.Config.DecoderConfig.Code32.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Telepen:
                    handler.BKDView.Config.DecoderConfig.Telepen.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Dotcode:
                    handler.BKDView.Config.DecoderConfig.Dotcode.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.IDDocument:
                    handler.BKDView.Config.DecoderConfig.IDDocument.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
            }
        }
    }

    private static void MapSetEnableVINRestrictions(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.EnableVINRestrictions = enabled;
        }
    }

    private static void MapSetThresholdBetweenDuplicatesScans(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int thresholdBetweenDuplicatesScans) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ThresholdBetweenDuplicatesScans = thresholdBetweenDuplicatesScans;
        }
    }

    private static void MapSetBarcodeTypeLengthRange(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is BarcodeRangeEventArg barcodeRangeEventArg) && (handler.BKDView != null))
        {
            switch (barcodeRangeEventArg.BarcodeType)
            {
                case Enums.BarcodeType.Code128:
                    {
                        handler.BKDView.Config.DecoderConfig.Code128.SetLengthRange(barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case Enums.BarcodeType.Code93:
                    {
                        handler.BKDView.Config.DecoderConfig.Code93.SetLengthRange(barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case Enums.BarcodeType.Code39:
                    {
                        handler.BKDView.Config.DecoderConfig.Code39.SetLengthRange(barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case Enums.BarcodeType.Codabar:
                    {
                        handler.BKDView.Config.DecoderConfig.Codabar.SetLengthRange(barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case Enums.BarcodeType.Code11:
                    {
                        handler.BKDView.Config.DecoderConfig.Code11.SetLengthRange(barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                case Enums.BarcodeType.Msi:
                    {
                        handler.BKDView.Config.DecoderConfig.Msi.SetLengthRange(barcodeRangeEventArg.Min, barcodeRangeEventArg.Max);
                        break;
                    }
                default:
                    break;

            }
        }
    }

    private static void MapConfigureBarkoder(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string configAsJsonString) && (handler.BKDView != null))
        {
            dynamic configAsJson = new JSONObject(configAsJsonString);

            BarkoderHelper.ApplyJsonToConfig(handler.BKDView.Config, configAsJson);
        }
    }

}

public class Util
{
    public static int HexColorToIntColor(string hexColor)
    {
        int color;

        if (hexColor.StartsWith("#"))
            color = (int)ColorTranslator.FromHtml(hexColor).ToArgb();
        else
            color = (int)ColorTranslator.FromHtml("#" + hexColor).ToArgb();

        return color;
    }

    public static string IntColorToHexColor(int intColor)
    {
        // Convert the integer color value to a Color object
        System.Drawing.Color color = System.Drawing.Color.FromArgb(intColor);
        // Convert the Color object to a hexadecimal color string with "#" on start
        string hexColor = "#" + ColorTranslator.ToHtml(color).Substring(1);
        return hexColor;
    }
}

public class AndroidBarkoderView : AppCompatActivity, Com.Barkoder.Interfaces.IBarkoderResultCallback, Com.Barkoder.Interfaces.IFlashAvailableCallback, Com.Barkoder.Interfaces.IMaxZoomAvailableCallback
{
    IBarkoderDelegate? BarkoderDelegate;
    Com.Barkoder.BarkoderView BkdView;
    Action<bool>? flashAvailableCompletion;
    Action<float>? maxZoomFactorCompletion;

    public AndroidBarkoderView(Com.Barkoder.BarkoderView bkdView)
    {
        BkdView = bkdView;
    }

    public AndroidBarkoderView(IBarkoderDelegate barkoderDelegate, Com.Barkoder.BarkoderView bkdView)
    {
        BarkoderDelegate = barkoderDelegate;
        BkdView = bkdView;
    }

    public void ScanImage(Bitmap bitmap, BarkoderConfig config, Context context)
    {
        BarkoderHelper.ScanImage(bitmap, config, this, context);
    }

    public void StartScanning()
    {
        BkdView.StartScanning(this);
    }

    public void ScanningFinished(Com.Barkoder.Barkoder.Result[] result, Bitmap[] bitmaps, Bitmap originalImage)
    {
        BarcodeResult[] barcodeResults = new BarcodeResult[result.Length];

        for (int i = 0; i < result.Length; i++)
        {
            List<ImageData> mrzImages = new List<ImageData>(); // Create a new list for each result

            var res = result[i];

            if (res.Images != null)
            {
                foreach (var image in res.Images)
                {
                    if (image.Image != null)
                    {
                        ImageSource? imageSource = BitmapToImageSource(image.Image);

                        if (imageSource != null)
                        {
                            mrzImages.Add(new ImageData( image.Name, imageSource));
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Failed to convert Bitmap to ImageSource for image name: {image.Name}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Image is null for name: {image.Name}");
                    }
                }
            }

            // Create BarcodeResult for each scanned result
            BarcodeResult barcodeResult = new BarcodeResult(
                res.TextualData,
                res.BarcodeTypeName,
                "", // CharacterSet
                mrzImages
            );

            barcodeResults[i] = barcodeResult;
        }

        // Delegate call with the results
        BarkoderDelegate?.DidFinishScanning(barcodeResults, BitmapToImageSource(originalImage));
    }

    public void isFlashAvailable(Action<bool> completion)
    {
        flashAvailableCompletion = completion;
        BkdView.IsFlashAvailable(this);
    }

    public void getMaxZoomFactor(Action<float> completion)
    {
        maxZoomFactorCompletion = completion;
        BkdView.GetMaxZoomFactor(this);
    }

    public void OnFlashAvailable(bool flashAvailable)
    {
        if (flashAvailableCompletion != null)
        {
            flashAvailableCompletion(flashAvailable);
        }
    }

    public void OnMaxZoomAvailable(float maxZoomFactor)
    {
        if (maxZoomFactorCompletion != null)
        {
            maxZoomFactorCompletion(maxZoomFactor);
        }
    }

    private static ImageSource BitmapToImageSource(Bitmap bitmap)
    {
        if (bitmap == null || Bitmap.CompressFormat.Jpeg == null)
        {
            return ImageSource.FromStream(() => new MemoryStream());
        }

        // Converting from Bitmap to Base64 string
        MemoryStream stream = new MemoryStream();
        bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
        byte[] bytes = stream.ToArray();
        string ImageInBase64 = Convert.ToBase64String(bytes);

        // Converting from Base64 string to ImageSource
        byte[] imageBytes = Convert.FromBase64String(ImageInBase64);
        MemoryStream imageSourceStream = new MemoryStream(imageBytes);
        ImageSource imageSource = ImageSource.FromStream(() => imageSourceStream);

        return imageSource;
    }



}



#endif
