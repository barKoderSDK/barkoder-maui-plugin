﻿using System;
using Plugin.Maui.Barkoder.Enums;
using Plugin.Maui.Barkoder.Handlers;
using Plugin.Maui.Barkoder.Interfaces;
using Barkoder;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;

namespace Plugin.Maui.Barkoder.Controls;

public class BarkoderView : View
{
    private List<BarcodeTypeEventArgs> BarcodeTypes;

    /// <summary>
    /// Initializes a new instance of the BarkoderView class.
    /// </summary>
    public BarkoderView()
    {
        BarcodeTypes = new List<BarcodeTypeEventArgs>();

        foreach (BarcodeType barcodeType in Enum.GetValues(typeof(BarcodeType)))
        {
            BarcodeTypes.Add(new BarcodeTypeEventArgs(barcodeType, false));
        }
    }

    public void SetCustomOption(string optionName, int optionValue)
    {
        Handler?.Invoke(nameof(SetCustomOption), (optionName, optionValue));
    }

  
    public void SetDynamicExposure(int dynamicExposure)
    {
        Handler?.Invoke(nameof(SetDynamicExposure), dynamicExposure);
    }

    public void SetCamera(BarkoderCameraPosition position)
    {
        Handler?.Invoke(nameof(SetCamera), position);
    }

    public void SetCentricFocusAndExposure(bool enabled)
    {
        Handler?.Invoke(nameof(SetCentricFocusAndExposure), enabled);
    }

    public void SetUPCE1expandToUPCA(bool enabled)
    {
        Handler?.Invoke(nameof(SetUPCE1expandToUPCA), enabled);
    }

    public void SetUPCEexpandToUPCA(bool enabled)
    {
        Handler?.Invoke(nameof(SetUPCEexpandToUPCA), enabled);
    }

    public void SetVideoStabilization(bool enabled)
    {
        Handler?.Invoke(nameof(SetVideoStabilization), enabled);
    }

    public void InitCameraProperties()
    {
        Handler?.Invoke(nameof(InitCameraProperties), null);
    }

    public void SetShowDuplicatesLocation(bool enabled)
    {
        Handler?.Invoke(nameof(SetShowDuplicatesLocation), enabled);
    }

    public float GetCurrentZoomFactor()
    {
        if (Handler is Controls.BarkoderViewHandler handler)
            return handler.GetCurrentZoomFactor();

        return -1f;
    }

    public event EventHandler? StartCameraRequested;
    public event EventHandler<IBarkoderDelegate>? StartScanningRequested;
    public event EventHandler<IBarkoderDelegate>? ScanImageRequest;
    public event EventHandler? StopScanningRequested;
    public event EventHandler? PauseScanningRequested;
    public event EventHandler? FreezeScanningRequested;
    public event EventHandler? UnfreezeScanningRequested;
    public event EventHandler<bool>? FlashEnableRequested;
    public event EventHandler<float>? SetZoomFactorRequested;
    public event EventHandler<bool>? SetPinchToZoomEnabledRequested;
    public event EventHandler<bool>? SetRegionOfInterestVisibleRequested;
    public event EventHandler<float>? SetRoiLineWidthRequested;
    public event EventHandler<bool>? SetCloseSessionOnResultEnabledRequested;
    public event EventHandler<bool>? SetImageResultEnabledRequested;
    public event EventHandler<bool>? SetARImageResultEnabledRequested;
    public event EventHandler<bool>? SetLocationInPreviewEnabledRequested;
    public event EventHandler<bool>? SetLocationInImageResultEnabledRequested;
    public event EventHandler<bool>? SetBeepOnSuccessEnabledRequested;
    public event EventHandler<bool>? SetScanningIndicatorAlwaysVisibleRequested;
    public event EventHandler<bool>? SetVibrateOnSuccessEnabledRequested;
    public event EventHandler<string>? SetLocationLineColorRequested;
    public event EventHandler<string>? SetARSelectedLocationLineColorRequested;
    public event EventHandler<string>? SetARNonSelectedLocationLineColorRequested;
    public event EventHandler<string>? SetARNonSelectedHeaderTextColorRequested;
    public event EventHandler<string>? SetARSelectedHeaderTextColorRequested;
    public event EventHandler<double>? SetARHeaderVerticalTextMarginRequested;
    public event EventHandler<double>? SetARHeaderHorizontalTextMarginRequested;
    public event EventHandler<string>? SetRoiLineColorRequested;
    public event EventHandler<string>? SetRoiOverlayBackgroundColorRequested;
    public event EventHandler<string>? SetScanningIndicatorColorHexRequested;
    public event EventHandler<double>? SetLocationLineWidthRequested;
    public event EventHandler<double>? SetARSelectedLocationLineWidthRequested;
    public event EventHandler<double>? SetARNonSelectedLocationLineWidthRequested;
    public event EventHandler<double>? SetARLocationTransitionSpeedRequested;
    public event EventHandler<double>? SetScanningIndicatorLineWidthRequested;
    public event EventHandler<int[]>? SetRegionOfInterestRequested;
    public event EventHandler<BarkoderResolution>? SetBarkoderResolutionRequested;
    public event EventHandler<BarkoderARMode>? SetBarkoderARModeRequested;
    public event EventHandler<BarkoderARHeaderShowMode>? SetBarkoderARHeaderShowModeRequested;
    public event EventHandler<BarkoderARLocationType>? SetBarkoderARLocationTypeRequested;
    public event EventHandler<BarkoderAROverlayRefresh>? SetBarkoderARoverlayRefreshRequested;
    public event EventHandler<DecodingSpeed>? SetDecodingSpeedRequested;
    public event EventHandler<FormattingType>? SetFormattingTypeRequested;
    public event EventHandler<MsiChecksumType>? SetMsiChecksumTypeRequested;
    public event EventHandler<MsiChecksumType>? SetCode11ChecksumTypeRequested;
    public event EventHandler<MsiChecksumType>? SetCode39ChecksumTypeRequested;
    public event EventHandler<bool>? SetIdDocumentMasterChecksumEnabledRequested;
    public event EventHandler<string>? SetEncodingCharacterSetRequested;
    public event EventHandler<string>? SetARHeaderTextFormatRequested;
    public event EventHandler<bool>? SetDatamatrixDpmModeEnabledRequested;
    public event EventHandler<bool>? SetQRDpmModeEnabledRequested;
    public event EventHandler<bool>? SetQRMicroDpmModeEnabledRequested;
    public event EventHandler<bool>? SetUpcEanDeblurEnabledRequested;
    public event EventHandler<bool>? SetEnableMisshaped1DEnabledRequested;
    public event EventHandler<bool>? SetBarcodeThumbnailOnResultEnabledRequested;
    public event EventHandler<bool>? SetARBarcodeThumbnailOnResultEnabledRequested;
    public event EventHandler<int>? SetMaximumResultsCountRequested;
    public event EventHandler<int>? SetResultDisappearanceDelayMsRequested;
    public event EventHandler<double>? SetARHeaderHeightRequested;
    public event EventHandler<double>? SetARHeaderMaxTextHeightRequested;
    public event EventHandler<double>? SetARHeaderMinTextHeightRequested;
    public event EventHandler<int>? SetScanningIndicatorAnimationModeRequested;
    public event EventHandler<int>? SetDuplicatesDelayMsRequested;
    public event EventHandler<BarcodeTypeEventArgs>? SetBarcodeTypeEnabledRequested;
    public event EventHandler<bool>? SetEnableVINRestrictionsRequested;
    public event EventHandler<bool>? SetARDoubleTapToFreezeEnabledRequested;
    public event EventHandler<int>? SetThresholdBetweenDuplicatesScansRequested;
    public event EventHandler<int>? SetEnableCompositeRequested;
    public event EventHandler<BarcodeRangeEventArg>? SetBarcodeTypeLengthRangeRequested;
    public event EventHandler<string>? ConfigureBarkoderRequested;

