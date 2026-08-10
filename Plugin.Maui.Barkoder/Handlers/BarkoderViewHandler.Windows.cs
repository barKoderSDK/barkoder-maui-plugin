#if WINDOWS
using Microsoft.Maui.Handlers;
using global::Barkoder.Windows;
using global::Barkoder.Windows.Config;
using global::Barkoder.Windows.Decoder;
using global::Barkoder.Windows.Models;
using global::Barkoder.Windows.Camera;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Reflection;
using Plugin.Maui.Barkoder.Handlers;
using Plugin.Maui.Barkoder.Interfaces;
using Microsoft.UI.Xaml.Media.Imaging;
using global::Barkoder.Windows.UI;

using CommonBarkoderResolution = Plugin.Maui.Barkoder.Enums.BarkoderResolution;
using WinBarkoderResolution = global::Barkoder.Windows.Models.BarkoderResolution;

using CommonARMode = Plugin.Maui.Barkoder.Enums.BarkoderARMode;
using WinARMode = global::Barkoder.Windows.Models.BarkoderARMode;

using CommonARHeaderShowMode = Plugin.Maui.Barkoder.Enums.BarkoderARHeaderShowMode;
using WinARHeaderShowMode = global::Barkoder.Windows.Models.BarkoderARHeaderShowMode;

using CommonARLocationType = Plugin.Maui.Barkoder.Enums.BarkoderARLocationType;
using WinARLocationType = global::Barkoder.Windows.Models.BarkoderARLocationType;
using CommonRoiCenterMark = Plugin.Maui.Barkoder.Enums.BarkoderRoiCenterMark;
using WinRoiCenterMark = global::Barkoder.Windows.Models.BarkoderRoiCenterMark;

using CommonAROverlayRefresh = Plugin.Maui.Barkoder.Enums.BarkoderAROverlayRefresh;
using WinAROverlayRefresh = global::Barkoder.Windows.Models.BarkoderAROverlayRefresh;

using CommonBarkoderCameraPosition = Plugin.Maui.Barkoder.Enums.BarkoderCameraPosition;
using WinBarkoderCameraPosition = global::Barkoder.Windows.Models.BarkoderCameraPosition;

using CommonDecodingSpeed = Plugin.Maui.Barkoder.Enums.DecodingSpeed;
using WinDecodingSpeed = global::Barkoder.Windows.Models.DecodingSpeed;

using CommonFormattingType = Plugin.Maui.Barkoder.Enums.FormattingType;
using WinFormattingType = global::Barkoder.Windows.Models.FormattingType;

using CommonMsiChecksumType = Plugin.Maui.Barkoder.Enums.MsiChecksumType;
using WinMsiChecksumType = global::Barkoder.Windows.Models.MsiChecksumType;

using CommonCode11ChecksumType = Plugin.Maui.Barkoder.Enums.Code11ChecksumType;
using WinCode11ChecksumType = global::Barkoder.Windows.Models.Code11ChecksumType;

using CommonCode39ChecksumType = Plugin.Maui.Barkoder.Enums.Code39ChecksumType;
using WinCode39ChecksumType = global::Barkoder.Windows.Models.Code39ChecksumType;

using PluginBarcodeResult = Plugin.Maui.Barkoder.Handlers.BarcodeResult;
using PluginImageData = Plugin.Maui.Barkoder.Handlers.ImageData;
using PluginBarcodeLocation = Plugin.Maui.Barkoder.Handlers.BarcodeLocation;
using PluginBarcodePoint = Plugin.Maui.Barkoder.Handlers.BarcodePoint;

using WinBarcodeResult = global::Barkoder.Windows.Models.BarcodeResult;
using WinDecoderType = global::Barkoder.Windows.Models.DecoderType;
using WinBarkoderView = global::Barkoder.Windows.BarkoderView;

namespace Plugin.Maui.Barkoder.Controls;

public partial class BarkoderViewHandler : ViewHandler<BarkoderView, Microsoft.UI.Xaml.FrameworkElement>
{
    private WinBarkoderView? _winBarkoderView;
    private BarkoderConfig? _config;
    private Microsoft.UI.Xaml.Controls.Image? _previewImage;
    private WriteableBitmap? _bitmap;
    private int _bitmapWidth;
    private int _bitmapHeight;
    private long _lastPreviewTick;
    private double _viewWidth;
    private double _viewHeight;

    /// <summary>
    /// Push the current _config to the native decoder so settings take effect during live scanning.
    /// </summary>
    private void ApplyConfigLive()
    {
        if (_winBarkoderView != null && _config != null)
        {
            _winBarkoderView.Config = _config;
        }
    }

