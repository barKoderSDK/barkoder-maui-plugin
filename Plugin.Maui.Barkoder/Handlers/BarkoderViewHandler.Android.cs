#if ANDROID
using Microsoft.Maui.Handlers;
using Com.Barkoder;
using Android.Content;
using Android.Graphics;
using System.Drawing;
using System.Text.Json;
using Plugin.Maui.Barkoder.Handlers;
using AndroidX.ExifInterface.Media;
using Android.Media;

using CommonBarkoderResolution = Plugin.Maui.Barkoder.Enums.BarkoderResolution;
using AndroidBarkoderResolution = Com.Barkoder.Enums.BarkoderResolution;
using CommonARMode = Plugin.Maui.Barkoder.Enums.BarkoderARMode;
using CommonARHeaderShowMode = Plugin.Maui.Barkoder.Enums.BarkoderARHeaderShowMode;
using CommonARLocationType = Plugin.Maui.Barkoder.Enums.BarkoderARLocationType;
using CommonAROverlayRefresh = Plugin.Maui.Barkoder.Enums.BarkoderAROverlayRefresh;
using AndroidARMode = Com.Barkoder.Enums.BarkoderARMode;
using AndroidARHeaderShowMode = Com.Barkoder.Enums.BarkoderARHeaderShowMode;
using AndroidARLocationType = Com.Barkoder.Enums.BarkoderARLocationType;