    // Bindable properties


   
  

public static BindableProperty RegionOfInterestVisibleProperty = BindableProperty.Create(nameof(RegionOfInterestVisible)
    , typeof(bool)
    , typeof(BarkoderView)
    , true
    , defaultBindingMode: BindingMode.TwoWay);

    public static BindableProperty LicenseKeyProperty = BindableProperty.Create(nameof(LicenseKey)
    , typeof(string)
    , typeof(BarkoderView)
    , "Default_License_key");

    public static BindableProperty VersionProperty = BindableProperty.Create(nameof(Version)
    , typeof(string)
    , typeof(BarkoderView)
    , "1.0");

    public static BindableProperty IsFlashAvailableProperty = BindableProperty.Create(nameof(RegionOfInterestVisible)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty MaxZoomFactorProperty = BindableProperty.Create(nameof(MaxZoomFactor)
    , typeof(double)
    , typeof(BarkoderView)
    , 0.0);

    public static BindableProperty LocationLineColorHexProperty = BindableProperty.Create(nameof(LocationLineColorHex)
    , typeof(string)
    , typeof(BarkoderView)
    , "#000000");

    public static BindableProperty ARNonSelectedLocationLineColorHexProperty = BindableProperty.Create(nameof(ARNonSelectedLocationLineColorHex)
  , typeof(string)
  , typeof(BarkoderView)
  , "#000000");

    public static BindableProperty ARSelectedLocationLineColorHexProperty = BindableProperty.Create(nameof(ARSelectedLocationLineColorHex)
 , typeof(string)
 , typeof(BarkoderView)
 , "#000000");

    public static BindableProperty ARSelectedHeaderTextColorHexProperty = BindableProperty.Create(nameof(ARSelectedHeaderTextColorHex)
, typeof(string)
, typeof(BarkoderView)
, "#000000");

    public static BindableProperty ARNonSelectedHeaderTextColorHexProperty = BindableProperty.Create(nameof(ARNonSelectedHeaderTextColorHex)
, typeof(string)
, typeof(BarkoderView)
, "#000000");

    public static BindableProperty RoiLineColorHexProperty = BindableProperty.Create(nameof(RoiLineColorHex)
    , typeof(string)
    , typeof(BarkoderView)
    , "#000000");

    public static BindableProperty ScanningIndicatorColorHexProperty = BindableProperty.Create(nameof(ScanningIndicatorLineColorHex)
   , typeof(string)
   , typeof(BarkoderView)
   , "#000000");

    public static BindableProperty RoiOverlayBackgroundColorHexProperty = BindableProperty.Create(nameof(RoiOverlayBackgroundColorHex)
    , typeof(string)
    , typeof(BarkoderView)
    , "#000000");

    public static BindableProperty EncodingCharacterSetProperty = BindableProperty.Create(nameof(EncodingCharacterSet)
    , typeof(string)
    , typeof(BarkoderView)
    , "N/A");

    public static BindableProperty ARHeaderTextFormatProperty = BindableProperty.Create(nameof(ARHeaderTextFormat)
  , typeof(string)
  , typeof(BarkoderView)
  , "N/A");

    public static BindableProperty LocationLineWidthProperty = BindableProperty.Create(nameof(LocationLineWidth)
    , typeof(double)
    , typeof(BarkoderView)
    , 0.0);

    public static BindableProperty ARSelectedLocationLineWidthProperty = BindableProperty.Create(nameof(ARSelectedLocationLineWidth)
   , typeof(double)
   , typeof(BarkoderView)
   , 0.0);

    public static BindableProperty ARNonSelectedLocationLineWidthProperty = BindableProperty.Create(nameof(ARNonSelectedLocationLineWidth)
  , typeof(double)
  , typeof(BarkoderView)
  , 0.0);

    public static BindableProperty ARLocationTransitionSpeedProperty = BindableProperty.Create(nameof(ARLocationTransitionSpeed)
, typeof(double)
, typeof(BarkoderView)
, 0.0);

    public static BindableProperty ScanningIndicatorLineWidthProperty = BindableProperty.Create(nameof(ScanningIndicatorLineWidth)
    , typeof(double)
    , typeof(BarkoderView)
    , 0.0);

    public static BindableProperty RoiLineWidthProperty = BindableProperty.Create(nameof(RoiLineWidth)
    , typeof(double)
    , typeof(BarkoderView)
    , 0.0);

    public static BindableProperty ImageResultEnabledProperty = BindableProperty.Create(nameof(ImageResultEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty ARImageResultEnabledProperty = BindableProperty.Create(nameof(ARImageResultEnabled)
   , typeof(bool)
   , typeof(BarkoderView)
   , false);

    public static BindableProperty LocationInImageResultEnabledProperty = BindableProperty.Create(nameof(LocationInImageResultEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty LocationInPreviewEnabledProperty = BindableProperty.Create(nameof(LocationInPreviewEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty PinchToZoomEnabledProperty = BindableProperty.Create(nameof(PinchToZoomEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty BeepOnSuccessEnabledProperty = BindableProperty.Create(nameof(BeepOnSuccessEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty ScanningIndicatorAlwaysVisibleProperty = BindableProperty.Create(nameof(ScanningIndicatorAlwaysVisibleEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty VibrateOnSuccessEnabledProperty = BindableProperty.Create(nameof(VibrateOnSuccessEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty CloseSessionOnResultEnabledProperty = BindableProperty.Create(nameof(CloseSessionOnResultEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty BarkoderResolutionProperty = BindableProperty.Create(nameof(BarkoderResolution)
    , typeof(BarkoderResolution)
    , typeof(BarkoderView)
    , BarkoderResolution.HD);

    public static BindableProperty BarkoderARModeProperty = BindableProperty.Create(nameof(BarkoderARMode)
   , typeof(BarkoderARMode)
   , typeof(BarkoderView)
   , BarkoderARMode.OFF);

    public static BindableProperty BarkoderARHeaderShowModeProperty = BindableProperty.Create(nameof(BarkoderARHeaderShowMode)
, typeof(BarkoderARHeaderShowMode)
, typeof(BarkoderView)
, BarkoderARHeaderShowMode.ONSELECTED);

    public static BindableProperty BarkoderARLocationTypeProperty = BindableProperty.Create(nameof(BarkoderARLocationType)
, typeof(BarkoderARLocationType)
, typeof(BarkoderView)
, BarkoderARLocationType.TIGHT);

    public static BindableProperty BarkoderARoverlayRefreshProperty = BindableProperty.Create(nameof(BarkoderAROverlayRefresh)
, typeof(BarkoderAROverlayRefresh)
, typeof(BarkoderView)
, BarkoderAROverlayRefresh.NORMAL);

    public static BindableProperty DecodingSpeedProperty = BindableProperty.Create(nameof(DecodingSpeed)
    , typeof(DecodingSpeed)
    , typeof(BarkoderView)
    , DecodingSpeed.Normal);

    public static BindableProperty FormattingTypeProperty = BindableProperty.Create(nameof(FormattingType)
    , typeof(FormattingType)
    , typeof(BarkoderView)
    , FormattingType.Automatic);

    public static BindableProperty MsiChecksumTypeProperty = BindableProperty.Create(nameof(MsiChecksumType)
    , typeof(MsiChecksumType)
    , typeof(BarkoderView)
    , MsiChecksumType.Mod10);

    public static BindableProperty Code11ChecksumTypeProperty = BindableProperty.Create(nameof(Code11ChecksumType)
    , typeof(Code11ChecksumType)
    , typeof(BarkoderView)
    , Code11ChecksumType.Disabled);

    public static BindableProperty Code39ChecksumTypeProperty = BindableProperty.Create(nameof(Code39ChecksumType)
    , typeof(Code39ChecksumType)
    , typeof(BarkoderView)
    , Code39ChecksumType.Disabled);

    public static BindableProperty IdDocumentMasterChecksumEnabledProperty = BindableProperty.Create(nameof(IdDocumentMasterCheckSumEnabled)
   , typeof(bool)
   , typeof(BarkoderView)
   , false);

    public static BindableProperty DatamatrixDpmModeEnabledProperty = BindableProperty.Create(nameof(DatamatrixDpmModeEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty QRDpmModeEnabledProperty = BindableProperty.Create(nameof(QRDpmModeEnabled)
   , typeof(bool)
   , typeof(BarkoderView)
   , false);

    public static BindableProperty QRMicroDpmModeEnabledProperty = BindableProperty.Create(nameof(QRMIcroDpmModeEnabled)
   , typeof(bool)
   , typeof(BarkoderView)
   , false);

    public static BindableProperty UpcEanDeblurEnabledProperty = BindableProperty.Create(nameof(UpcEanDeblurEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty EnableMisshaped1DEnabledProperty = BindableProperty.Create(nameof(EnableMisshaped1DEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty BarcodeThumbnailOnResultEnabledProperty = BindableProperty.Create(nameof(BarcodeThumbnailOnResultEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty ARBarcodeThumbnailOnResultEnabledProperty = BindableProperty.Create(nameof(ARBarcodeThumbnailOnResultEnabled)
  , typeof(bool)
  , typeof(BarkoderView)
  , false);

    public static BindableProperty MaximumResultsCountProperty = BindableProperty.Create(nameof(MaximumResultsCount)
    , typeof(int)
    , typeof(BarkoderView)
    , 0);

    public static BindableProperty ResultDisappearanceDelayMsProperty = BindableProperty.Create(nameof(ResultDisappearanceDelayMs)
   , typeof(int)
   , typeof(BarkoderView)
   , 0);

    public static BindableProperty HeaderARVerticalTextMarginProperty = BindableProperty.Create(nameof(HeaderARVerticalTextMargin)
  , typeof(double)
  , typeof(BarkoderView)
  , 0.0);

    public static BindableProperty HeaderARHorizontalTextMarginProperty = BindableProperty.Create(nameof(HeaderARHorizontalTextMargin)
  , typeof(double)
  , typeof(BarkoderView)
  , 0.0);

    public static BindableProperty ARHeaderHeightProperty = BindableProperty.Create(nameof(ARHeaderHeight)
 , typeof(double)
 , typeof(BarkoderView)
 , 0.0);

    public static BindableProperty ARHeaderMaxTextHeightProperty = BindableProperty.Create(nameof(ARHeaderMaxTextHeight)
, typeof(double)
, typeof(BarkoderView)
, 0.0);
    public static BindableProperty ARHeaderMinTextHeightProperty = BindableProperty.Create(nameof(ARHeaderMinTextHeight)
, typeof(double)
, typeof(BarkoderView)
, 0.0);

    public static BindableProperty ScanningIndicatorAnimationModeProperty = BindableProperty.Create(nameof(ScanningIndicatorAnimationMode)
    , typeof(int)
    , typeof(BarkoderView)
    , 0);

    public static BindableProperty DuplicatesDelayMsProperty = BindableProperty.Create(nameof(DuplicatesDelayMs)
    , typeof(int)
    , typeof(BarkoderView)
    , 0);

    public static BindableProperty VINRestrictionsEnabledProperty = BindableProperty.Create(nameof(VINRestrictionsEnabled)
    , typeof(bool)
    , typeof(BarkoderView)
    , false);

    public static BindableProperty ARDoubleTapToFreezeEnabledProperty = BindableProperty.Create(nameof(ARDoubleTapToFreezeEnabled)
, typeof(bool)
, typeof(BarkoderView)
, false);

    public static BindableProperty ThresholdBetweenDuplicatesScansProperty = BindableProperty.Create(nameof(ThresholdBetweenDuplicatesScans)
    , typeof(int)
    , typeof(BarkoderView)
    , 0);

    public static BindableProperty EnableCompositeProperty = BindableProperty.Create(nameof(EnableComposite)
   , typeof(int)
   , typeof(BarkoderView)
   , 0);

    public static BindableProperty RegionOfInterestProperty = BindableProperty.Create(nameof(RegionOfInterest)
    , typeof((int, int, int, int))
    , typeof(BarkoderView)
    , (0, 0, 0, 0),
    defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty IsIdDocumentMasterChecksumEnabledProperty =
   BindableProperty.Create(nameof(IsIdDocumentMasterChecksumEnabled), typeof(bool), typeof(BarkoderView), false);

    public static readonly BindableProperty IsDatamatrixDpmModeEnabledProperty =
    BindableProperty.Create(nameof(IsDatamatrixDpmModeEnabled), typeof(bool), typeof(BarkoderView), false);

    public static readonly BindableProperty IsQRDpmModeEnabledProperty =
        BindableProperty.Create(nameof(IsQRDpmModeEnabled), typeof(bool), typeof(BarkoderView), false);

    public static readonly BindableProperty IsQRMicroDpmModeEnabledProperty =
        BindableProperty.Create(nameof(IsQRMicroDpmModeEnabled), typeof(bool), typeof(BarkoderView), false);


    // Properties


    /// <summary>
    /// Retrieves the version of the Barkoder library.
    /// </summary>
    public string Version
    {
        get => (string)GetValue(VersionProperty);
        set => SetValue(VersionProperty, value);
    }

    public string LicenseKey
    {
        get => (string)GetValue(LicenseKeyProperty);
        set => SetValue(LicenseKeyProperty, value);
    }

    /// <summary>
    /// Checks or sets if the region of interest (ROI) is visible.
    /// </summary>
    public bool RegionOfInterestVisible
    {
        get => (bool)GetValue(RegionOfInterestVisibleProperty);
        set
        {
            SetValue(RegionOfInterestVisibleProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetRegionOfInterestVisibleRequested), value);
        }
    }

    /// <summary>
    /// Checks whether the device has a built-in flash (torch) that can be used for illumination during barcode scanning.
    /// </summary>
    public bool IsFlashAvailable
    {
        get => (bool)GetValue(IsFlashAvailableProperty);
        set => SetValue(IsFlashAvailableProperty, value);
    }

    /// <summary>
    /// Retrieves or sets the maximum available zoom factor for the device's camera.
    /// </summary>
    public double MaxZoomFactor
    {
        get => (double)GetValue(MaxZoomFactorProperty);
        set
        {
            SetValue(MaxZoomFactorProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetZoomFactorRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the hexadecimal color code representing the line color used to indicate the location of detected barcodes.
    /// </summary>
    public string LocationLineColorHex
    {
        get => (string)GetValue(LocationLineColorHexProperty);
        set
        {
            SetValue(LocationLineColorHexProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetLocationLineColorRequested), value);
        }
    }

    public string ARNonSelectedLocationLineColorHex
    {
        get => (string)GetValue(ARNonSelectedLocationLineColorHexProperty);
        set
        {
            SetValue(ARNonSelectedLocationLineColorHexProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARNonSelectedLocationLineColorRequested), value);
        }
    }

    public string ARSelectedLocationLineColorHex
    {
        get => (string)GetValue(ARSelectedLocationLineColorHexProperty);
        set
        {
            SetValue(ARSelectedLocationLineColorHexProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARSelectedLocationLineColorRequested), value);
        }
    }

    public string ARSelectedHeaderTextColorHex
    {
        get => (string)GetValue(ARSelectedHeaderTextColorHexProperty);
        set
        {
            SetValue(ARSelectedHeaderTextColorHexProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARSelectedHeaderTextColorRequested), value);
        }
    }

    public string ARNonSelectedHeaderTextColorHex
    {
        get => (string)GetValue(ARNonSelectedHeaderTextColorHexProperty);
        set
        {
            SetValue(ARNonSelectedHeaderTextColorHexProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARNonSelectedHeaderTextColorRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the hexadecimal color code representing the line color of the Region of Interest (ROI) on the camera preview.
    /// </summary>
    public string RoiLineColorHex
    {
        get => (string)GetValue(RoiLineColorHexProperty);
        set
        {
            SetValue(RoiLineColorHexProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetRoiLineColorRequested), value);
        }
    }

    public string ScanningIndicatorLineColorHex
    {
        get => (string)GetValue(ScanningIndicatorColorHexProperty);
        set
        {
            SetValue(ScanningIndicatorColorHexProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetScanningIndicatorColorHexRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the hexadecimal color code representing the background color of the overlay within the Region of Interest (ROI) on the camera preview.
    /// </summary>
    public string RoiOverlayBackgroundColorHex
    {
        get => (string)GetValue(RoiOverlayBackgroundColorHexProperty);
        set
        {
            SetValue(RoiOverlayBackgroundColorHexProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetRoiOverlayBackgroundColorRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the character set used for encoding barcode data.
    /// </summary>
    public string EncodingCharacterSet
    {
        get => (string)GetValue(EncodingCharacterSetProperty);
        set
        {
            SetValue(EncodingCharacterSetProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetEncodingCharacterSetRequested), value);
        }
    }

    public string ARHeaderTextFormat
    {
        get => (string)GetValue(ARHeaderTextFormatProperty);
        set
        {
            SetValue(ARHeaderTextFormatProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARHeaderTextFormatRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the current width setting for the lines indicating the location of detected barcodes on the camera feed.
    /// </summary>
    public double LocationLineWidth
    {
        get => (double)GetValue(LocationLineWidthProperty);
        set
        {
            SetValue(LocationLineWidthProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetLocationLineWidthRequested), value);
        }
    }

    public double ARSelectedLocationLineWidth
    {
        get => (double)GetValue(ARSelectedLocationLineWidthProperty);
        set
        {
            SetValue(ARSelectedLocationLineWidthProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARSelectedLocationLineWidthRequested), value);
        }
    }

    public double ARNonSelectedLocationLineWidth
    {
        get => (double)GetValue(ARNonSelectedLocationLineWidthProperty);
        set
        {
            SetValue(ARNonSelectedLocationLineWidthProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARNonSelectedLocationLineWidthRequested), value);
        }
    }

    public double ARLocationTransitionSpeed
    {
        get => (double)GetValue(ARLocationTransitionSpeedProperty);
        set
        {
            SetValue(ARLocationTransitionSpeedProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARLocationTransitionSpeedRequested), value);
        }
    }

    public double ScanningIndicatorLineWidth
    {
        get => (double)GetValue(ScanningIndicatorLineWidthProperty);
        set
        {
            SetValue(ScanningIndicatorLineWidthProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetScanningIndicatorLineWidthRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the current width setting for the lines outlining the Region of Interest (ROI) on the camera preview.
    /// </summary>
    public double RoiLineWidth
    {
        get => (double)GetValue(RoiLineWidthProperty);
        set
        {
            SetValue(RoiLineWidthProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetRoiLineWidthRequested), value);
        }
    }

    /// <summary>
    /// Checks or sets if image result is enabled.
    /// </summary>
    public bool ImageResultEnabled
    {
        get => (bool)GetValue(ImageResultEnabledProperty);
        set
        {
            SetValue(ImageResultEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetImageResultEnabledRequested), value);
        }
    }

    public bool ARImageResultEnabled
    {
        get => (bool)GetValue(ARImageResultEnabledProperty);
        set
        {
            SetValue(ARImageResultEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARImageResultEnabledRequested), value);
        }
    }

    /// <summary>
    /// Checks or sets if location in image result is enabled.
    /// </summary>
    public bool LocationInImageResultEnabled
    {
        get => (bool)GetValue(LocationInImageResultEnabledProperty);
        set
        {
            SetValue(LocationInImageResultEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetLocationInImageResultEnabledRequested), true);
        }
    }

    /// <summary>
    /// Checks if location in preview is enabled.
    /// </summary>
    public bool LocationInPreviewEnabled
    {
        get => (bool)GetValue(LocationInPreviewEnabledProperty);
        set
        {
            SetValue(LocationInPreviewEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetLocationInPreviewEnabledRequested), value);
        }
    }

    /// <summary>
    /// Checks or sets if pinch to zoom is enabled.
    /// </summary>
    public bool PinchToZoomEnabled
    {
        get => (bool)GetValue(PinchToZoomEnabledProperty);
        set
        {
            SetValue(PinchToZoomEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetPinchToZoomEnabledRequested), value);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether a beep sound is played on successful barcode scanning.
    /// </summary>
    public bool BeepOnSuccessEnabled
    {
        get => (bool)GetValue(BeepOnSuccessEnabledProperty);
        set
        {
            SetValue(BeepOnSuccessEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetBeepOnSuccessEnabledRequested), value);
        }
    }

    public bool ScanningIndicatorAlwaysVisibleEnabled
    {
        get => (bool)GetValue(ScanningIndicatorAlwaysVisibleProperty);
        set
        {
            SetValue(ScanningIndicatorAlwaysVisibleProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetScanningIndicatorAlwaysVisibleRequested), value);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether vibration is enabled on successful barcode scanning.
    /// </summary>
    public bool VibrateOnSuccessEnabled
    {
        get => (bool)GetValue(VibrateOnSuccessEnabledProperty);
        set
        {
            SetValue(VibrateOnSuccessEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetVibrateOnSuccessEnabledRequested), value);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the session is closed upon detecting a result during barcode scanning.
    /// </summary>
    public bool CloseSessionOnResultEnabled
    {
        get => (bool)GetValue(CloseSessionOnResultEnabledProperty);
        set
        {
            SetValue(CloseSessionOnResultEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetCloseSessionOnResultEnabledRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the resolution for barcode scanning.
    /// </summary>
    public BarkoderResolution BarkoderResolution
    {
        get => (BarkoderResolution)GetValue(BarkoderResolutionProperty);
        set
        {
            SetValue(BarkoderResolutionProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetBarkoderResolutionRequested), value);
        }
    }

    public BarkoderARMode BarkoderARMode
    {
        get => (BarkoderARMode)GetValue(BarkoderARModeProperty);
        set
        {
            SetValue(BarkoderARModeProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetBarkoderARModeRequested), value);
        }
    }

    public BarkoderARHeaderShowMode BarkoderARHeaderShowMode
    {
        get => (BarkoderARHeaderShowMode)GetValue(BarkoderARHeaderShowModeProperty);
        set
        {
            SetValue(BarkoderARHeaderShowModeProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetBarkoderARHeaderShowModeRequested), value);
        }
    }

    public BarkoderARLocationType BarkoderARLocationType
    {
        get => (BarkoderARLocationType)GetValue(BarkoderARLocationTypeProperty);
        set
        {
            SetValue(BarkoderARLocationTypeProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetBarkoderARLocationTypeRequested), value);
        }
    }


    public BarkoderAROverlayRefresh BarkoderAROverlayRefresh
    {
        get => (BarkoderAROverlayRefresh)GetValue(BarkoderARoverlayRefreshProperty);
        set
        {
            SetValue(BarkoderARoverlayRefreshProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetBarkoderARoverlayRefreshRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the current decoding speed setting for barcode scanning.
    /// </summary>
    public DecodingSpeed DecodingSpeed
    {
        get => (DecodingSpeed)GetValue(DecodingSpeedProperty);
        set
        {
            SetValue(DecodingSpeedProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetDecodingSpeedRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the formatting type used for presenting decoded barcode data..
    /// </summary>
    public FormattingType FormattingType
    {
        get => (FormattingType)GetValue(FormattingTypeProperty);
        set
        {
            SetValue(FormattingTypeProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetFormattingTypeRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the MSI checksum type.
    /// </summary>
    public MsiChecksumType MsiChecksumType
    {
        get => (MsiChecksumType)GetValue(MsiChecksumTypeProperty);
        set
        {
            SetValue(MsiChecksumTypeProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetMsiChecksumTypeRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the Code11 checksum type.
    /// </summary>
    public Code11ChecksumType Code11ChecksumType
    {
        get => (Code11ChecksumType)GetValue(Code11ChecksumTypeProperty);
        set
        {
            SetValue(Code11ChecksumTypeProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetCode11ChecksumTypeRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the checksum type for Code 39 barcodes.
    /// </summary>
    public Code39ChecksumType Code39ChecksumType
    {
        get => (Code39ChecksumType)GetValue(Code39ChecksumTypeProperty);
        set
        {
            SetValue(Code39ChecksumTypeProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetCode39ChecksumTypeRequested), value);
        }
    }


    public bool IdDocumentMasterCheckSumEnabled
    {
        get => (bool)GetValue(IdDocumentMasterChecksumEnabledProperty);
        set
        {
            SetValue(IdDocumentMasterChecksumEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetIdDocumentMasterChecksumEnabledRequested), value);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether Direct Part Marking (DPM) mode is enabled for Datamatrix barcodes.
    /// </summary>
    public bool DatamatrixDpmModeEnabled
    {
        get => (bool)GetValue(DatamatrixDpmModeEnabledProperty);
        set
        {
            SetValue(DatamatrixDpmModeEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetDatamatrixDpmModeEnabledRequested), value);
        }
    }

    public bool QRDpmModeEnabled
    {
        get => (bool)GetValue(QRDpmModeEnabledProperty);
        set
        {
            SetValue(QRDpmModeEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetQRDpmModeEnabledRequested), value);
        }
    }

    public bool QRMIcroDpmModeEnabled
    {
        get => (bool)GetValue(QRMicroDpmModeEnabledProperty);
        set
        {
            SetValue(QRMicroDpmModeEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetQRMicroDpmModeEnabledRequested), value);
        }
    }

    public bool IsIdDocumentMasterChecksumEnabled
    {
        get => (bool)GetValue(IsIdDocumentMasterChecksumEnabledProperty);
        set => SetValue(IsIdDocumentMasterChecksumEnabledProperty, value);
    }

    public bool IsDatamatrixDpmModeEnabled
    {
        get => (bool)GetValue(IsDatamatrixDpmModeEnabledProperty);
        set => SetValue(IsDatamatrixDpmModeEnabledProperty, value);
    }

    public bool IsQRDpmModeEnabled
    {
        get => (bool)GetValue(IsQRDpmModeEnabledProperty);
        set => SetValue(IsQRDpmModeEnabledProperty, value);
    }

    public bool IsQRMicroDpmModeEnabled
    {
        get => (bool)GetValue(IsQRMicroDpmModeEnabledProperty);
        set => SetValue(IsQRMicroDpmModeEnabledProperty, value);
    }


/// <summary>
/// Sets or retrieves the value indicating whether deblurring is enabled for UPC/EAN barcodes.
/// </summary>
public bool UpcEanDeblurEnabled
    {
        get => (bool)GetValue(UpcEanDeblurEnabledProperty);
        set
        {
            SetValue(UpcEanDeblurEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetUpcEanDeblurEnabledRequested), value);
        }
    }

    /// <summary>
    /// Checks if the detection of misshaped 1D barcodes is enabled.
    /// </summary>
    public bool EnableMisshaped1DEnabled
    {
        get => (bool)GetValue(EnableMisshaped1DEnabledProperty);
        set
        {
            SetValue(EnableMisshaped1DEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetEnableMisshaped1DEnabledRequested), value);
        }
    }

    /// <summary>
    /// Checks if the barcode thumbnail on result is enabled.
    /// </summary>
    public bool BarcodeThumbnailOnResultEnabled
    {
        get => (bool)GetValue(BarcodeThumbnailOnResultEnabledProperty);
        set
        {
            SetValue(BarcodeThumbnailOnResultEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetBarcodeThumbnailOnResultEnabledRequested), value);
        }
    }

    public bool ARBarcodeThumbnailOnResultEnabled
    {
        get => (bool)GetValue(ARBarcodeThumbnailOnResultEnabledProperty);
        set
        {
            SetValue(ARBarcodeThumbnailOnResultEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARBarcodeThumbnailOnResultEnabledRequested), value);
        }
    }

    /// <summary>
    /// Retrieves the maximum number of results to be returned from barcode scanning at once.
    /// </summary>
    public int MaximumResultsCount
    {
        get => (int)GetValue(MaximumResultsCountProperty);
        set
        {
            SetValue(MaximumResultsCountProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetMaximumResultsCountRequested), value);
        }
    }

    public int ResultDisappearanceDelayMs
    {
        get => (int)GetValue(ResultDisappearanceDelayMsProperty);
        set
        {
            SetValue(ResultDisappearanceDelayMsProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetResultDisappearanceDelayMsRequested), value);
        }
    }

    public double HeaderARVerticalTextMargin
    {
        get => (double)GetValue(HeaderARVerticalTextMarginProperty);
        set
        {
            SetValue(HeaderARVerticalTextMarginProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARHeaderVerticalTextMarginRequested), value);
        }
    }

    public double HeaderARHorizontalTextMargin
    {
        get => (double)GetValue(HeaderARHorizontalTextMarginProperty);
        set
        {
            SetValue(HeaderARHorizontalTextMarginProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARHeaderHorizontalTextMarginRequested), value);
        }
    }

    public double ARHeaderHeight
    {
        get => (double)GetValue(ARHeaderHeightProperty);
        set
        {
            SetValue(ARHeaderHeightProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARHeaderHeightRequested), value);
        }
    }
    public double ARHeaderMaxTextHeight
    {
        get => (double)GetValue(ARHeaderMaxTextHeightProperty);
        set
        {
            SetValue(ARHeaderMaxTextHeightProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARHeaderMaxTextHeightRequested), value);
        }
    }
    public double ARHeaderMinTextHeight
    {
        get => (double)GetValue(ARHeaderMinTextHeightProperty);
        set
        {
            SetValue(ARHeaderMinTextHeightProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARHeaderMinTextHeightRequested), value);
        }
    }

    public int ScanningIndicatorAnimationMode
    {
        get => (int)GetValue(ScanningIndicatorAnimationModeProperty);
        set
        {
            SetValue(ScanningIndicatorAnimationModeProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetScanningIndicatorAnimationModeRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the delay in milliseconds for considering duplicate barcodes during scanning.
    /// </summary>
    private int DuplicatesDelayMs
    {
        get => (int)GetValue(DuplicatesDelayMsProperty);
        set
        {
            SetValue(DuplicatesDelayMsProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetDuplicatesDelayMsRequested), value);
        }
    }

    /// <summary>
    /// Sets or checks if VIN restrictions are enabled.
    /// </summary>
    public bool VINRestrictionsEnabled
    {
        get => (bool)GetValue(VINRestrictionsEnabledProperty);
        set
        {
            SetValue(VINRestrictionsEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetEnableVINRestrictionsRequested), value);
        }
    }


    public bool ARDoubleTapToFreezeEnabled
    {
        get => (bool)GetValue(ARDoubleTapToFreezeEnabledProperty);
        set
        {
            SetValue(ARDoubleTapToFreezeEnabledProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetARDoubleTapToFreezeEnabledRequested), value);
        }
    }

    /// <summary>
    /// Retrieves the threshold between duplicate scans.
    /// </summary>
    public int ThresholdBetweenDuplicatesScans
    {
        get => (int)GetValue(ThresholdBetweenDuplicatesScansProperty);
        set
        {
            SetValue(ThresholdBetweenDuplicatesScansProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetThresholdBetweenDuplicatesScansRequested), value);
        }
    }

    public int EnableComposite
    {
        get => (int)GetValue(EnableCompositeProperty);
        set
        {
            SetValue(EnableCompositeProperty, value);
            Handler?.Invoke(nameof(BarkoderView.SetEnableCompositeRequested), value);
        }
    }

    /// <summary>
    /// Retrieves or sets the region of interest (ROI).
    /// </summary>
    public (int, int, int, int) RegionOfInterest
    {
        get => ((int, int, int, int))GetValue(RegionOfInterestProperty);
        set
        {
            SetValue(RegionOfInterestProperty, value);

            int[] roi = { value.Item1, value.Item2, value.Item3, value.Item4 };

            Handler?.Invoke(nameof(BarkoderView.SetRegionOfInterestRequested), roi);
        }
    }

    // Public methods

    /// <summary>
    /// Starts the camera for barcode scanning.
    /// </summary>
    public void StartCamera() {
        Handler?.Invoke(nameof(BarkoderView.StartCameraRequested));
    }

    /// <summary>
    /// Initiates the barcode scanning process, allowing the application to detect and decode barcodes from the device's camera feed.
    /// </summary>
    /// <param name="barkoderDelegate">The delegate to handle barcode scanning events.</param>
    public void StartScanning(IBarkoderDelegate barkoderDelegate)
    {
        Handler?.Invoke(nameof(BarkoderView.StartScanningRequested), barkoderDelegate);
    }

    public void ScanImage(string base64Image, IBarkoderDelegate barkoderDelegate)
    {
        // Pass both the delegate and the base64 string to the handler
        Handler?.Invoke(nameof(BarkoderView.ScanImageRequest), new { base64Image , barkoderDelegate });
    }

    /// <summary>
    /// Halts the barcode scanning process, stopping the camera from capturing and processing barcode information.
    /// </summary>
    public void StopScanning()
    {
        Handler?.Invoke(nameof(BarkoderView.StopScanningRequested));
    }

    /// <summary>
    /// Temporarily suspends the barcode scanning process, pausing the camera feed without completely stopping the scanning session.
    /// </summary>
    public void PauseScanning()
    {
        Handler?.Invoke(nameof(BarkoderView.PauseScanningRequested));
    }

    public void UnfreezeScanning()
    {
        Handler?.Invoke(nameof(BarkoderView.UnfreezeScanningRequested));
    }

    public void FreezeScanning()
    {
        Handler?.Invoke(nameof(BarkoderView.FreezeScanningRequested));
    }

    /// <summary>
    /// Enables or disables the device's flash (torch) for illumination during barcode scanning.
    /// </summary>
    /// <param name="enabled">True to enable the flash, false to disable it.</param>
    public void SetFlashEnabled(bool enabled)
    {
        Handler?.Invoke(nameof(BarkoderView.FlashEnableRequested), enabled);
    }

    /// <summary>
    /// Sets the zoom factor for the device's camera, adjusting the level of zoom during barcode scanning.
    /// </summary>
    /// <param name="zoomFactor">The zoom factor to set.</param>
    public void SetZoomFactor(float zoomFactor)
    {
        Handler?.Invoke(nameof(BarkoderView.SetZoomFactorRequested), zoomFactor);
    }

    /// <summary>
    /// Enables or disables the pinch-to-zoom feature for adjusting the zoom level during barcode scanning.
    /// </summary>
    /// <param name="enabled">True to enable pinch-to-zoom, false to disable it.</param>
    public void SetPinchToZoomEnabled(bool enabled)
    {
        PinchToZoomEnabled = enabled;
    }

    /// <summary>
    /// Sets the visibility of the Region of Interest (ROI) on the camera preview.
    /// </summary>
    /// <param name="visible">True to make the ROI visible, false to hide it.</param>
    public void SetRegionOfInterestVisible(bool visible)
    {
        RegionOfInterestVisible = visible;
    }

    /// <summary>
    /// Enables or disables the automatic closing of the scanning session upon detecting a barcode result.
    /// </summary>
    /// <param name="enabled">True to close the session on result detection, false otherwise.</param>
    public void SetCloseSessionOnResultEnabled(bool enabled)
    {
        CloseSessionOnResultEnabled = enabled;
    }

    /// <summary>
    /// Enables or disables the capturing and processing of image data when a barcode is successfully detected.
    /// </summary>
    /// <param name="enabled">True to enable image result display, false to disable it.</param>
    public void SetImageResultEnabled(bool enabled)
    {
        ImageResultEnabled = enabled;
    }

    public void SetARImageResultEnabled(bool enabled)
    {
        ARImageResultEnabled = enabled;
    }

    /// <summary>
    /// Enables or disables the display of barcode location information on the camera preview.
    /// </summary>
    /// <param name="enabled">True to display the location, false to hide it.</param>
    public void SetLocationInPreviewEnabled(bool enabled)
    {
        LocationInPreviewEnabled = enabled;
    }

    /// <summary>
    /// Enables or disables the inclusion of barcode location information within the image data result.
    /// </summary>
    /// <param name="enabled">True to display the location, false to hide it.</param>
    public void SetLocationInImageResultEnabled(bool enabled)
    {
        LocationInImageResultEnabled = enabled;
    }

    /// <summary>
    /// Enables or disables the audible beep sound upon successfully decoding a barcode.
    /// </summary>
    /// <param name="enabled">True to enable beep sound, false to disable it.</param>
    public void SetBeepOnSuccessEnabled(bool enabled)
    {
        BeepOnSuccessEnabled = enabled;
    }

    public void SetScanningIndicatorAlwaysVisibleEnabled(bool enabled)
    {
        ScanningIndicatorAlwaysVisibleEnabled = enabled;
    }

    /// <summary>
    /// Retrieves or sets the value indicating whether vibration is enabled on successful barcode scanning.
    /// </summary>
    /// <param name="enabled">True to enable vibration, false to disable it.</param>
    public void SetVibrateOnSuccessEnabled(bool enabled)
    {
        VibrateOnSuccessEnabled = enabled;
    }

    /// <summary>
    /// Sets the color of the lines used to indicate the location of detected barcodes on the camera feed.
    /// </summary>
    /// <param name="hexColor">The hexadecimal representation of the color.</param>
    public void SetLocationLineColor(string hexColor)
    {
        LocationLineColorHex = hexColor;
    }

    public void SetARSelectedLocationLineColor(string hexColor)
    {
        ARSelectedLocationLineColorHex = hexColor;
    }

    public void SetARSelectedHeaderTextColor(string hexColor)
    {
        ARSelectedHeaderTextColorHex = hexColor;
    }

    public void SetARNonSelectedHeaderTextColor(string hexColor)
    {
        ARNonSelectedHeaderTextColorHex = hexColor;
    }

    public void SetARNonSelectedLocationLineColor(string hexColor)
    {
        ARNonSelectedLocationLineColorHex = hexColor;
    }

    /// <summary>
    /// Sets the color of the lines outlining the Region of Interest (ROI) for barcode scanning on the camera feed.
    /// </summary>
    /// <param name="hexColor">The hexadecimal representation of the color.</param>
    public void SetRoiLineColor(string hexColor)
    {
        RoiLineColorHex = hexColor;
    }

    public void SetScanningIndicatorColor(string hexColor)
    {
        ScanningIndicatorLineColorHex = hexColor;
    }

    /// <summary>
    /// Sets the background color of the overlay within the Region of Interest (ROI) for barcode scanning on the camera feed.
    /// </summary>
    /// <param name="hexColor">The hexadecimal representation of the color.</param>
    public void SetRoiOverlayBackgroundColor(string hexColor)
    {
        RoiOverlayBackgroundColorHex = hexColor;
    }

    /// <summary>
    /// Defines the Region of Interest (ROI) on the camera preview for barcode scanning, specifying an area where the application focuses on detecting barcodes.
    /// </summary>
    /// <param name="left">The left coordinate of the ROI.</param>
    /// <param name="top">The top coordinate of the ROI.</param>
    /// <param name="width">The width of the ROI.</param>
    /// <param name="height">The height of the ROI.</param>
    public void SetRegionOfInterest(int left, int top, int width, int height)
    { 
        RegionOfInterest = (left, top, width, height);
    }

    /// <summary>
    /// Sets the resolution for barcode scanning.
    /// </summary>
    /// <param name="resolution">The resolution to be set.</param>
    public void SetBarkoderResolution(BarkoderResolution resolution)
    {
        BarkoderResolution = resolution;
    }

    public void SetBarkoderARMode(BarkoderARMode arMode)
    {
        BarkoderARMode = arMode;
    }
    public void SetBarkoderARHeaderShowMode(BarkoderARHeaderShowMode headerShowMode)
    {
        BarkoderARHeaderShowMode = headerShowMode;
    }

    public void SetBarkoderARLocationType(BarkoderARLocationType locationType)
    {
        BarkoderARLocationType = locationType;
    }

    public void SetBarkoderARoverlayRefresh(BarkoderAROverlayRefresh overlayRefresh)
    {
        BarkoderAROverlayRefresh = overlayRefresh;
    }


    /// <summary>
    /// Sets the decoding speed for barcode scanning.
    /// </summary>
    /// <param name="decodingSpeed">The decoding speed to be set.</param>
    public void SetDecodingSpeed(DecodingSpeed decodingSpeed)
    {
        DecodingSpeed = decodingSpeed;
    }

    /// <summary>
    /// Sets the formatting type for barcode scanning.
    /// </summary>
    /// <param name="formattingType">The formatting type to be set.</param>
    public void SetFormattingType(FormattingType formattingType)
    {
        FormattingType = formattingType;
    }

    /// <summary>
    /// Sets the MSI checksum type.
    /// </summary>
    /// <param name="msiChecksumType">The MSI checksum type to be set.</param>
    public void SetMsiChecksumType(MsiChecksumType msiChecksumType)
    {
        MsiChecksumType = msiChecksumType;
    }

    /// <summary>
    /// Sets the Code11 checksum type.
    /// </summary>
    /// <param name="code11ChecksumType">The Code 11 checksum type to be set.</param>
    public void SetCode11ChecksumType(Code11ChecksumType code11ChecksumType)
    {
        Code11ChecksumType = code11ChecksumType;
    }

    /// <summary>
    /// Sets the Code39 checksum type.
    /// </summary>
    /// <param name="code39ChecksumType">The Code 39 checksum type to be set.</param>
    public void SetCode39ChecksumType(Code39ChecksumType code39ChecksumType)
    {
        Code39ChecksumType = code39ChecksumType;
    }

    /// <summary>
    /// Sets the encoding character set for barcode scanning.
    /// </summary>
    /// <param name="encodingCharacterSet">The encoding character set to be set.</param>
    public void SetEncodingCharacterSet(string encodingCharacterSet)
    {
        EncodingCharacterSet = encodingCharacterSet;
    }

    public void SetHeaderTextFormatAR(string headerTextformat)
    {
        ARHeaderTextFormat = headerTextformat;
    }

    public void SetIdDocumentMasterChecksumEnabled(bool enabled)
    {
        IdDocumentMasterCheckSumEnabled = enabled;
    }

    /// <summary>
    /// Sets whether the Direct Part Marking (DPM) mode for Datamatrix barcodes is enabled.
    /// </summary>
    /// <param name="enabled">True to enable DPM mode, false to disable it.</param>
    public void SetDatamatrixDpmModeEnabled(bool enabled)
    {
        DatamatrixDpmModeEnabled = enabled;
    }

    public void SetQRDpmModeEnabled(bool enabled)
    {
        QRDpmModeEnabled = enabled;
    }

    public void SetQRMicroDpmModeEnabled(bool enabled)
    {
        QRMIcroDpmModeEnabled = enabled;
    }

    /// <summary>
    /// Sets whether the deblurring feature for UPC/EAN barcodes is enabled.
    /// </summary>
    /// <param name="enabled">True to enable deblurring, false to disable it.</param>
    public void SetUpcEanDeblurEnabled(bool enabled)
    {
        UpcEanDeblurEnabled = enabled;
    }

    /// <summary>
    /// Sets whether the detection of misshaped 1D barcodes is enabled.
    /// </summary>
    /// <param name="enabled">True to enable decoding, false to disable it.</param>
    public void SetEnableMisshaped1DEnabled(bool enabled)
    {
        EnableMisshaped1DEnabled = enabled;
    }

    /// <summary>
    /// Sets whether to enable barcode thumbnail on result.
    /// </summary>
    /// <param name="enabled">True to enable barcode thumbnails, false to disable them.</param>
    public void SetBarcodeThumbnailOnResultEnabled(bool enabled)
    {
        BarcodeThumbnailOnResultEnabled = enabled;
    }

    public void SetARBarcodeThumbnailOnResultEnabled(bool enabled)
    {
        ARBarcodeThumbnailOnResultEnabled = enabled;
    }
    /// <summary>
    /// Sets the maximum number of results to be returned from barcode scanning.
    /// </summary>
    /// <param name="maximumResultsCount">The maximum number of results to return.</param>
    public void SetMaximumResultsCount(int maximumResultsCount)
    {
        MaximumResultsCount = maximumResultsCount;
    }

    public void SetResultDisappearanceDelayMs(int resultDisappearanceDelayMs)
    {
        ResultDisappearanceDelayMs = resultDisappearanceDelayMs;
    }

    public void SetARHeaderVerticalTextMargin(double verticalTextMargin)
    {
        HeaderARVerticalTextMargin = verticalTextMargin;
    }

    public void SetARHeaderHorizontalTextMargin(double horizontalTextMargin)
    {
        HeaderARHorizontalTextMargin = horizontalTextMargin;
    }


    public void SetARHeaderHeight(double headerHeight)
    {
        ARHeaderHeight = headerHeight;
    }

    public void SetARHeaderTextFormat(string headerTextFormat)
    {
        ARHeaderTextFormat = headerTextFormat;
    }

    public void SetARLocationTransitionSpeed(double transitionSpeed)
    {
        ARLocationTransitionSpeed = transitionSpeed;
    }

    public void SetARSelectedLocationLineWidth(double locationWidth)
    {
        ARSelectedLocationLineWidth = locationWidth;
    }

    public void SetARNonSelectedLocationLineWidth(double locationWidth)
    {
        ARNonSelectedLocationLineWidth = locationWidth;
    }

    public void SetARHeaderMaxTextHeight(double headerHeightMaxText)
    {
        ARHeaderMaxTextHeight = headerHeightMaxText;
    }

    public void SetARHeaderMinTextHeight(double headerHeightMinText)
    {
        ARHeaderMinTextHeight = headerHeightMinText;
    }


    public void SetScanningIndicatorAnimationMode(int animationMode)
    {
        ScanningIndicatorAnimationMode = animationMode;
    }

    /// <summary>
    /// Sets the delay in milliseconds for considering duplicate barcodes during scanning.
    /// </summary>
    /// <param name="duplicatesDelayMs">The delay in milliseconds for duplicate detection.</param>
    //public void SetDuplicatesDelayMs(int duplicatesDelayMs)
    //{
    //    DuplicatesDelayMs = duplicatesDelayMs;
    //}

    /// <summary>
    /// Sets whether a specific barcode type is enabled.
    /// </summary>
    /// <param name="barcodeType">The barcode type to enable or disable.</param>
    /// <param name="enabled">True to enable the barcode type, false to disable it.</param>
    public void SetBarcodeTypeEnabled(BarcodeType barcodeType, bool enabled)
    {
        Handler?.Invoke(nameof(BarkoderView.SetBarcodeTypeEnabledRequested), new BarcodeTypeEventArgs(barcodeType, enabled));

        for (int i = 0; i < BarcodeTypes.Count; i++)
        {
            if (BarcodeTypes[i].BarcodeType == barcodeType)
            {
                BarcodeTypes[i] = new BarcodeTypeEventArgs(barcodeType, enabled);
            }
        }
    }

    /// <summary>
    /// Sets whether Vehicle Identification Number (VIN) restrictions are enabled.
    /// </summary>
    /// <param name="enabled">True to enable VIN restrictions, false to disable them.</param>
    public void SetEnableVINRestrictions(bool enabled)
    {
        VINRestrictionsEnabled = enabled;
    }


    public void SetARDoubleTapToFreezeEnabled(bool enabled)
    {
        ARDoubleTapToFreezeEnabled = enabled;
    }

    /// <summary>
    /// Sets the threshold between duplicate scans.
    /// </summary>
    /// <param name="thresholdBetweenDuplicatesScans">The threshold between duplicate scans in milliseconds.</param>
    public void SetThresholdBetweenDuplicatesScans(int thresholdBetweenDuplicatesScans)
    {
        ThresholdBetweenDuplicatesScans = thresholdBetweenDuplicatesScans;
    }


    public void SetEnableComposite(int enableComposite)
    {
        EnableComposite = enableComposite;
    }

    /// <summary>
    /// Checks if a specific barcode type is enabled.
    /// </summary>
    /// <param name="barcode">The barcode type to check.</param>
    /// <returns>True if the barcode type is enabled; otherwise, false.</returns>
    public bool IsBarcodeTypeEnabled(BarcodeType barcode)
    {
        foreach (var barcodeType in BarcodeTypes)
        {
            if (barcodeType.BarcodeType == barcode)
            {
                return barcodeType.Enabled;
            }
        }

        return false;
    }

    /// <summary>
    /// Sets the length range for the specified barcode type.
    /// </summary>
    /// <param name="barcodeType">The type of barcode.</param>
    /// <param name="min">The minimum length of the barcode.</param>
    /// <param name="max">The maximum length of the barcode.</param>
    public void SetBarcodeTypeLengthRange(BarcodeType barcodeType, int min, int max)
    {
        switch (barcodeType)
        {
            case BarcodeType.Code128:
            case BarcodeType.Code93:
            case BarcodeType.Code39:
            case BarcodeType.Codabar:
            case BarcodeType.Code11:
            case BarcodeType.Msi:
                {
                    Handler?.Invoke(nameof(BarkoderView.SetBarcodeTypeLengthRangeRequested), new BarcodeRangeEventArg(barcodeType, min, max));
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    /// <summary>
    /// Configures the Barkoder functionality based on the provided configuration.
    /// </summary>
    /// <param name="BarkoderConfig">The configuration parameters for the Barkoder.</param>
    public void ConfigureBarkoder(BarkoderConfig BarkoderConfig)
    {
        string configAsJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(BarkoderConfig);

        Handler?.Invoke(nameof(BarkoderView.ConfigureBarkoderRequested), configAsJsonString);
    }

}

public class BarcodeTypeEventArgs : EventArgs
{
    public BarcodeType BarcodeType { get; }
    public bool Enabled { get; }

    public BarcodeTypeEventArgs(BarcodeType barcodeType, bool enabled)
    {
        BarcodeType = barcodeType;
        Enabled = enabled;
    }
}

public class BarcodeRangeEventArg : EventArgs
{
    public BarcodeType BarcodeType { get; }
    public int Min { get; }
    public int Max { get; }

    public BarcodeRangeEventArg(BarcodeType barcodeType, int min, int max)
    {
        BarcodeType = barcodeType;
        Min = min;
        Max = max;
    }
}