    /// <summary>
    /// Clear the camera preview to black.
    /// </summary>
    private void ClearPreviewToBlack()
    {
        if (_previewImage == null) return;
        _previewImage.DispatcherQueue.TryEnqueue(() =>
        {
            try
            {
                _previewImage.Source = null;
                _bitmap = null;
                _bitmapWidth = 0;
                _bitmapHeight = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Barkoder] ClearPreviewToBlack error: {ex.Message}");
            }
        });
    }

    protected override Microsoft.UI.Xaml.FrameworkElement CreatePlatformView()
    {
        var grid = new Microsoft.UI.Xaml.Controls.Grid();
        _previewImage = new Microsoft.UI.Xaml.Controls.Image
        {
            Stretch = Microsoft.UI.Xaml.Media.Stretch.UniformToFill,
            HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
            VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center
        };
        grid.Children.Add(_previewImage);

        // Clip the grid to its bounds so the camera image is cropped, not stretched
        // Also track view dimensions for ROI coordinate mapping
        grid.SizeChanged += (s, e) =>
        {
            _viewWidth = e.NewSize.Width;
            _viewHeight = e.NewSize.Height;
            grid.Clip = new Microsoft.UI.Xaml.Media.RectangleGeometry
            {
                Rect = new Windows.Foundation.Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
            };
        };

        // Handle taps for AR barcode selection
        grid.Tapped += OnGridTapped;
        grid.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(
            Microsoft.UI.Colors.Transparent); // Needed for hit testing

        return grid;
    }

    private void OnGridTapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        if (_winBarkoderView == null || _config?.ARConfig.ARMode == WinARMode.Off)
            return;

        var arMode = _config?.ARConfig.ARMode;
        const int matchFilterModeValue = 4;
        if (arMode == WinARMode.NonInteractive || (int?)arMode == matchFilterModeValue)
            return;

        var grid = sender as Microsoft.UI.Xaml.Controls.Grid;
        if (grid == null || _previewImage == null) return;

        var position = e.GetPosition(_previewImage);
        float tapX = (float)position.X;
        float tapY = (float)position.Y;

        // Convert view coordinates to frame pixel coordinates
        float viewWidth = (float)_previewImage.ActualWidth;
        float viewHeight = (float)_previewImage.ActualHeight;
        int frameW = _winBarkoderView.LastFrameWidth;
        int frameH = _winBarkoderView.LastFrameHeight;

        if (viewWidth <= 0 || viewHeight <= 0 || frameW <= 0 || frameH <= 0)
            return;

        // Compute mapping for UniformToFill (account for cropping)
        float scale = Math.Max(viewWidth / frameW, viewHeight / frameH);
        float visibleW = viewWidth / scale;
        float visibleH = viewHeight / scale;
        float cropX = (frameW - visibleW) / 2f;
        float cropY = (frameH - visibleH) / 2f;
        float frameX = cropX + tapX / viewWidth * visibleW;
        float frameY = cropY + tapY / viewHeight * visibleH;

        frameX = Math.Clamp(frameX, 0, frameW - 1);
        frameY = Math.Clamp(frameY, 0, frameH - 1);

        try
        {
            _winBarkoderView.ARToggleSelectionAt(frameX, frameY);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Barkoder] ARToggleSelectionAt error: {ex.Message}");
        }
    }

    private void RenderFrame(byte[] bgra8Data, int width, int height)
    {
        if (_previewImage == null) return;

        // Throttle to ~30fps
        var now = Environment.TickCount64;
        if (now - _lastPreviewTick < 33) return;
        _lastPreviewTick = now;

        // Get overlay data (ROI, locations, AR) from the Windows SDK
        var overlay = _winBarkoderView?.BuildOverlay();

        // Adjust ROI coordinates from view-space to frame-space for UniformToFill cropping
        if (overlay?.Roi != null && _viewWidth > 0 && _viewHeight > 0 && width > 0 && height > 0)
        {
            float scale = Math.Max((float)_viewWidth / width, (float)_viewHeight / height);
            float visibleW = (float)_viewWidth / scale;
            float visibleH = (float)_viewHeight / scale;
            float cropX = (width - visibleW) / 2f;
            float cropY = (height - visibleH) / 2f;

            var roi = overlay.Roi.Value;
            // ROI percentages are relative to the view; remap to full frame coordinates
            float newLeft = (cropX + roi.Left / 100f * visibleW) / width * 100f;
            float newTop = (cropY + roi.Top / 100f * visibleH) / height * 100f;
            float newWidth = roi.Width / 100f * visibleW / width * 100f;
            float newHeight = roi.Height / 100f * visibleH / height * 100f;
            overlay.Roi = (newLeft, newTop, newWidth, newHeight);
        }

        var pixels = CameraPreviewHandler.ApplyOverlay(bgra8Data, width, height, overlay);

        // Must run on UI thread
        _previewImage.DispatcherQueue.TryEnqueue(() =>
        {
            try
            {
                if (_bitmap == null || _bitmapWidth != width || _bitmapHeight != height)
                {
                    _bitmapWidth = width;
                    _bitmapHeight = height;
                    _bitmap = new WriteableBitmap(width, height);
                    _previewImage.Source = _bitmap;
                }

                using var stream = _bitmap.PixelBuffer.AsStream();
                stream.Position = 0;
                stream.Write(pixels, 0, Math.Min(pixels.Length, width * height * 4));
                _bitmap.Invalidate();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Barkoder] RenderFrame error: {ex.Message}");
            }
        });
    }

    public float GetCurrentZoomFactor()
    {
        return -1f; // TODO: implement if camera zoom tracking needed
    }

    private void EnsureBarkoderView()
    {
        if (_winBarkoderView == null)
        {
            _winBarkoderView = new global::Barkoder.Windows.BarkoderView();
        }
    }

    private void EnsureConfig(BarkoderView view)
    {
        if (_config == null && !string.IsNullOrEmpty(view.LicenseKey))
        {
            _config = new BarkoderConfig(view.LicenseKey);
            EnsureBarkoderView();
            _winBarkoderView!.Config = _config;
        }
    }

    // ---- Property Mappers ----

    private static void MapLicenseKey(BarkoderViewHandler handler, BarkoderView view)
    {
        if (!string.IsNullOrEmpty(view.LicenseKey))
        {
            handler._config = new BarkoderConfig(view.LicenseKey);
            handler.EnsureBarkoderView();
            handler._winBarkoderView!.Config = handler._config;

            // Set DeviceId now that config is initialized
            try
            {
                view.DeviceId = handler._winBarkoderView.GetDeviceId() ?? "";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"DeviceId error: {ex.Message}");
            }
        }
    }

    private static void MapRegionOfInterestVisible(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            handler._config.RegionOfInterestVisible = true;
        }
    }

    private static void MapIsFlashAvailable(BarkoderViewHandler handler, BarkoderView view) { }

    private static void MapMaxZoomFactor(BarkoderViewHandler handler, BarkoderView view) { }

    private static void MapInitCameraProperties(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.EnsureBarkoderView();
        handler._winBarkoderView?.IsFlashAvailable((available) =>
        {
            view.IsFlashAvailable = available;
        });
        handler._winBarkoderView?.GetMaxZoomFactor((maxZoom) =>
        {
            view.MaxZoomFactor = maxZoom;
        });
    }

    private static void MapRoiLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.RoiLineColorHex = handler._config.RoiLineColor;
        }
    }

    private static void MapLocationLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.LocationLineColorHex = handler._config.LocationLineColor;
        }
    }

    private static void MapARNonSelectedLocationLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARNonSelectedLocationLineColorHex = handler._config.ARConfig.NonSelectedLocationColor;
        }
    }

    private static void MapARImageResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARImageResultEnabled = handler._config.ARConfig.ImageResultEnabled;
        }
    }

    private static void MapARBarcodeThumbnailOnResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARBarcodeThumbnailOnResultEnabled = handler._config.ARConfig.BarcodeThumbnailOnResult;
        }
    }

    private static void MapARSelectedLocationLineColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARSelectedLocationLineColorHex = handler._config.ARConfig.SelectedLocationColor;
        }
    }

    private static void MapARSelectedHeaderTextColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARSelectedHeaderTextColorHex = handler._config.ARConfig.HeaderTextColorSelected;
        }
    }

    private static void MapARNonSelectedHeaderTextColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARNonSelectedHeaderTextColorHex = handler._config.ARConfig.HeaderTextColorNonSelected;
        }
    }

    private static void MapRoiOverlayBackgroundColorHex(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null && !string.IsNullOrEmpty(view.RoiOverlayBackgroundColorHex))
        {
            handler._config.RoiOverlayBackgroundColor = view.RoiOverlayBackgroundColorHex;
        }
    }

    private static void MapEncodingCharacterSet(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.EncodingCharacterSet = handler._config.DecoderConfig.EncodingCharacterSet;
        }
    }

    private static void MapARHeaderTextFormat(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARHeaderTextFormat = handler._config.ARConfig.HeaderTextFormat;
        }
    }

    private static void MapLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.LocationLineWidth = handler._config.LocationLineWidth;
        }
    }

    private static void MapARSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARSelectedLocationLineWidth = handler._config.ARConfig.SelectedLocationLineWidth;
        }
    }

    private static void MapARNonSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARNonSelectedLocationLineWidth = handler._config.ARConfig.NonSelectedLocationLineWidth;
        }
    }

    private static void MapARLocationTransitionSpeed(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARLocationTransitionSpeed = handler._config.ARConfig.LocationTransitionSpeed;
        }
    }

    private static void MapImageResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ImageResultEnabled = handler._config.ImageResultEnabled;
        }
    }

    private static void MapLocationInImageResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.LocationInImageResultEnabled = handler._config.LocationInImageResultEnabled;
        }
    }

    private static void MapLocationInPreviewEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.LocationInPreviewEnabled = handler._config.LocationInPreviewEnabled;
        }
    }

    private static void MapPinchToZoomEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.PinchToZoomEnabled = handler._config.PinchToZoomEnabled;
        }
    }

    private static void MapBeepOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.BeepOnSuccessEnabled = handler._config.BeepOnSuccessEnabled;
        }
    }

    private static void MapVibrateOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.VibrateOnSuccessEnabled = handler._config.VibrateOnSuccessEnabled;
        }
    }

    private static void MapCloseSessionOnResultEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            handler._config.CloseSessionOnResultEnabled = view.CloseSessionOnResultEnabled;
        }
    }

    private static void MapARContinueScanningOnLimit(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARContinueScanningOnLimit = handler._config.ARConfig.ContinueScanningOnLimit;
        }
    }

    private static void MapAREmitResultsAtSessionEndOnly(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.AREmitResultsAtSessionEndOnly = handler._config.ARConfig.EmitResultsAtSessionEndOnly;
        }
    }

    private static void MapVersion(BarkoderViewHandler handler, BarkoderView view)
    {
        view.Version = "Windows-1.0.0";
    }

    private static void MapLibVersion(BarkoderViewHandler handler, BarkoderView view)
    {
        view.LibVersion = global::Barkoder.Windows.BarkoderView.GetLibVersion();
    }

    private static void MapDeviceId(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._winBarkoderView != null)
        {
            try
            {
                view.DeviceId = handler._winBarkoderView.GetDeviceId() ?? "";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"MapDeviceId error: {ex.Message}");
                view.DeviceId = "";
            }
        }
    }

    private static void MapBarkoderResolution(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.BarkoderResolution = handler._config.BarkoderResolution switch
            {
                WinBarkoderResolution.HD => CommonBarkoderResolution.HD,
                WinBarkoderResolution.FHD => CommonBarkoderResolution.FHD,
                WinBarkoderResolution.UHD => CommonBarkoderResolution.UHD,
                _ => CommonBarkoderResolution.HD
            };
        }
    }

    private static void MapBarkoderARMode(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            var mode = (int)handler._config.ARConfig.ARMode;
            view.BarkoderARMode = mode switch
            {
                0 => CommonARMode.OFF,
                1 => CommonARMode.InteractiveDisabled,
                2 => CommonARMode.InteractiveEnabled,
                3 => CommonARMode.NonInteractive,
                4 => CommonARMode.MatchFilter,
                _ => CommonARMode.OFF
            };
        }
    }

    private static void MapBarkoderARHeaderShowMode(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.BarkoderARHeaderShowMode = handler._config.ARConfig.HeaderShowMode switch
            {
                WinARHeaderShowMode.Never => CommonARHeaderShowMode.NEVER,
                WinARHeaderShowMode.Always => CommonARHeaderShowMode.ALWAYS,
                WinARHeaderShowMode.OnSelected => CommonARHeaderShowMode.ONSELECTED,
                _ => CommonARHeaderShowMode.NEVER
            };
        }
    }

    private static void MapBarkoderARLocationType(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.BarkoderARLocationType = handler._config.ARConfig.LocationType switch
            {
                WinARLocationType.None => CommonARLocationType.NONE,
                WinARLocationType.Tight => CommonARLocationType.TIGHT,
                WinARLocationType.BoundingBox => CommonARLocationType.BOUNDINGBOX,
                _ => CommonARLocationType.NONE
            };
        }
    }

    private static void MapDecodingSpeed(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.DecodingSpeed = handler._config.DecoderConfig.DecodingSpeed switch
            {
                WinDecodingSpeed.Fast => CommonDecodingSpeed.Fast,
                WinDecodingSpeed.Normal => CommonDecodingSpeed.Normal,
                WinDecodingSpeed.Slow => CommonDecodingSpeed.Slow,
                WinDecodingSpeed.Rigorous => CommonDecodingSpeed.Rigorous,
                _ => CommonDecodingSpeed.Normal
            };
        }
    }

    private static void MapBarkoderRoiCenterMark(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.BarkoderRoiCenterMark = handler._config.DecoderConfig.RoiCenterMark switch
            {
                WinRoiCenterMark.None => CommonRoiCenterMark.NONE,
                WinRoiCenterMark.Point => CommonRoiCenterMark.POINT,
                WinRoiCenterMark.Crosshair => CommonRoiCenterMark.CROSSHAIR,
                _ => CommonRoiCenterMark.NONE
            };
        }
    }

    private static void MapFormattingType(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.FormattingType = ((int)handler._config.DecoderConfig.Formatting) switch
            {
                0 => CommonFormattingType.Disabled,
                1 => CommonFormattingType.Automatic,
                2 => CommonFormattingType.GS1,
                3 => CommonFormattingType.AAMVA,
                4 => CommonFormattingType.SADL,
                5 => CommonFormattingType.BCBP,
                _ => CommonFormattingType.Automatic
            };
        }
    }

    private static void MapCode39ChecksumType(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.Code39ChecksumType = handler._config.DecoderConfig.Code39Checksum switch
            {
                WinCode39ChecksumType.Disabled => CommonCode39ChecksumType.Disabled,
                WinCode39ChecksumType.Enabled => CommonCode39ChecksumType.Enabled,
                _ => CommonCode39ChecksumType.Disabled
            };
        }
    }

    private static void MapCode11ChecksumType(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.Code11ChecksumType = handler._config.DecoderConfig.Code11Checksum switch
            {
                WinCode11ChecksumType.Disabled => CommonCode11ChecksumType.Disabled,
                WinCode11ChecksumType.Single => CommonCode11ChecksumType.Single,
                WinCode11ChecksumType.Double => CommonCode11ChecksumType.Double,
                _ => CommonCode11ChecksumType.Disabled
            };
        }
    }

    private static void MapMsiChecksumType(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.MsiChecksumType = handler._config.DecoderConfig.MsiChecksum switch
            {
                WinMsiChecksumType.Disabled => CommonMsiChecksumType.Disabled,
                WinMsiChecksumType.Mod10 => CommonMsiChecksumType.Mod10,
                WinMsiChecksumType.Mod11 => CommonMsiChecksumType.Mod11,
                WinMsiChecksumType.Mod1010 => CommonMsiChecksumType.Mod1010,
                WinMsiChecksumType.Mod1110 => CommonMsiChecksumType.Mod1110,
                WinMsiChecksumType.Mod11IBM => CommonMsiChecksumType.Mod11IBM,
                WinMsiChecksumType.Mod1110IBM => CommonMsiChecksumType.Mod1110IBM,
                _ => CommonMsiChecksumType.Disabled
            };
        }
    }

    private static void MapIdDocumentMasterChecksumEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        // Windows SDK doesn't have a separate IDDocument master checksum property
    }

    private static void MapDatamatrixDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.DatamatrixDpmModeEnabled = handler._config.DecoderConfig.DataMatrixDpmMode != 0;
        }
    }

    private static void MapQRDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.QRDpmModeEnabled = handler._config.DecoderConfig.QrDpmMode != 0;
        }
    }

    private static void MapQRMicroDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.QRMIcroDpmModeEnabled = handler._config.DecoderConfig.QrMicroDpmMode != 0;
        }
    }

    private static void MapUpcEanDeblurEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.UpcEanDeblurEnabled = handler._config.DecoderConfig.UpcEanDeblur;
        }
    }

    private static void MapEnableMisshaped1DEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.EnableMisshaped1DEnabled = handler._config.DecoderConfig.EnableMisshaped1D;
        }
    }

    private static void MapBarcodeThumbnailOnResultEnabledProperty(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.BarcodeThumbnailOnResultEnabled = handler._config.BarcodeThumbnailOnResult;
        }
    }

    private static void MapMaximumResultsCount(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.MaximumResultsCount = handler._config.DecoderConfig.MaximumResultsCount;
        }
    }

    private static void MapARResultLimit(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARResultLimit = handler._config.ARConfig.ResultLimit;
        }
    }

    private static void MapResultDisappearanceDelayMs(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ResultDisappearanceDelayMs = handler._config.ARConfig.ResultDisappearanceDelayMs;
        }
    }

    private static void MapARHeaderVerticalTextMargin(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.HeaderARVerticalTextMargin = handler._config.ARConfig.HeaderVerticalTextMargin;
        }
    }

    private static void MapARHeaderHorizontalTextMargin(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.HeaderARHorizontalTextMargin = handler._config.ARConfig.HeaderHorizontalTextMargin;
        }
    }

    private static void MapARHeaderHeight(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARHeaderHeight = handler._config.ARConfig.HeaderHeight;
        }
    }

    private static void MapARHeaderMaxTextHeight(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARHeaderMaxTextHeight = handler._config.ARConfig.HeaderMaxTextHeight;
        }
    }

    private static void MapARHeaderMinTextHeight(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARHeaderMinTextHeight = handler._config.ARConfig.HeaderMinTextHeight;
        }
    }

    private static void MapDuplicatesDelayMs(BarkoderViewHandler handler, BarkoderView view)
    {
    }

    private static void MapScanningIndicatorAlwaysVisibleEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ScanningIndicatorAlwaysVisibleEnabled = handler._config.ScanningIndicatorAlwaysVisible;
        }
    }

    private static void MapScanningIndicatorAnimationMode(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ScanningIndicatorAnimationMode = (int)handler._config.ScanningIndicatorAnimation;
        }
    }

    private static void MapScanningIndicatorColor(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ScanningIndicatorLineColorHex = handler._config.ScanningIndicatorColor;
        }
    }

    private static void MapScanningIndicatorWidth(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ScanningIndicatorLineWidth = handler._config.ScanningIndicatorWidth;
        }
    }

    private static void MapEnableComposite(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.EnableComposite = handler._config.DecoderConfig.CompositeCodesEnabled ? 1 : 0;
        }
    }

    private static void MapVINRestrictionsEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.VINRestrictionsEnabled = handler._config.DecoderConfig.EnableVINRestrictions;
        }
    }

    private static void MapARDoubleTapToFreezeEnabled(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARDoubleTapToFreezeEnabled = handler._config.ARConfig.DoubleTapToFreezeEnabled;
        }
    }

    private static void MapARReturnOnlyMatchedResults(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            SetBoolPropertyIfAvailable(handler._config.ARConfig, "ReturnOnlyMatchedResults", view.ARReturnOnlyMatchedResults);
        }
    }

    private static void MapARDisplayOnlyMatchedResults(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.ARDisplayOnlyMatchedResults = GetBoolProperty(handler._config.ARConfig, "DisplayOnlyMatchedResults", false);
        }
    }

    private static void MapDecoderMatchFilter(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.DecoderMatchFilter = GetStringProperty(handler._config.DecoderConfig, "MatchFilter", string.Empty);
        }
    }

    private static void MapDecoderReturnOnlyMatchedResults(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.DecoderReturnOnlyMatchedResults = GetBoolProperty(handler._config.DecoderConfig, "ReturnOnlyMatchedResults", false);
        }
    }

    private static void MapThresholdBetweenDuplicatesScans(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            handler._config.ThresholdBetweenDuplicatesScans = view.ThresholdBetweenDuplicatesScans;
        }
    }

    private static void MapPowerSavingMode(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            view.PowerSavingMode = handler._config.PowerSavingMode;
        }
    }

    private static void MapRegionOfInterest(BarkoderViewHandler handler, BarkoderView view)
    {
        if (handler._config != null)
        {
            var roi = view.RegionOfInterest;
            try
            {
                handler._config.SetRegionOfInterest(roi.Item1, roi.Item2, roi.Item3, roi.Item4);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Barkoder] MapRegionOfInterest ignored invalid ROI {roi}: {ex.Message}");
            }
        }
    }

    // ---- Command Mappers ----

    private static void MapStartCamera(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler.EnsureBarkoderView();
        handler.EnsureConfig(view);
        if (handler._winBarkoderView != null)
        {
            handler._winBarkoderView.SetCameraProvider(new MediaCaptureCameraProvider());
            handler._winBarkoderView.SetPreviewFramesCallback((frameData, width, height) =>
            {
                handler.RenderFrame(frameData, width, height);
            });
            _ = handler._winBarkoderView.StartCameraAsync();
        }
    }

    private static void MapStartScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is IBarkoderDelegate barkoderDelegate)
        {
            handler.EnsureBarkoderView();
            handler.EnsureConfig(view);
            if (handler._winBarkoderView != null && handler._config != null)
            {
                System.Diagnostics.Debug.WriteLine("[Barkoder] MapStartScanning: starting...");

                // Re-apply config so the native decoder picks up all enabled barcode types
                // (settings like SetBarcodeTypeEnabled are applied to _config after initial creation)
                handler._winBarkoderView.Config = handler._config;

                var provider = new MediaCaptureCameraProvider();
                handler._winBarkoderView.SetCameraProvider(provider);

                // Set preview frames callback to render camera feed into the platform Image
                handler._winBarkoderView.SetPreviewFramesCallback((frameData, width, height) =>
                {
                    handler.RenderFrame(frameData, width, height);
                });

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await handler._winBarkoderView.StartScanning((results, thumbnails, resultImage) =>
                        {
                            System.Diagnostics.Debug.WriteLine($"[Barkoder] Got {results?.Length ?? 0} results");
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                var pluginResults = ConvertResults(results);
                                var pluginThumbnails = thumbnails ?? Array.Empty<ImageSource>();
                                barkoderDelegate.DidFinishScanning(pluginResults, pluginThumbnails, resultImage);
                            });
                        });
                        System.Diagnostics.Debug.WriteLine("[Barkoder] StartScanning completed successfully");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Barkoder] StartScanning FAILED: {ex}");
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await Application.Current!.MainPage!.DisplayAlert(
                                "Barkoder Error",
                                $"Failed to start scanning: {ex.Message}",
                                "OK");
                        });
                    }
                });
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[Barkoder] MapStartScanning: winBarkoderView={handler._winBarkoderView != null}, config={handler._config != null}");
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"[Barkoder] MapStartScanning: arg3 is not IBarkoderDelegate, arg3={arg3?.GetType().Name ?? "null"}");
        }
    }

    private static void MapScanImage(BarkoderViewHandler handler, BarkoderView view, object? arg)
    {
        var arguments = (dynamic)arg;
        string base64Image = arguments.base64Image;
        IBarkoderDelegate barkoderDelegate = arguments.barkoderDelegate;

        handler.EnsureBarkoderView();
        handler.EnsureConfig(view);

        if (handler._config != null && !string.IsNullOrEmpty(base64Image))
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(base64Image);

                    // Decode the image to raw BGRA8 pixels
                    int width, height;
                    byte[] bgra8;

                    using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
                    {
                        await stream.WriteAsync(imageBytes.AsBuffer());
                        stream.Seek(0);

                        var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                        width = (int)decoder.PixelWidth;
                        height = (int)decoder.PixelHeight;

                        var pixelData = await decoder.GetPixelDataAsync(
                            Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                            Windows.Graphics.Imaging.BitmapAlphaMode.Premultiplied,
                            new Windows.Graphics.Imaging.BitmapTransform(),
                            Windows.Graphics.Imaging.ExifOrientationMode.RespectExifOrientation,
                            Windows.Graphics.Imaging.ColorManagementMode.DoNotColorManage);
                        bgra8 = pixelData.DetachPixelData();
                    }

                    // Call ScanImage on the main thread so it captures a valid
                    // SynchronizationContext — otherwise DeliverScanImageResults
                    // falls through to the null-context branch that discards
                    // thumbnails and resultImage.
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await global::Barkoder.Windows.BarkoderView.ScanImage(bgra8, width, height, handler._config,
                                (results, thumbnails, resultImage) =>
                                {
                                    var pluginResults = ConvertResults(results);
                                    var pluginThumbnails = thumbnails ?? Array.Empty<ImageSource>();
                                    barkoderDelegate.DidFinishScanning(pluginResults, pluginThumbnails, resultImage);
                                });
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[Barkoder] ScanImage error: {ex.Message}");
                            barkoderDelegate.DidFinishScanning(Array.Empty<PluginBarcodeResult>(), Array.Empty<ImageSource>(), null);
                        }
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Barkoder] ScanImage error: {ex.Message}");
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        barkoderDelegate.DidFinishScanning(Array.Empty<PluginBarcodeResult>(), Array.Empty<ImageSource>(), null);
                    });
                }
            });
        }
    }

    private static void MapStopScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (handler._winBarkoderView != null)
        {
            _ = Task.Run(async () =>
            {
                await handler._winBarkoderView.StopScanning();
                handler.ClearPreviewToBlack();
            });
        }
    }

    private static void MapSelectVisibleBarcodes(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Not directly supported on Windows SDK
    }

    private static void MapConfigureCloseButton(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Button configuration is UI-level; Windows SDK handles this differently
    }

    private static void MapConfigureFlashButton(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Button configuration is UI-level; Windows SDK handles this differently
    }

    private static void MapConfigureZoomButton(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Button configuration is UI-level; Windows SDK handles this differently
    }

    private static void MapCaptureImage(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (handler._winBarkoderView != null)
        {
            _ = handler._winBarkoderView.CaptureImageAsync();
        }
    }

    private static void MapPauseScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (handler._winBarkoderView != null)
        {
            _ = handler._winBarkoderView.PauseScanning();
        }
    }

    private static void MapFreezeScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler._winBarkoderView?.FreezeScanning();
    }

    private static void MapUnfreezeScanning(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler._winBarkoderView?.UnfreezeScanning();
    }

    private static void MapResetARCache(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        handler._winBarkoderView?.ResetARSession();
    }

    private static void MapFlashEnable(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled)
        {
            handler._winBarkoderView?.SetFlashEnabled(enabled);
        }
    }

    private static void MapSetZoomFactor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is float zoomFactor)
        {
            handler._winBarkoderView?.SetZoomFactor(zoomFactor);
        }
    }

    private static void MapSetPinchToZoomEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.PinchToZoomEnabled = enabled;
        }
    }

    private static void MapSetRegionOfInterestVisible(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool visible && handler._config != null)
        {
            handler._config.RegionOfInterestVisible = visible;
        }
    }

    private static void MapSetRoiLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is float width && handler._config != null)
        {
            handler._config.RoiLineWidth = width;
        }
    }

    private static void MapSetVibrateOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.VibrateOnSuccessEnabled = enabled;
        }
    }

    private static void MapSetBeepOnSuccessEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.BeepOnSuccessEnabled = enabled;
        }
    }

    private static void MapSetLocationInImageResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.LocationInImageResultEnabled = enabled;
        }
    }

    private static void MapSetLocationInPreviewEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.LocationInPreviewEnabled = enabled;
        }
    }

    private static void MapSetImageResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.ImageResultEnabled = enabled;
        }
    }

    private static void MapSetCloseSessionOnResult(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.CloseSessionOnResultEnabled = enabled;
        }
    }

    private static void MapSetARContinueScanningOnLimit(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.ARConfig.ContinueScanningOnLimit = enabled;
        }
    }

    private static void MapSetAREmitResultsAtSessionEndOnly(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.ARConfig.EmitResultsAtSessionEndOnly = enabled;
        }
    }

    private static void MapSetRoiLineColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor && handler._config != null)
        {
            handler._config.RoiLineColor = NormalizeHexColor(hexColor);
        }
    }

    private static void MapSetLocationLineColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor && handler._config != null)
        {
            handler._config.LocationLineColor = NormalizeHexColor(hexColor);
        }
    }

    private static void MapSetARNonSelectedLocationColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor && handler._config != null)
        {
            handler._config.ARConfig.NonSelectedLocationColor = NormalizeHexColor(hexColor);
        }
    }

    private static void MapSetARImageResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.ARConfig.ImageResultEnabled = enabled;
        }
    }

    private static void MapSetARBarcodeThumbnailOnResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.ARConfig.BarcodeThumbnailOnResult = enabled;
        }
    }

    private static void MapSetARSelectedLocationColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor && handler._config != null)
        {
            handler._config.ARConfig.SelectedLocationColor = NormalizeHexColor(hexColor);
        }
    }

    private static void MapSetARSelectedHeaderTextColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor && handler._config != null)
        {
            handler._config.ARConfig.HeaderTextColorSelected = NormalizeHexColor(hexColor);
        }
    }

    private static void MapSetARNonSelectedHeaderTextColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor && handler._config != null)
        {
            handler._config.ARConfig.HeaderTextColorNonSelected = NormalizeHexColor(hexColor);
        }
    }

    private static void MapSetARSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is double width && handler._config != null)
        {
            handler._config.ARConfig.SelectedLocationLineWidth = (float)width;
        }
    }

    private static void MapSetARNonSelectedLocationLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is double width && handler._config != null)
        {
            handler._config.ARConfig.NonSelectedLocationLineWidth = (float)width;
        }
    }

    private static void MapSetARLocationTransitionSpeed(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is double speed && handler._config != null)
        {
            handler._config.ARConfig.LocationTransitionSpeed = (float)speed;
        }
    }

    private static void MapSetRoiOverlayBackgroundColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor && handler._config != null)
        {
            handler._config.RoiOverlayBackgroundColor = NormalizeHexColor(hexColor);
        }
    }

    private static void MapSetLocationLineWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is float width && handler._config != null)
        {
            handler._config.LocationLineWidth = width;
        }
    }

    private static void MapSetRegionOfInterest(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is int[] roi && handler._config != null)
        {
            try
            {
                handler._config.SetRegionOfInterest(roi[0], roi[1], roi[2], roi[3]);
                handler.ApplyConfigLive();
            }
            catch { }
        }
    }

    private static void MapSetBarkoderResolution(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonBarkoderResolution res && handler._config != null)
        {
            handler._config.BarkoderResolution = res switch
            {
                CommonBarkoderResolution.HD => WinBarkoderResolution.HD,
                CommonBarkoderResolution.FHD => WinBarkoderResolution.FHD,
                CommonBarkoderResolution.UHD => WinBarkoderResolution.UHD,
                _ => WinBarkoderResolution.HD
            };
        }
    }

    private static void MapSetBarkoderARMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonARMode mode && handler._config != null)
        {
            handler._config.ARConfig.ARMode = mode switch
            {
                CommonARMode.OFF => WinARMode.Off,
                CommonARMode.InteractiveDisabled => WinARMode.InteractiveDisabled,
                CommonARMode.InteractiveEnabled => WinARMode.InteractiveEnabled,
                CommonARMode.NonInteractive => WinARMode.NonInteractive,
                CommonARMode.MatchFilter => (WinARMode)4,
                _ => WinARMode.Off
            };
        }
    }

    private static void MapSetBarkoderARHeaderShowMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonARHeaderShowMode mode && handler._config != null)
        {
            handler._config.ARConfig.HeaderShowMode = mode switch
            {
                CommonARHeaderShowMode.NEVER => WinARHeaderShowMode.Never,
                CommonARHeaderShowMode.ALWAYS => WinARHeaderShowMode.Always,
                CommonARHeaderShowMode.ONSELECTED => WinARHeaderShowMode.OnSelected,
                _ => WinARHeaderShowMode.Never
            };
        }
    }

    private static void MapSetBarkoderARLocationType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonARLocationType type && handler._config != null)
        {
            handler._config.ARConfig.LocationType = type switch
            {
                CommonARLocationType.NONE => WinARLocationType.None,
                CommonARLocationType.TIGHT => WinARLocationType.Tight,
                CommonARLocationType.BOUNDINGBOX => WinARLocationType.BoundingBox,
                _ => WinARLocationType.None
            };
        }
    }

    private static void MapSetBarkoderARoverlayRefresh(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonAROverlayRefresh refresh && handler._config != null)
        {
            handler._config.ARConfig.OverlayRefresh = refresh switch
            {
                CommonAROverlayRefresh.SMOOTH => WinAROverlayRefresh.Smooth,
                CommonAROverlayRefresh.NORMAL => WinAROverlayRefresh.Normal,
                _ => WinAROverlayRefresh.Smooth
            };
        }
    }

    private static void MapSetDecodingSpeed(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonDecodingSpeed speed && handler._config != null)
        {
            handler._config.DecoderConfig.DecodingSpeed = speed switch
            {
                CommonDecodingSpeed.Fast => WinDecodingSpeed.Fast,
                CommonDecodingSpeed.Normal => WinDecodingSpeed.Normal,
                CommonDecodingSpeed.Slow => WinDecodingSpeed.Slow,
                CommonDecodingSpeed.Rigorous => WinDecodingSpeed.Rigorous,
                _ => WinDecodingSpeed.Normal
            };
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetBarkoderRoiCenterMark(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonRoiCenterMark roiCenterMark && handler._config != null)
        {
            handler._config.DecoderConfig.RoiCenterMark = roiCenterMark switch
            {
                CommonRoiCenterMark.NONE => WinRoiCenterMark.None,
                CommonRoiCenterMark.POINT => WinRoiCenterMark.Point,
                CommonRoiCenterMark.CROSSHAIR => WinRoiCenterMark.Crosshair,
                _ => WinRoiCenterMark.None
            };
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetFormattingType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonFormattingType fmt && handler._config != null)
        {
            handler._config.DecoderConfig.Formatting = (WinFormattingType)(fmt switch
            {
                CommonFormattingType.Disabled => 0,
                CommonFormattingType.Automatic => 1,
                CommonFormattingType.GS1 => 2,
                CommonFormattingType.AAMVA => 3,
                CommonFormattingType.SADL => 4,
                CommonFormattingType.BCBP => 5,
                _ => 1
            });
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetCode39ChecksumType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonCode39ChecksumType cst && handler._config != null)
        {
            handler._config.DecoderConfig.Code39Checksum = cst switch
            {
                CommonCode39ChecksumType.Disabled => WinCode39ChecksumType.Disabled,
                CommonCode39ChecksumType.Enabled => WinCode39ChecksumType.Enabled,
                _ => WinCode39ChecksumType.Disabled
            };
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetCode11ChecksumType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonCode11ChecksumType cst && handler._config != null)
        {
            handler._config.DecoderConfig.Code11Checksum = cst switch
            {
                CommonCode11ChecksumType.Disabled => WinCode11ChecksumType.Disabled,
                CommonCode11ChecksumType.Single => WinCode11ChecksumType.Single,
                CommonCode11ChecksumType.Double => WinCode11ChecksumType.Double,
                _ => WinCode11ChecksumType.Disabled
            };
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetMsiChecksumType(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonMsiChecksumType cst && handler._config != null)
        {
            handler._config.DecoderConfig.MsiChecksum = cst switch
            {
                CommonMsiChecksumType.Disabled => WinMsiChecksumType.Disabled,
                CommonMsiChecksumType.Mod10 => WinMsiChecksumType.Mod10,
                CommonMsiChecksumType.Mod11 => WinMsiChecksumType.Mod11,
                CommonMsiChecksumType.Mod1010 => WinMsiChecksumType.Mod1010,
                CommonMsiChecksumType.Mod1110 => WinMsiChecksumType.Mod1110,
                CommonMsiChecksumType.Mod11IBM => WinMsiChecksumType.Mod11IBM,
                CommonMsiChecksumType.Mod1110IBM => WinMsiChecksumType.Mod1110IBM,
                _ => WinMsiChecksumType.Disabled
            };
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetEncodingCharacterSet(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string charset && handler._config != null)
        {
            handler._config.DecoderConfig.EncodingCharacterSet = charset;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetARHeaderTextFormat(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string format && handler._config != null)
        {
            handler._config.ARConfig.HeaderTextFormat = format;
        }
    }

    private static void MapSetIdDocumentMasterChecksumEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Windows SDK handles ID document settings through the decoder config
    }

    private static void MapSetDatamatrixDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.DecoderConfig.DataMatrixDpmMode = enabled ? 1 : 0;
            view.IsDatamatrixDpmModeEnabled = enabled;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetQRDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.DecoderConfig.QrDpmMode = enabled ? 1 : 0;
            view.IsQRDpmModeEnabled = enabled;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetShowDuplicatesLocation(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.ShowDuplicateLocations = enabled;
        }
    }

    private static void MapSetQRMicroDpmModeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.DecoderConfig.QrMicroDpmMode = enabled ? 1 : 0;
            view.IsQRMicroDpmModeEnabled = enabled;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetUpcEanDeblurEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.DecoderConfig.UpcEanDeblur = enabled;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetEnableMisshaped1DEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.DecoderConfig.EnableMisshaped1D = enabled;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetBarcodeThumbnailOnResultEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.BarcodeThumbnailOnResult = enabled;
        }
    }

    private static void MapSetMaximumResultsCount(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is int count && handler._config != null)
        {
            handler._config.DecoderConfig.MaximumResultsCount = count;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetARResultLimit(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is int limit && handler._config != null)
        {
            handler._config.ARConfig.ResultLimit = limit;
        }
    }

    private static void MapSetResultDisappearanceDelayMs(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is int delay && handler._config != null)
        {
            handler._config.ARConfig.ResultDisappearanceDelayMs = delay;
        }
    }

    private static void MapSetARHeaderVerticalTextMargin(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is double margin && handler._config != null)
        {
            handler._config.ARConfig.HeaderVerticalTextMargin = (float)margin;
        }
    }

    private static void MapSetARHeaderHorizontalTextMargin(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is double margin && handler._config != null)
        {
            handler._config.ARConfig.HeaderHorizontalTextMargin = (float)margin;
        }
    }

    private static void MapSetARHeaderHeight(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is double height && handler._config != null)
        {
            handler._config.ARConfig.HeaderHeight = (float)height;
        }
    }

    private static void MapSetARHeaderMaxTextHeight(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is double height && handler._config != null)
        {
            handler._config.ARConfig.HeaderMaxTextHeight = (float)height;
        }
    }

    private static void MapSetARHeaderMinTextHeight(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is double height && handler._config != null)
        {
            handler._config.ARConfig.HeaderMinTextHeight = (float)height;
        }
    }

    private static void MapSetDuplicatesDelayMs(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is int delay && handler._config != null)
        {
            handler._config.DecoderConfig.DuplicatesDelayMs = delay;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetScanningIndicatorAnimationMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is int mode && handler._config != null)
        {
            handler._config.ScanningIndicatorAnimation = (global::Barkoder.Windows.Models.ScanningIndicatorAnimation)mode;
        }
    }

    private static void MapSetDynamicExposure(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Not supported on Windows
    }

    private static void MapSetCamera(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is CommonBarkoderCameraPosition pos && handler._winBarkoderView != null)
        {
            var winPos = pos switch
            {
                CommonBarkoderCameraPosition.BACK => WinBarkoderCameraPosition.Back,
                CommonBarkoderCameraPosition.FRONT => WinBarkoderCameraPosition.Front,
                _ => WinBarkoderCameraPosition.Back
            };
            _ = handler._winBarkoderView.SetCamera(winPos);
        }
    }

    private static void MapSetCentricFocusAndExposure(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Not supported on Windows
    }

    private static void MapSetUPCE1expandToUPCA(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.DecoderConfig.UpcE1ExpandToUPCA = enabled;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetUPCEexpandToUPCA(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.DecoderConfig.UpcEExpandToUPCA = enabled;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetVideoStabilization(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Not supported on Windows
    }

    private static void MapSetScanningIndicatorAlwaysVisibleEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.ScanningIndicatorAlwaysVisible = enabled;
        }
    }

    private static void MapSetScanningIndicatorColor(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string hexColor && handler._config != null)
        {
            handler._config.ScanningIndicatorColor = NormalizeHexColor(hexColor);
        }
    }

    private static void MapSetScanningIndicatorWidth(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is float width && handler._config != null)
        {
            handler._config.ScanningIndicatorWidth = width;
        }
    }

    private static void MapSetEnableComposite(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is int val && handler._config != null)
        {
            handler._config.DecoderConfig.CompositeCodesEnabled = val != 0;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetBarcodeTypeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is BarcodeTypeEventArgs args && handler._config != null)
        {
            var winType = MapBarcodeTypeToWin(args.BarcodeType);
            if (winType != null)
            {
                if (args.Enabled)
                    handler._config.DecoderConfig.EnableDecoder(winType.Value);
                else
                    handler._config.DecoderConfig.DisableDecoder(winType.Value);
                handler.ApplyConfigLive();
            }
        }
    }

    private static void MapSetEnableVINRestrictions(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.DecoderConfig.EnableVINRestrictions = enabled;
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetDoubleTapToFreezeEnabled(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            handler._config.ARConfig.DoubleTapToFreezeEnabled = enabled;
        }
    }

    private static void MapSetARReturnOnlyMatchedResults(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            SetBoolPropertyIfAvailable(handler._config.ARConfig, "ReturnOnlyMatchedResults", enabled);
        }
    }

    private static void MapSetARDisplayOnlyMatchedResults(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            SetBoolPropertyIfAvailable(handler._config.ARConfig, "DisplayOnlyMatchedResults", enabled);
        }
    }

    private static void MapSetDecoderMatchFilter(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string matchFilter && handler._config != null)
        {
            SetStringPropertyIfAvailable(handler._config.DecoderConfig, "MatchFilter", matchFilter);
            handler.ApplyConfigLive();
        }
    }

    private static void MapSetDecoderReturnOnlyMatchedResults(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is bool enabled && handler._config != null)
        {
            SetBoolPropertyIfAvailable(handler._config.DecoderConfig, "ReturnOnlyMatchedResults", enabled);
            handler.ApplyConfigLive();
        }
    }

    private static bool GetBoolProperty(object target, string propertyName, bool fallback)
    {
        var prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (prop == null)
            return fallback;

        var value = prop.GetValue(target);
        if (value is bool b)
            return b;
        if (value is int i)
            return i != 0;
        if (value is nint ni)
            return ni != 0;

        return fallback;
    }

    private static string GetStringProperty(object target, string propertyName, string fallback)
    {
        var prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (prop == null)
            return fallback;

        return prop.GetValue(target) as string ?? fallback;
    }

    private static void SetBoolPropertyIfAvailable(object target, string propertyName, bool value)
    {
        var prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (prop == null || !prop.CanWrite)
            return;

        if (prop.PropertyType == typeof(bool))
        {
            prop.SetValue(target, value);
            return;
        }

        if (prop.PropertyType == typeof(int))
        {
            prop.SetValue(target, value ? 1 : 0);
            return;
        }

        if (prop.PropertyType == typeof(nint))
        {
            prop.SetValue(target, (nint)(value ? 1 : 0));
        }
    }

    private static void SetStringPropertyIfAvailable(object target, string propertyName, string value)
    {
        var prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (prop == null || !prop.CanWrite || prop.PropertyType != typeof(string))
            return;

        prop.SetValue(target, value ?? string.Empty);
    }

    private static void MapSetThresholdBetweenDuplicatesScans(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is int threshold && handler._config != null)
        {
            handler._config.ThresholdBetweenDuplicatesScans = threshold;
        }
    }

    private static void MapSetPowerSavingMode(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is int mode && handler._config != null)
        {
            handler._config.PowerSavingMode = mode;
        }
    }

    private static void MapSetBarcodeTypeLengthRange(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        // Windows SDK DecoderConfig does not have per-barcode length ranges
        // This is handled by the native BarkoderNET config when applied
    }

    private static void MapConfigureBarkoder(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string configAsJsonString && handler._config != null)
        {
            try
            {
                global::Barkoder.Windows.Utils.BarkoderHelper.ApplyConfigSettingsFromJson(handler._config, configAsJsonString);
                handler.EnsureBarkoderView();
                handler._winBarkoderView!.Config = handler._config;
            }
            catch { }
        }
    }

    public static void MapSetCustomOption(BarkoderViewHandler handler, BarkoderView view, object? argument)
    {
        if (handler._winBarkoderView != null && argument is ValueTuple<string, int> customOption)
        {
            var (optionName, optionValue) = customOption;
            handler._winBarkoderView.SetCustomOption(optionName, optionValue);
        }
    }

    // ---- Helpers ----

    private static WinDecoderType? MapBarcodeTypeToWin(Enums.BarcodeType type) => type switch
    {
        Enums.BarcodeType.Aztec => WinDecoderType.Aztec,
        Enums.BarcodeType.AztecCompact => WinDecoderType.AztecCompact,
        Enums.BarcodeType.QR => WinDecoderType.QR,
        Enums.BarcodeType.QRMicro => WinDecoderType.QRMicro,
        Enums.BarcodeType.Code128 => WinDecoderType.Code128,
        Enums.BarcodeType.Code93 => WinDecoderType.Code93,
        Enums.BarcodeType.Code39 => WinDecoderType.Code39,
        Enums.BarcodeType.Codabar => WinDecoderType.Codabar,
        Enums.BarcodeType.Code11 => WinDecoderType.Code11,
        Enums.BarcodeType.Msi => WinDecoderType.Msi,
        Enums.BarcodeType.UpcA => WinDecoderType.UPCa,
        Enums.BarcodeType.UpcE => WinDecoderType.UPCe,
        Enums.BarcodeType.UpcE1 => WinDecoderType.UPCe1,
        Enums.BarcodeType.Ean13 => WinDecoderType.EAN13,
        Enums.BarcodeType.Ean8 => WinDecoderType.EAN8,
        Enums.BarcodeType.PDF417 => WinDecoderType.PDF417,
        Enums.BarcodeType.PDF417Micro => WinDecoderType.PDF417Micro,
        Enums.BarcodeType.Datamatrix => WinDecoderType.DataMatrix,
        Enums.BarcodeType.Code25 => WinDecoderType.Code25,
        Enums.BarcodeType.Interleaved25 => WinDecoderType.Interleaved2of5,
        Enums.BarcodeType.Itf14 => WinDecoderType.ITF14,
        Enums.BarcodeType.Uata25 => WinDecoderType.IATA25,
        Enums.BarcodeType.Matrix25 => WinDecoderType.Matrix25,
        Enums.BarcodeType.Datalogic25 => WinDecoderType.Datalogic25,
        Enums.BarcodeType.Coop25 => WinDecoderType.COOP25,
        Enums.BarcodeType.Code32 => WinDecoderType.Code32,
        Enums.BarcodeType.Telepen => WinDecoderType.Telepen,
        Enums.BarcodeType.Dotcode => WinDecoderType.DotCode,
        Enums.BarcodeType.IDDocument => WinDecoderType.IDDocument,
        Enums.BarcodeType.Databar14 => WinDecoderType.Databar14,
        Enums.BarcodeType.DatabarLimited => WinDecoderType.DatabarLimited,
        Enums.BarcodeType.DatabarExpanded => WinDecoderType.DatabarExpanded,
        Enums.BarcodeType.PostalIMB => WinDecoderType.PostalIMB,
        Enums.BarcodeType.Postnet => WinDecoderType.Postnet,
        Enums.BarcodeType.Planet => WinDecoderType.Planet,
        Enums.BarcodeType.AustralianPost => WinDecoderType.AustralianPost,
        Enums.BarcodeType.RoyalMail => WinDecoderType.RoyalMail,
        Enums.BarcodeType.KIX => WinDecoderType.KIX,
        Enums.BarcodeType.JapanesePost => WinDecoderType.JapanesePost,
        Enums.BarcodeType.Maxicode => WinDecoderType.MaxiCode,
        Enums.BarcodeType.OCRText => WinDecoderType.OCRText,
        _ => null
    };

    /// <summary>
    /// Convert Windows SDK BarcodeResult[] to Plugin BarcodeResult[]
    /// </summary>
    private static PluginBarcodeResult[] ConvertResults(WinBarcodeResult[] winResults)
    {
        if (winResults == null || winResults.Length == 0)
            return Array.Empty<PluginBarcodeResult>();

        var results = new PluginBarcodeResult[winResults.Length];
        for (int i = 0; i < winResults.Length; i++)
        {
            var wr = winResults[i];

            // Convert extras
            Dictionary<string, object>? extra = null;
            if (wr.Extra != null && wr.Extra.Count > 0)
            {
                extra = new Dictionary<string, object>();
                foreach (var kvp in wr.Extra)
                    extra[kvp.Key] = kvp.Value;
            }

            // Convert MRZ images (BKImageDescriptor -> ImageData)
            var mrzImages = new List<PluginImageData>();
            if (wr.Images != null)
            {
                foreach (var img in wr.Images)
                {
                    if (img.Image != null)
                        mrzImages.Add(new PluginImageData(img.Name, img.Image));
                }
            }

            // Convert location (always provide a non-null object for plugin model)
            var location = new PluginBarcodeLocation
            {
                LocationName = string.Empty,
                Points = new List<PluginBarcodePoint>()
            };
            if (wr.Location != null)
            {
                location.Points = new List<PluginBarcodePoint>
                {
                    new PluginBarcodePoint(wr.Location.TopLeft.X, wr.Location.TopLeft.Y),
                    new PluginBarcodePoint(wr.Location.TopRight.X, wr.Location.TopRight.Y),
                    new PluginBarcodePoint(wr.Location.BottomRight.X, wr.Location.BottomRight.Y),
                    new PluginBarcodePoint(wr.Location.BottomLeft.X, wr.Location.BottomLeft.Y)
                };
            }

            // Convert binary data to base64
            string binaryDataAsBase64 = wr.BinaryData != null
                ? Convert.ToBase64String(wr.BinaryData)
                : string.Empty;

            // Compatibility: older Windows SDK result models may not expose IsMatched.
            bool isMatched = GetBoolProperty(wr, "IsMatched", false);

            results[i] = new PluginBarcodeResult(
                wr.TextualData ?? string.Empty,
                wr.BarcodeTypeName ?? string.Empty,
                extra,
                wr.CharacterSet ?? string.Empty,
                mrzImages,
                location,
                binaryDataAsBase64,
                string.Empty, // SADL image handled via extras
                isMatched
            );
        }

        return results;
    }

    /// <summary>
    /// Normalize a hex color string to include # prefix
    /// </summary>
    private static string NormalizeHexColor(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return "#FF000000";
        return hex.StartsWith("#") ? hex : "#" + hex;
    }

    public List<Dictionary<string, string>> GetAvailableCameras()
    {
        var result = new List<Dictionary<string, string>>();
        try
        {
            var cameras = Task.Run(() => MediaCaptureCameraProvider.GetAvailableCameras()).GetAwaiter().GetResult();
            foreach (var cam in cameras)
            {
                result.Add(new Dictionary<string, string>
                {
                    { "id", cam.Id },
                    { "name", cam.DisplayName }
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Barkoder] GetAvailableCameras error: {ex.Message}");
        }
        return result;
    }

    private static void MapSetCameraById(BarkoderViewHandler handler, BarkoderView view, object? arg3)
    {
        if (arg3 is string cameraId && handler._winBarkoderView != null)
        {
            var provider = handler._winBarkoderView.CameraProvider;
            if (provider != null)
            {
                provider.SetCameraById(cameraId);
            }
        }
    }
}

#endif