using CommonBarkoderCameraPosition = Plugin.Maui.Barkoder.Enums.BarkoderCameraPosition;
using AndroidBarkoderCameraPosition = Com.Barkoder.Enums.BarkoderCameraPosition;

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
using static Com.Barkoder.Barkoder;
using static Android.Icu.Text.ListFormatter;
using Plugin.Maui.Barkoder.Handlers;
using Java.Util;

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

    public float GetCurrentZoomFactor()
    {
        return BKDView?.CurrentZoomFactor ?? -1f;
    }

    private static void MapLicenseKey(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            handler.BKDView.Config = new Com.Barkoder.BarkoderConfig(handler.BKDView.Context, view.LicenseKey, null);


            IDictionary<string, string> licenseInfo = Com.Barkoder.Barkoder.LicenseInfo;

            if (licenseInfo != null)
            {
                foreach (var kvp in licenseInfo)
                {
                    System.Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
        }
    }

    private static void MapIsFlashAvailable(BarkoderViewHandler handler, BarkoderView view)
    {
        //if (handler.BKDView != null)
        //{
        //    AndroidBarkoderView AndroidBarkoderView = new AndroidBarkoderView(handler.BKDView);

        //    AndroidBarkoderView.isFlashAvailable((flashAvailable) =>
        //    {
        //        view.IsFlashAvailable = flashAvailable;
        //    });
        //}
    }

    private static void MapMaxZoomFactor(BarkoderViewHandler handler, BarkoderView view)
    {
        //if (handler.BKDView != null)
        //{
        //    AndroidBarkoderView AndroidBarkoderView = new AndroidBarkoderView(handler.BKDView);

        //    AndroidBarkoderView.getMaxZoomFactor((maxZoomFactor) =>
        //    {
        //        view.MaxZoomFactor = maxZoomFactor;
        //    });
        //}
    }

    private static void MapInitCameraProperties(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (handler.BKDView != null)
        {
            AndroidBarkoderView AndroidBarkoderView = new AndroidBarkoderView(handler.BKDView);

            AndroidBarkoderView.isFlashAvailable((flashAvailable) =>
            {
                view.IsFlashAvailable = flashAvailable;
            });

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

    private static void MapARNonSelectedLocationLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARNonSelectedLocationLineColorHex = Util.IntColorToHexColor(handler.BKDView.Config.ArConfig.NonSelectedLocationColor);
        }
    }

    private static void MapARImageResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARImageResultEnabled = handler.BKDView.Config.ArConfig.ImageResultEnabled; ;
        }
    }

    private static void MapARBarcodeThumbnailOnResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARBarcodeThumbnailOnResultEnabled = handler.BKDView.Config.ArConfig.BarcodeThumbnailOnResult; 
        }
    }


    private static void MapARSelectedLocationLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARSelectedLocationLineColorHex = Util.IntColorToHexColor(handler.BKDView.Config.ArConfig.SelectedLocationColor);
        }
    }

    private static void MapARSelectedHeaderTextColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARSelectedHeaderTextColorHex = Util.IntColorToHexColor(handler.BKDView.Config.ArConfig.HeaderTextColorSelected);
        }
    }
    private static void MapARNonSelectedHeaderTextColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARNonSelectedHeaderTextColorHex = Util.IntColorToHexColor(handler.BKDView.Config.ArConfig.HeaderTextColorNonSelected);
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

    private static void MapARHeaderTextFormat(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARHeaderTextFormat = handler.BKDView.Config.ArConfig.HeaderTextFormat;
        }
    }

    private static void MapLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.LocationLineWidth = handler.BKDView.Config.LocationLineWidth;
        }
    }

    private static void MapARSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARSelectedLocationLineWidth = handler.BKDView.Config.ArConfig.SelectedLocationLineWidth * 1.1f;
        }
    }

    private static void MapARNonSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARNonSelectedLocationLineWidth = handler.BKDView.Config.ArConfig.NonSelectedLocationLineWidth * 1.1f;
        }
    }

    private static void MapARLocationTransitionSpeed(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARLocationTransitionSpeed = handler.BKDView.Config.ArConfig.LocationTransitionSpeed;
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

    private static void MapARContinueScanningOnLimit(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.ARContinueScanningOnLimit = handler.BKDView.Config.ArConfig.ContinueScanningOnLimit;
        }
    }

    private static void MapAREmitResultsAtSessionEndOnly(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.AREmitResultsAtSessionEndOnly = handler.BKDView.Config.ArConfig.EmitResultsAtSessionEndOnly;
        }
    }

    private static void MapVersion(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.Version = Com.Barkoder.Barkoder.Version;

        }
    }

    private static void MapLibVersion(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler.BKDView != null)
        {
            view.LibVersion = Com.Barkoder.Barkoder.LibVersion;

        }
    }

    public static void MapSetCustomOption(BarkoderViewHandler handler, BarkoderView view, object? argument)
    {
        if (handler.BKDView != null && argument is ValueTuple<string, int> customOption)
        {
            var (optionName, optionValue) = customOption; // Deconstruct after casting
            Com.Barkoder.Barkoder.SetCustomOption(handler.BKDView.Config.DecoderConfig, optionName, optionValue);
        }

    }



    private static void MapBarkoderResolution(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidBarkoderResolution barkoderResolution = handler.BKDView.Config.BarkoderResolution;

            // Converting from android BarkoderResolution to CommonBarkoderResolution
            if (barkoderResolution == AndroidBarkoderResolution.Hd)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.HD;
            }
            else if (barkoderResolution == AndroidBarkoderResolution.Fhd)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.FHD;
            }
            else if (barkoderResolution == AndroidBarkoderResolution.Uhd)
            {
                view.BarkoderResolution = Enums.BarkoderResolution.UHD;
            }
        }
    }

    private static void MapBarkoderARMode(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidARMode barkoderARMode = handler.BKDView.Config.ArConfig.ARMode;


            if (barkoderARMode == AndroidARMode.Off)
            {
                view.BarkoderARMode = Enums.BarkoderARMode.OFF;
            }
            else if (barkoderARMode == AndroidARMode.InteractiveDisabled)
            {
                view.BarkoderARMode = Enums.BarkoderARMode.InteractiveDisabled;
            }
            else if (barkoderARMode == AndroidARMode.InteractiveEnabled)
            {
                view.BarkoderARMode = Enums.BarkoderARMode.InteractiveEnabled;
            }
            else if (barkoderARMode == AndroidARMode.NonInteractive)
            {
                view.BarkoderARMode = Enums.BarkoderARMode.NonInteractive;
            }
        }
    }

    private static void MapBarkoderARHeaderShowMode(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidARHeaderShowMode barkoderARHeaderShowMode = handler.BKDView.Config.ArConfig.HeaderShowMode;


            if (barkoderARHeaderShowMode == AndroidARHeaderShowMode.Never)
            {
                view.BarkoderARHeaderShowMode = Enums.BarkoderARHeaderShowMode.NEVER;
            }
            else if (barkoderARHeaderShowMode == AndroidARHeaderShowMode.Onselected)
            {
                view.BarkoderARHeaderShowMode = Enums.BarkoderARHeaderShowMode.ONSELECTED;
            }
            else if (barkoderARHeaderShowMode == AndroidARHeaderShowMode.Always)
            {
                view.BarkoderARHeaderShowMode = Enums.BarkoderARHeaderShowMode.ALWAYS;
            }

        }
    }

    private static void MapBarkoderARLocationType(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            AndroidARLocationType barkoderARLocationType = handler.BKDView.Config.ArConfig.LocationType;


            if (barkoderARLocationType == AndroidARLocationType.None)
            {
                view.BarkoderARLocationType = Enums.BarkoderARLocationType.NONE;
            }
            else if (barkoderARLocationType == AndroidARLocationType.Tight)
            {
                view.BarkoderARLocationType = Enums.BarkoderARLocationType.TIGHT;
            }
            else if (barkoderARLocationType == AndroidARLocationType.Boundingbox)
            {
                view.BarkoderARLocationType = Enums.BarkoderARLocationType.BOUNDINGBOX;
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
            else if (decodingSpeed == AndroidDecodingSpeed.Rigorous)
            {
                view.DecodingSpeed = CommonDecodingSpeed.Rigorous;
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
            else if (formattingType == AndroidFormattingType.Sadl)
            {
                view.FormattingType = CommonFormattingType.SADL;
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

    private static void MapARResultLimit(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ARResultLimit = handler.BKDView.Config.ArConfig.ResultLimit;
        }
    }

    private static void MapResultDisappearanceDelayMs(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ResultDisappearanceDelayMs = handler.BKDView.Config.ArConfig.ResultDisappearanceDelayMs;
        }
    }

    private static void MapARHeaderVerticalTextMargin(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.HeaderARVerticalTextMargin = handler.BKDView.Config.ArConfig.HeaderVerticalTextMargin;
        }
    }

    private static void MapARHeaderHorizontalTextMargin(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.HeaderARHorizontalTextMargin = handler.BKDView.Config.ArConfig.HeaderHorizontalTextMargin;
        }
    }

    private static void MapARHeaderHeight(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ARHeaderHeight = handler.BKDView.Config.ArConfig.HeaderHeight;
        }
    }

    private static void MapARHeaderMaxTextHeight(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ARHeaderMaxTextHeight = handler.BKDView.Config.ArConfig.HeaderMaxTextHeight;
        }
    }

    private static void MapARHeaderMinTextHeight(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ARHeaderMinTextHeight = handler.BKDView.Config.ArConfig.HeaderMinTextHeight;
        }
    }

    private static void MapDuplicatesDelayMs(BarkoderViewHandler handler, BarkoderView view)
    {
        //if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        //{
        //    view.DuplicatesDelayMs = handler.BKDView.Config.DecoderConfig.DuplicatesDelayMs;
        //}
    }

    private static void MapScanningIndicatorAlwaysVisibleEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ScanningIndicatorAlwaysVisibleEnabled = handler.BKDView.Config.ScanningIndicatorAlwaysVisible;
        }
    }

    private static void MapScanningIndicatorAnimationMode(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ScanningIndicatorAnimationMode = handler.BKDView.Config.ScanningIndicatorAnimation;
        }

    }


    

    private static void MapScanningIndicatorColor(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ScanningIndicatorLineColorHex = Util.IntColorToHexColor(handler.BKDView.Config.ScanningIndicatorColor);
        }
    }

    private static void MapScanningIndicatorWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ScanningIndicatorLineWidth = handler.BKDView.Config.ScanningIndicatorWidth;
        
    }
   }


    private static void MapEnableComposite(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.EnableComposite = handler.BKDView.Config.DecoderConfig.EnableComposite;
        }

    }

    private static void MapMulticodeCachingDuration(BarkoderViewHandler handler, BarkoderView view)
    {
        //if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        //{
        //    view.DuplicatesDelayMs = Com.Barkoder.
        //        Barkoder.Config.GlobalOptionMulticodeCachingDuration;
        //}
    }

    private static void MapVINRestrictionsEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.VINRestrictionsEnabled = handler.BKDView.Config.DecoderConfig.EnableVINRestrictions;
        }
    }

    private static void MapARDoubleTapToFreezeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if ((handler.BKDView != null) && (handler.BKDView.Config != null))
        {
            view.ARDoubleTapToFreezeEnabled = handler.BKDView.Config.ArConfig.DoubleTapToFreezeEnabled;
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

            bitmap = ApplyExifRotationIfNeeded(bitmap, imageBytes);
            Context context = handler.Context;

            AndroidBarkoderView AndroidBarkoderView = new AndroidBarkoderView(barkoderDelegate, handler.BKDView);
            AndroidBarkoderView.ScanImage(bitmap,handler.BKDView.Config, context);

        }
        else
        {
      
        }
    }

    private static Bitmap RotateBitmap(Bitmap bitmap, float degrees)
    {
        Matrix matrix = new Matrix();
        matrix.PostRotate(degrees);
        Bitmap rotated = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
        bitmap.Recycle(); // free original bitmap
        return rotated;
    }

    private static Bitmap ApplyExifRotationIfNeeded(Bitmap bitmap, byte[] imageBytes)
    {
        try
        {
            using var exifStream = new MemoryStream(imageBytes);
            var exif = new AndroidX.ExifInterface.Media.ExifInterface(exifStream);

            int orientation = exif.GetAttributeInt(AndroidX.ExifInterface.Media.ExifInterface.TagOrientation, (int)Orientation.Normal);

            return orientation switch
            {
                (int)Orientation.Rotate90 => RotateBitmap(bitmap, 90),
                (int)Orientation.Rotate180 => RotateBitmap(bitmap, 180),
                (int)Orientation.Rotate270 => RotateBitmap(bitmap, 270),
                _ => bitmap, // Normal orientation, no rotation
            };
        }
        catch
        {
            // No EXIF or cannot read → return bitmap as-is
            return bitmap;
        }
    }

  

    private static void MapStopScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.BKDView?.StopScanning();
    }

    private static void MapSelectVisibleBarcodes(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.BKDView?.SelectVisibleBarcodes();
    }

    private static void MapConfigureCloseButton(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (handler?.BKDView == null || arg3 is not object parameters)
            return;

        var type = parameters.GetType();

        bool visible = Convert.ToBoolean(type.GetProperty("visible")?.GetValue(parameters));
        var positionObj = type.GetProperty("position")?.GetValue(parameters);
        float[] position;

        // 1. **CRITICAL:** Check for the native float[] type first.
        if (positionObj is float[] floatArray)
        {
            position = floatArray;
        }
        // 2. Fallback check: Handle generic collection (like List<float> or List<object>)
        else if (positionObj is IEnumerable<object> positionEnumerable)
        {
            // This handles collections where elements are boxed as 'object'
            position = positionEnumerable.Select(x => Convert.ToSingle(x)).ToArray();
        }
        // 3. Last resort: Fallback to empty array.
        else
        {
            position = Array.Empty<float>();
        }

        float iconSize = Convert.ToSingle(type.GetProperty("iconSize")?.GetValue(parameters));

        string? tintColorHex = type.GetProperty("tintColor")?.GetValue(parameters) as string;
        string? backgroundColorHex = type.GetProperty("backgroundColor")?.GetValue(parameters) as string;

        float cornerRadius = Convert.ToSingle(type.GetProperty("cornerRadius")?.GetValue(parameters));
        float padding = Convert.ToSingle(type.GetProperty("padding")?.GetValue(parameters));
        bool useCustomIcon = Convert.ToBoolean(type.GetProperty("useCustomIcon")?.GetValue(parameters));

        string? base64CustomIcon = type.GetProperty("customIconBase64")?.GetValue(parameters) as string;

        Action? onClose = type.GetProperty("onClose")?.GetValue(parameters) as Action;

        bool HasColor(string? hex) => !string.IsNullOrWhiteSpace(hex);

        Java.Lang.Integer? tintColorJavaInt = null;
        if (HasColor(tintColorHex))
        {
            // If the string is valid, convert it and create the Java object
            var tintColorInt = Util.HexColorToIntColor(tintColorHex);
            tintColorJavaInt = new Java.Lang.Integer(tintColorInt);
        }

        Java.Lang.Integer? backgroundColorJavaInt = null;
        if (HasColor(backgroundColorHex))
        {
            // If the string is valid, convert it and create the Java object
            var backgroundColorInt = Util.HexColorToIntColor(backgroundColorHex);
            backgroundColorJavaInt = new Java.Lang.Integer(backgroundColorInt);
        }


        Android.Graphics.Bitmap? customIconBitmap = DecodeBase64ToBitmap(base64CustomIcon);

        handler.BKDView.ConfigureCloseButton(
           visible,
     position,             // Center position
         iconSize,
          tintColorJavaInt,
          backgroundColorJavaInt,
          cornerRadius,
          padding,
          useCustomIcon,
          customIconBitmap,
          new Java.Lang.Runnable(() => onClose?.Invoke())
      );


    }










    private static void MapConfigureFlashButton(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (handler?.BKDView == null || arg3 is not object parameters)
            return;

        var type = parameters.GetType();

        bool visible = Convert.ToBoolean(type.GetProperty("visible")?.GetValue(parameters));

        // Parse position array safely
        var positionObj = type.GetProperty("position")?.GetValue(parameters);
        float[] position;

        // 1. **CRITICAL:** Check for the native float[] type first.
        if (positionObj is float[] floatArray)
        {
            position = floatArray;
        }
        // 2. Fallback check: Handle generic collection (like List<float> or List<object>)
        else if (positionObj is IEnumerable<object> positionEnumerable)
        {
            // This handles collections where elements are boxed as 'object'
            position = positionEnumerable.Select(x => Convert.ToSingle(x)).ToArray();
        }
        // 3. Last resort: Fallback to empty array.
        else
        {
            position = Array.Empty<float>();
        }

        float iconSize = Convert.ToSingle(type.GetProperty("iconSize")?.GetValue(parameters));
        string? tintColorHex = type.GetProperty("tintColor")?.GetValue(parameters) as string;
        string? backgroundColorHex = type.GetProperty("backgroundColor")?.GetValue(parameters) as string;

        float cornerRadius = Convert.ToSingle(type.GetProperty("cornerRadius")?.GetValue(parameters));
        float padding = Convert.ToSingle(type.GetProperty("padding")?.GetValue(parameters));
        bool useCustomIcon = Convert.ToBoolean(type.GetProperty("useCustomIcon")?.GetValue(parameters));

        string? base64FlashOn = type.GetProperty("customIconFlashOnBase64")?.GetValue(parameters) as string;
        string? base64FlashOff = type.GetProperty("customIconFlashOffBase64")?.GetValue(parameters) as string;

        // Convert colors from hex to Android.Graphics.Color, then to Java.Lang.Integer or null
        bool HasColor(string? hex) => !string.IsNullOrWhiteSpace(hex);

        Java.Lang.Integer? tintColorJavaInt = null;
        if (HasColor(tintColorHex))
        {
            // If the string is valid, convert it and create the Java object
            var tintColorInt = Util.HexColorToIntColor(tintColorHex);
            tintColorJavaInt = new Java.Lang.Integer(tintColorInt);
        }

        Java.Lang.Integer? backgroundColorJavaInt = null;
        if (HasColor(backgroundColorHex))
        {
            // If the string is valid, convert it and create the Java object
            var backgroundColorInt = Util.HexColorToIntColor(backgroundColorHex);
            backgroundColorJavaInt = new Java.Lang.Integer(backgroundColorInt);
        }



        var customIconFlashOn = DecodeBase64ToBitmap(base64FlashOn);
        var customIconFlashOff = DecodeBase64ToBitmap(base64FlashOff);

    

        handler.BKDView.ConfigureFlashButton(
              visible,
          position,
            iconSize,
            tintColorJavaInt,
            backgroundColorJavaInt,
            cornerRadius,
            padding,
            useCustomIcon,
            customIconFlashOn,
            customIconFlashOff
        );
    }



    private static void MapConfigureZoomButton(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (handler?.BKDView == null || arg3 is not object parameters)
            return;

        var type = parameters.GetType();

        bool visible = Convert.ToBoolean(type.GetProperty("visible")?.GetValue(parameters));

        var positionObj = type.GetProperty("position")?.GetValue(parameters);
        float[] position;

        // 1. **CRITICAL:** Check for the native float[] type first.
        if (positionObj is float[] floatArray)
        {
            position = floatArray;
        }
        // 2. Fallback check: Handle generic collection (like List<float> or List<object>)
        else if (positionObj is IEnumerable<object> positionEnumerable)
        {
            // This handles collections where elements are boxed as 'object'
            position = positionEnumerable.Select(x => Convert.ToSingle(x)).ToArray();
        }
        // 3. Last resort: Fallback to empty array.
        else
        {
            position = Array.Empty<float>();
        }

        float iconSize = Convert.ToSingle(type.GetProperty("iconSize")?.GetValue(parameters));
        string? tintColorHex = type.GetProperty("tintColor")?.GetValue(parameters) as string;
        string? backgroundColorHex = type.GetProperty("backgroundColor")?.GetValue(parameters) as string;

        float cornerRadius = Convert.ToSingle(type.GetProperty("cornerRadius")?.GetValue(parameters));
        float padding = Convert.ToSingle(type.GetProperty("padding")?.GetValue(parameters));
        bool useCustomIcon = Convert.ToBoolean(type.GetProperty("useCustomIcon")?.GetValue(parameters));

        string? base64ZoomIn = type.GetProperty("customIconZoomedInBase64")?.GetValue(parameters) as string;
        string? base64ZoomOut = type.GetProperty("customIconZoomedOutBase64")?.GetValue(parameters) as string;

        float zoomedInFactor = Convert.ToSingle(type.GetProperty("zoomedInFactor")?.GetValue(parameters));
        float zoomedOutFactor = Convert.ToSingle(type.GetProperty("zoomedOutFactor")?.GetValue(parameters));

       
        var customIconZoomIn = DecodeBase64ToBitmap(base64ZoomIn);
        var customIconZoomOut = DecodeBase64ToBitmap(base64ZoomOut);


        bool HasColor(string? hex) => !string.IsNullOrWhiteSpace(hex);

        Java.Lang.Integer? tintColorJavaInt = null;
        if (HasColor(tintColorHex))
        {
            // If the string is valid, convert it and create the Java object
            var tintColorInt = Util.HexColorToIntColor(tintColorHex);
            tintColorJavaInt = new Java.Lang.Integer(tintColorInt);
        }

        Java.Lang.Integer? backgroundColorJavaInt = null;
        if (HasColor(backgroundColorHex))
        {
            // If the string is valid, convert it and create the Java object
            var backgroundColorInt = Util.HexColorToIntColor(backgroundColorHex);
            backgroundColorJavaInt = new Java.Lang.Integer(backgroundColorInt);
        }

        handler.BKDView.ConfigureZoomButton(
            true,
            position,
            iconSize,
            tintColorJavaInt,
            backgroundColorJavaInt,
            cornerRadius,
            padding,
            useCustomIcon,
            customIconZoomIn,
            customIconZoomOut,
            zoomedInFactor,
            zoomedOutFactor
        );
    }


    public static int HexColorToIntColor2(string? hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
            return unchecked((int)0xFFFFFFFF); // default white or transparent

        // Normalize hex (e.g. #RRGGBB or #AARRGGBB)
        hex = hex.TrimStart('#');

        uint colorValue;
        if (uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out colorValue))
        {
            // If hex was RRGGBB (6 chars), add opaque alpha FF
            if (hex.Length == 6)
            {
                colorValue |= 0xFF000000;
            }
            // Cast to int with unchecked to allow overflow
            return unchecked((int)colorValue);
        }

        return unchecked((int)0xFFFFFFFF); // fallback color
    }



    private static void MapCaptureImage(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.BKDView?.CaptureImage();
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


    private static Android.Graphics.Color? ParseHexColor(string? hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
            return null;

        try
        {
            // Ensure it starts with "#" and has 6 or 8 characters
            if (!hex.StartsWith("#"))
                hex = "#" + hex;

            return Android.Graphics.Color.ParseColor(hex);
        }
        catch
        {
            return null;
        }
    }


    private static Android.Graphics.Bitmap? DecodeBase64ToBitmap(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
            return null;

        try
        {
            byte[] decoded = Convert.FromBase64String(base64);
            return Android.Graphics.BitmapFactory.DecodeByteArray(decoded, 0, decoded.Length);
        }
        catch
        {
            return null;
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

    private static void MapSetARContinueScanningOnLimit(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.ContinueScanningOnLimit = enabled;
        }
    }

    private static void MapSetAREmitResultsAtSessionEndOnly(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.EmitResultsAtSessionEndOnly = enabled;
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

    private static void MapSetARNonSelectedLocationColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string hexColor) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.NonSelectedLocationColor = Util.HexColorToIntColor(hexColor);
        }
    }

    private static void MapSetARImageResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.ImageResultEnabled = enabled;
        }
    }

    private static void MapSetARBarcodeThumbnailOnResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.BarcodeThumbnailOnResult = enabled;
        }
    }

    private static void MapSetARSelectedLocationColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string hexColor) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.SelectedLocationColor = Util.HexColorToIntColor(hexColor);
        }
    }

    private static void MapSetARSelectedHeaderTextColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string hexColor) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.HeaderTextColorSelected = Util.HexColorToIntColor(hexColor);
        }
    }

    private static void MapSetARNonSelectedHeaderTextColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string hexColor) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.HeaderTextColorNonSelected = Util.HexColorToIntColor(hexColor);
        }
    }

    private static void MapSetARSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double width) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.SelectedLocationLineWidth = (float)width *1.1f;
        }
    }

    private static void MapSetARNonSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double width) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.NonSelectedLocationLineWidth = (float)width * 1.1f;
        }
    }

    private static void MapSetARLocationTransitionSpeed(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double speed) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.LocationTransitionSpeed = (float)speed;
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
                case CommonBarkoderResolution.FHD:
                    {
                        handler.BKDView.Config.BarkoderResolution = AndroidBarkoderResolution.Fhd;
                        break;
                    }
                case CommonBarkoderResolution.HD:
                    {
                        handler.BKDView.Config.BarkoderResolution = AndroidBarkoderResolution.Hd;
                        break;
                    }
                case CommonBarkoderResolution.UHD:
                    {
                        handler.BKDView.Config.BarkoderResolution = AndroidBarkoderResolution.Hd;
                        break;
                    }
            }
        }
    }

    private static void MapSetBarkoderARMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonARMode barkoderArMode) && (handler.BKDView != null))
        {
            switch (barkoderArMode)
            {
                case CommonARMode.OFF:
                    {
                        handler.BKDView.Config.ArConfig.ArMode = AndroidARMode.Off;
                        break;
                    }
                case CommonARMode.InteractiveDisabled:
                    {
                        handler.BKDView.Config.ArConfig.ArMode = AndroidARMode.InteractiveDisabled;
                        break;
                    }
                case CommonARMode.InteractiveEnabled:
                    {
                        handler.BKDView.Config.ArConfig.ArMode = AndroidARMode.InteractiveEnabled;
                        break;
                    }
                case CommonARMode.NonInteractive:
                    {
                        handler.BKDView.Config.ArConfig.ArMode = AndroidARMode.NonInteractive;
                        break;
                    }
            }
        }
    }

    private static void MapSetBarkoderARHeaderShowMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonARHeaderShowMode barkoderArHeadersHowMode) && (handler.BKDView != null))
        {
            switch (barkoderArHeadersHowMode)
            {
                case CommonARHeaderShowMode.NEVER:
                    {
                        handler.BKDView.Config.ArConfig.HeaderShowMode = AndroidARHeaderShowMode.Never;
                        break;
                    }
                case CommonARHeaderShowMode.ONSELECTED:
                    {
                        handler.BKDView.Config.ArConfig.HeaderShowMode = AndroidARHeaderShowMode.Onselected;
                        break;
                    }
                case CommonARHeaderShowMode.ALWAYS:
                    {
                        handler.BKDView.Config.ArConfig.HeaderShowMode = AndroidARHeaderShowMode.Always;
                        break;
                    }
            }
        }
    }

    private static void MapSetBarkoderARLocationType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonARLocationType barkoderArLocationType) && (handler.BKDView != null))
        {
            switch (barkoderArLocationType)
            {
                case CommonARLocationType.NONE:
                    {
                        handler.BKDView.Config.ArConfig.LocationType = AndroidARLocationType.None;
                        break;
                    }
                case CommonARLocationType.TIGHT:
                    {
                        handler.BKDView.Config.ArConfig.LocationType = AndroidARLocationType.Tight;
                        break;
                    }
                case CommonARLocationType.BOUNDINGBOX:
                    {
                        handler.BKDView.Config.ArConfig.LocationType = AndroidARLocationType.Boundingbox;
                        break;
                    }
            }
        }
    }

    private static void MapSetBarkoderARoverlayRefresh(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is CommonAROverlayRefresh barkoderAROverlayRefresh) && (handler.BKDView != null))
        {
            switch (barkoderAROverlayRefresh)
            {
                case CommonAROverlayRefresh.NORMAL:
                    {
                        handler.BKDView.Config.ArConfig.OverlayRefresh = Com.Barkoder.Overlaymanager.BarkoderAROverlayRefresh.Normal;
                        break;
                    }
                case CommonAROverlayRefresh.SMOOTH:
                    {
                        handler.BKDView.Config.ArConfig.OverlayRefresh = Com.Barkoder.Overlaymanager.BarkoderAROverlayRefresh.Smooth;
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
                case CommonDecodingSpeed.Rigorous:
                    {
                        handler.BKDView.Config.DecoderConfig.DecodingSpeed = AndroidDecodingSpeed.Rigorous;
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
                case CommonFormattingType.SADL:
                    {
                        handler.BKDView.Config.DecoderConfig.FormattingType = AndroidFormattingType.Sadl;
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

    private static void MapSetARHeaderTextFormat(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string headerTextFormat) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.HeaderTextFormat = headerTextFormat;
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

    private static void MapSetShowDuplicatesLocation(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ShowDuplicatesLocations = enabled;


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

    private static void MapSetARResultLimit(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int resultLimit) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.ResultLimit = resultLimit;
        }
    }

    private static void MapSetResultDisappearanceDelayMs(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int resultDiseapereanceDelayMs) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.ResultDisappearanceDelayMs = resultDiseapereanceDelayMs;
        }
    }

    private static void MapSetARHeaderVerticalTextMargin(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double headerVerticalTextMargin) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.HeaderVerticalTextMargin = (float)headerVerticalTextMargin;
        }
    }

    private static void MapSetARHeaderHorizontalTextMargin(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double headerHorizontalTextMargin) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.HeaderHorizontalTextMargin = (float)headerHorizontalTextMargin;
        }
    }

    private static void MapSetARHeaderHeight(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double headerHeight) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.HeaderHeight = (float)headerHeight;
        }
    }

    private static void MapSetARHeaderMaxTextHeight(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double headerHeightMaxText) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.HeaderMaxTextHeight = (float)headerHeightMaxText;
        }
    }

    private static void MapSetARHeaderMinTextHeight(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is double headerHeightMinText) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.HeaderMinTextHeight = (float)headerHeightMinText;
        }
    }

    private static void MapSetDuplicatesDelayMs(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int duplicatesDelayMs) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.DuplicatesDelayMs = duplicatesDelayMs;
        }
    }

    private static void MapSetScanningIndicatorAnimationMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int scanningIndicatorAnimationMode) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ScanningIndicatorAnimation = scanningIndicatorAnimationMode;
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
        if ((arg3 is CommonBarkoderCameraPosition cameraPosition) && (handler.BKDView != null))
        {
            switch (cameraPosition)
            {
                case CommonBarkoderCameraPosition.BACK:
                    {
                        handler.BKDView.SetCamera(AndroidBarkoderCameraPosition.Back);
                        break;
                    }
                case CommonBarkoderCameraPosition.FRONT:
                    {
                        handler.BKDView.SetCamera(AndroidBarkoderCameraPosition.Front);
                        break;
                    }
            }
        }


    }

    private static void MapSetScanningIndicatorAlwaysVisibleEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ScanningIndicatorAlwaysVisible = enabled;
        }
    }

    private static void MapSetCentricFocusAndExposure(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetCentricFocusAndExposure(enabled);
        }
    }

    private static void MapSetUPCE1expandToUPCA(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.UpcE1.ExpandToUPCA = enabled;
        }
    }

    private static void MapSetUPCEexpandToUPCA(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.UpcE.ExpandToUPCA = enabled;
        }
    }

    private static void MapSetVideoStabilization(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.SetVideoStabilization(enabled);
        }
    }

    private static void MapSetScanningIndicatorColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is string hexColor) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ScanningIndicatorColor = Util.HexColorToIntColor(hexColor);
        }
    }

    private static void MapSetScanningIndicatorWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is float width) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ScanningIndicatorWidth = width;
        }
    }

    private static void MapSetEnableComposite(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is int enableComposite) && (handler.BKDView != null))
        {
            handler.BKDView.Config.DecoderConfig.EnableComposite = enableComposite;
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
                case Enums.BarcodeType.Maxicode:
                    handler.BKDView.Config.DecoderConfig.MaxiCode.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.IDDocument:
                    handler.BKDView.Config.DecoderConfig.IDDocument.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Databar14:
                    handler.BKDView.Config.DecoderConfig.Databar14.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.DatabarLimited:
                    handler.BKDView.Config.DecoderConfig.DatabarLimited.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.DatabarExpanded:
                    handler.BKDView.Config.DecoderConfig.DatabarExpanded.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.PostalIMB:
                    handler.BKDView.Config.DecoderConfig.PostalIMB.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Postnet:
                    handler.BKDView.Config.DecoderConfig.Postnet.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.Planet:
                    handler.BKDView.Config.DecoderConfig.Planet.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.AustralianPost:
                    handler.BKDView.Config.DecoderConfig.AustralianPost.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.RoyalMail:
                    handler.BKDView.Config.DecoderConfig.RoyalMail.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.KIX:
                    handler.BKDView.Config.DecoderConfig.Kix.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.JapanesePost:
                    handler.BKDView.Config.DecoderConfig.JapanesePost.Enabled = barcodeTypeEventArgs.Enabled;
                    break;
                case Enums.BarcodeType.OCRText:
                    handler.BKDView.Config.DecoderConfig.OCRText.Enabled = barcodeTypeEventArgs.Enabled;
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

    private static void MapSetDoubleTapToFreezeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if ((arg3 is bool enabled) && (handler.BKDView != null))
        {
            handler.BKDView.Config.ArConfig.DoubleTapToFreezeEnabled = enabled;
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

            // Convert IList<BKKeyValue> to Dictionary<string, object>
            Dictionary<string, object>? extraDict = null;

            if (res.Extra != null && res.Extra.Any())
            {
                extraDict = new Dictionary<string, object>();
                foreach (var kv in res.Extra)
                {
                    if (!string.IsNullOrEmpty(kv.Key))
                    {
                        extraDict[kv.Key] = kv.Value ?? string.Empty; // Handle null values
                    }
                }
            }

            // Convert Images
            if (res.Images != null)
            {
                foreach (var image in res.Images)
                {
                    if (image.Image != null)
                    {
                        ImageSource? imageSource = BitmapToImageSource(image.Image);

                        if (imageSource != null)
                        {
                            mrzImages.Add(new ImageData(image.Name, imageSource));
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
            string binaryDataAsBase64 = res.BinaryData != null
    ? Convert.ToBase64String(res.BinaryData.ToArray())
    : string.Empty;
            var sadlImage = BarkoderHelper.SadlImage(res.Extra?.ToArray() ?? Array.Empty<BKKeyValue>());
            var sadlImageBase64 = SadlImageToBase64(sadlImage);


            // Create BarcodeResult for each scanned result
            BarcodeResult barcodeResult = new BarcodeResult(
                res.TextualData,
                res.BarcodeTypeName,
                extraDict, // Pass the dictionary here
                "", // CharacterSet
                mrzImages,
                 ConvertLocation(res),
                 binaryDataAsBase64,
                 sadlImageBase64
            );

            barcodeResults[i] = barcodeResult;
        }

        ImageSource[]? thumbnails = bitmaps?
              .Where(b => b != null)
              .Select(b => BitmapToImageSource(b))
              .Where(src => src != null)
              .Cast<ImageSource>()
              .ToArray() ?? Array.Empty<ImageSource>();

        // Convert original image
        ImageSource originalImageSource = BitmapToImageSource(originalImage);

        // Call the 3-argument version (default impl will redirect if needed)

        try
        {
            // (even if empty)
        }
        finally
        {
            if (result != null)
            {
                foreach (var r in result)
                {
                    if (r?.Images != null)
                    {
                        foreach (var img in r.Images)
                        {
                            img?.Image?.Recycle();
                            img?.Image?.Dispose();
                            img?.Dispose();
                        }
                    }

                    r?.Dispose();
                }
            }

            if (bitmaps != null)
            {
                foreach (var b in bitmaps)
                {
                    b?.Recycle();
                    b?.Dispose();
                }
            }

            originalImage?.Recycle();
            originalImage?.Dispose();
        }

        BarkoderDelegate?.DidFinishScanning(barcodeResults, thumbnails, originalImageSource);
   
    }



string SadlImageToBase64(Bitmap bitmap)
{
    if (bitmap == null)
        return "";

    using (var stream = new MemoryStream())
    {
        bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream); // Or JPEG
        return Convert.ToBase64String(stream.ToArray());
    }
}


private BarcodeLocation ConvertLocation(Com.Barkoder.Barkoder.Result res)
    {
        if (res.Location == null)
            return null;

        return new BarcodeLocation
        {
            LocationName = res.Location.LocationName,
            Points = res.Location.Points?
                        .Select(p => new BarcodePoint(p.X, p.Y))
                        .ToList() ?? new List<BarcodePoint>()
        };
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

    public class SimpleKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }



}



#endif
