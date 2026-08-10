using Barkoder.Windows;
using Barkoder.Windows.Camera;
using Barkoder.Windows.Config;
using Barkoder.Windows.Models;
using Barkoder.Windows.UI;
using Barkoder.Windows.Utils;
#if WINDOWS
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
#endif

namespace BKDSampleMAUI;

public partial class MainPage : ContentPage
{
    private BarkoderView? _barkoderView;
    private ICameraProvider? _cameraProvider;
    private int _totalScanCount;
    private long _lastPreviewTick;
    private long _lastResultUiTick;
    private ImageSource? _lastResultImageSource;

    // Double-tap detection for AR freeze
    private DateTime _lastTapTime = DateTime.MinValue;
    private const double DoubleTapThresholdMs = 350;

    public MainPage()
    {
        InitializeComponent();

        // Force pickers to display their initial selection
        DecodingSpeedPicker.SelectedIndex = 1;
        FormattingPicker.SelectedIndex = 1;
        ResolutionPicker.SelectedIndex = 1;

        try
        {
            LibVersionLabel.Text = $"v{Barkoder.Windows.BarkoderView.GetLibVersion()}";
        }
        catch
        {
            LibVersionLabel.Text = "";
        }

        // Auto-refresh camera list on startup
        _ = RefreshCameraListAsync();
    }

    private async Task RefreshCameraListAsync()
    {
#if WINDOWS
        try
        {
            var cameras = await Barkoder.Windows.Camera.MediaCaptureCameraProvider.GetCameras();
            CameraPicker.ItemsSource = cameras;
            CameraPicker.ItemDisplayBinding = new Binding("DisplayName");
            if (cameras.Count > 0)
                CameraPicker.SelectedIndex = 0;
        }
        catch { }
#endif
    }

    private async void OnRefreshCameras(object sender, EventArgs e)
    {
        await RefreshCameraListAsync();
    }

    private BarkoderConfig BuildConfig()
    {
        var config = new BarkoderConfig("PEmBIohr9EZXgCkySoetbwP4gvOfMcGzgxKPL2X6uqOhO-ks9vpXUc9zILWWBkt9US-wE0DJVqwbSH3-aCYLcReF_kX2X_T-f5CjJfDe3eQn7ejMDuzAbJvS0e7ms7Y8sSgMn7t1Y9Q1o_OuK17KbzNpMCOX2DTLkkUZsjofiCCdMjSSV-QIDX_0akdmVCGkod_U0rujfiC6muXRKTTp56HPU4WkZsk3djDo7DBTT0U.");

        var decoders = new List<DecoderType>();
        // 1D
        if (Code128Check.IsChecked) decoders.Add(DecoderType.Code128);
        if (Code93Check.IsChecked) decoders.Add(DecoderType.Code93);
        if (Code39Check.IsChecked) decoders.Add(DecoderType.Code39);
        if (Code11Check.IsChecked) decoders.Add(DecoderType.Code11);
        if (Code25Check.IsChecked) decoders.Add(DecoderType.Code25);
        if (Code32Check.IsChecked) decoders.Add(DecoderType.Code32);
        if (CodabarCheck.IsChecked) decoders.Add(DecoderType.Codabar);
        if (MsiCheck.IsChecked) decoders.Add(DecoderType.Msi);
        if (TelepenCheck.IsChecked) decoders.Add(DecoderType.Telepen);
        if (Interleaved2of5Check.IsChecked) decoders.Add(DecoderType.Interleaved2of5);
        if (ITF14Check.IsChecked) decoders.Add(DecoderType.ITF14);
        if (IATA25Check.IsChecked) decoders.Add(DecoderType.IATA25);
        if (Matrix25Check.IsChecked) decoders.Add(DecoderType.Matrix25);
        if (Datalogic25Check.IsChecked) decoders.Add(DecoderType.Datalogic25);
        if (COOP25Check.IsChecked) decoders.Add(DecoderType.COOP25);
        // EAN/UPC
        if (Ean13Check.IsChecked) decoders.Add(DecoderType.EAN13);
        if (Ean8Check.IsChecked) decoders.Add(DecoderType.EAN8);
        if (UpcaCheck.IsChecked) decoders.Add(DecoderType.UPCa);
        if (UpceCheck.IsChecked) decoders.Add(DecoderType.UPCe);
        if (Upce1Check.IsChecked) decoders.Add(DecoderType.UPCe1);
        // 2D
        if (QrCheck.IsChecked) decoders.Add(DecoderType.QR);
        if (QrMicroCheck.IsChecked) decoders.Add(DecoderType.QRMicro);
        if (DataMatrixCheck.IsChecked) decoders.Add(DecoderType.DataMatrix);
        if (PDF417Check.IsChecked) decoders.Add(DecoderType.PDF417);
        if (PDF417MicroCheck.IsChecked) decoders.Add(DecoderType.PDF417Micro);
        if (AztecCheck.IsChecked) decoders.Add(DecoderType.Aztec);
        if (AztecCompactCheck.IsChecked) decoders.Add(DecoderType.AztecCompact);
        if (DotCodeCheck.IsChecked) decoders.Add(DecoderType.DotCode);
        if (MaxiCodeCheck.IsChecked) decoders.Add(DecoderType.MaxiCode);
        // Databar
        if (Databar14Check.IsChecked) decoders.Add(DecoderType.Databar14);
        if (DatabarLimitedCheck.IsChecked) decoders.Add(DecoderType.DatabarLimited);
        if (DatabarExpandedCheck.IsChecked) decoders.Add(DecoderType.DatabarExpanded);
        // Postal
        if (PostalIMBCheck.IsChecked) decoders.Add(DecoderType.PostalIMB);
        if (PostnetCheck.IsChecked) decoders.Add(DecoderType.Postnet);
        if (PlanetCheck.IsChecked) decoders.Add(DecoderType.Planet);
        if (AustralianPostCheck.IsChecked) decoders.Add(DecoderType.AustralianPost);
        if (RoyalMailCheck.IsChecked) decoders.Add(DecoderType.RoyalMail);
        if (KIXCheck.IsChecked) decoders.Add(DecoderType.KIX);
        if (JapanesePostCheck.IsChecked) decoders.Add(DecoderType.JapanesePost);
        // Other
        if (IDDocumentCheck.IsChecked) decoders.Add(DecoderType.IDDocument);
        if (OCRTextCheck.IsChecked) decoders.Add(DecoderType.OCRText);

        if (decoders.Count > 0)
            config.DecoderConfig.SetEnabledDecoders(decoders.ToArray());
         
        config.CloseSessionOnResultEnabled = CloseOnResultCheck.IsChecked;
        config.BeepOnSuccessEnabled = BeepCheck.IsChecked;
    
        config.BarkoderResolution = ResolutionPicker.SelectedIndex switch
        {
            0 => BarkoderResolution.HD,
            2 => BarkoderResolution.UHD,
            _ => BarkoderResolution.FHD
        };


        config.ThresholdBetweenDuplicatesScans = (int)ThresholdSlider.Value;

        config.ImageResultEnabled = ImageResultCheck.IsChecked;
        config.BarcodeThumbnailOnResult = ThumbnailCheck.IsChecked;
        config.LocationInImageResultEnabled = LocationInImageCheck.IsChecked;
        config.LocationInPreviewEnabled = LocationInPreviewCheck.IsChecked;
        config.RegionOfInterestVisible = RoiVisibleCheck.IsChecked;

        // Decoding speed
        config.DecoderConfig.DecodingSpeed = DecodingSpeedPicker.SelectedIndex switch
        {
            0 => DecodingSpeed.Fast,
            2 => DecodingSpeed.Slow,
            3 => DecodingSpeed.Rigorous,
            _ => DecodingSpeed.Normal
        };

        // Formatting
        config.DecoderConfig.Formatting = FormattingPicker.SelectedItem is string selectedFormatting
            && Enum.TryParse<FormattingType>(selectedFormatting, out var formatting)
            ? formatting
            : FormattingType.Automatic;

        config.DecoderConfig.EnableMisshaped1D = EnableMisshaped1DCheck.IsChecked;
        config.DecoderConfig.CompositeCodesEnabled = EnableCompositeCheck.IsChecked;
        config.DecoderConfig.UpcEanDeblur = UpcEanDeblurCheck.IsChecked;
        config.DecoderConfig.EnableVINRestrictions = EnableVINCheck.IsChecked;
        config.MulticodeCachingEnabled = MulticodeCachingCheck.IsChecked;

        config.DecoderConfig.MaximumResultsCount = 200;

        // AR Config
        config.ARConfig.ARMode = ARModePicker.SelectedIndex switch
        {
            1 => BarkoderARMode.InteractiveDisabled,
            2 => BarkoderARMode.InteractiveEnabled,
            3 => BarkoderARMode.NonInteractive,
            _ => BarkoderARMode.Off
        };

        config.ARConfig.LocationType = ARLocationTypePicker.SelectedIndex switch
        {
            0 => BarkoderARLocationType.None,
            1 => BarkoderARLocationType.Tight,
            _ => BarkoderARLocationType.BoundingBox
        };

        config.ARConfig.HeaderShowMode = ARHeaderModePicker.SelectedIndex switch
        {
            0 => BarkoderARHeaderShowMode.Never,
            1 => BarkoderARHeaderShowMode.Always,
            _ => BarkoderARHeaderShowMode.OnSelected
        };

        config.ARConfig.OverlayRefresh = ARRefreshPicker.SelectedIndex switch
        {
            1 => BarkoderAROverlayRefresh.Normal,
            _ => BarkoderAROverlayRefresh.Smooth
        };

        config.ARConfig.ResultDisappearanceDelayMs = (int)ARDisappearDelaySlider.Value;
        config.ARConfig.LocationTransitionSpeed = (float)ARTransitionSpeedSlider.Value;

        if (!string.IsNullOrWhiteSpace(ARHeaderFormatEntry.Text))
            config.ARConfig.HeaderTextFormat = ARHeaderFormatEntry.Text;

        config.ARConfig.ImageResultEnabled = ARImageResultCheck.IsChecked;
        config.ARConfig.BarcodeThumbnailOnResult = ARBarcodeThumbnailCheck.IsChecked;
        config.ARConfig.DoubleTapToFreezeEnabled = ARDoubleTapFreezeCheck.IsChecked;

        // ROI: left=10%, top=10%, width=80%, height=80%
        config.SetRegionOfInterest(0, 0, 100, 100);

        return config;
    }

    private async void OnStartScanning(object sender, EventArgs e)
    {
        try
        {
            _barkoderView?.Dispose();
            _barkoderView = new BarkoderView();

            var config = BuildConfig();
            _barkoderView.Config = config;
    
            // Enable OCR functionality
            _barkoderView.SetCustomOption("enable_ocr_functionality", 1);

            // Log license info for debugging
            var licenseInfo = _barkoderView.GetLicenseInfo();
            MauiProgram.Log($"LICENSE INFO: {licenseInfo}");

            UpdateButtonStates(scanning: true);
            StateLabel.Text = "State: Initializing camera...";
            NoCameraLabel.IsVisible = false;
            ScanImageOverlay.IsVisible = false;

            // Enable AR tap target if AR mode is interactive
            bool arInteractive = config.ARConfig.ARMode == BarkoderARMode.InteractiveEnabled ||
                                 config.ARConfig.ARMode == BarkoderARMode.InteractiveDisabled;
            ARTapTarget.IsVisible = arInteractive;

#if WINDOWS
            var mediaCaptureProvider = new Barkoder.Windows.Camera.MediaCaptureCameraProvider();
            _cameraProvider = mediaCaptureProvider;
            _barkoderView.SetCameraProvider(mediaCaptureProvider);

            // Use selected camera if one was chosen
            if (CameraPicker.SelectedItem is CameraDeviceInfo selectedCamera)
            {
                mediaCaptureProvider.SetCameraId(selectedCamera.Id);
            }
#else
            _cameraProvider = new MockCameraProvider();
            _barkoderView.SetCameraProvider(_cameraProvider);
#endif

            // Forward preview frames to the CameraPreview view (~30 fps throttle)
            _barkoderView.SetPreviewFramesCallback((frameData, width, height) =>
            {
                var now = Environment.TickCount64;
                if (now - _lastPreviewTick < 33) return; // ~30fps max
                _lastPreviewTick = now;

                // When frozen, keep rendering the frozen frame (with updated overlays)
                // but don't update with new live camera frames
                var bv = _barkoderView;
                if (bv == null) return;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Attach overlay (ROI + barcode locations) from the SDK
                    CameraPreviewView.Overlay = bv.BuildOverlay();

                    // When frozen, render the captured frozen frame instead of live camera
                    var frozen = bv.FrozenFrame;
                    if (frozen != null)
                    {
                        CameraPreviewView.UpdateFrame(frozen.Value.Data, frozen.Value.Width, frozen.Value.Height);
                    }
                    else
                    {
                        CameraPreviewView.UpdateFrame(frameData, width, height);
                    }
                });
            });

            _barkoderView.SetPerformanceCallback((fps, dps) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    StateLabel.Text = $"State: Scanning | FPS: {fps:F1} | DPS: {dps:F1}";
                });
            });

            StateLabel.Text = "State: Starting scanner...";

            await _barkoderView.StartScanning((results, thumbnails, resultImage) =>
            {
                // Guard against empty results (e.g. AR deselect emits empty array)
                if (results == null || results.Length == 0) return;

                // Count all results
                Interlocked.Add(ref _totalScanCount, results.Length);

                var now = Environment.TickCount64;
                if (now - _lastResultUiTick < 200) return;
                _lastResultUiTick = now;

                // Capture last result for UI
                var lastResult = results[^1];
                var count = _totalScanCount;

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    ResultCountLabel.Text = $"({count})";
                    LastCallbackLengthLabel.Text = $"[batch: {results.Length}]";

                    // Update last scanned label
                    MauiProgram.Log($"CAMERA RESULT: TextualData='{lastResult.TextualData}', Type='{lastResult.BarcodeTypeName}'");
                    LastScannedLabel.Text = string.IsNullOrEmpty(lastResult.BarcodeTypeName)
                        ? "(empty)"
                        : lastResult.BarcodeTypeName;
                    LastScannedTypeLabel.Text = lastResult.TextualData;

                    // Update single result display
                    LastResultTypeLabel.Text = lastResult.BarcodeTypeName;
                    LastResultDataLabel.Text = lastResult.TextualData;

                    // Show extras if available
                    if (lastResult.Extra != null && lastResult.Extra.Count > 0)
                    {
                        LastResultExtrasLabel.Text = string.Join("\n", lastResult.Extra.Select(kv => $"{kv.Key}: {kv.Value}"));
                        LastResultExtrasLabel.IsVisible = true;
                    }
                    else
                    {
                        LastResultExtrasLabel.Text = "";
                        LastResultExtrasLabel.IsVisible = false;
                    }

                    // Show result image (already an ImageSource from SDK)
                    if (resultImage != null)
                    {
                        _lastResultImageSource = resultImage;
                        ResultImageView.Source = await DownscaleForDisplayAsync(resultImage, 1200) ?? resultImage;
                    }

                    // Show last thumbnail (already an ImageSource from SDK)
                    ThumbnailsPanel.Children.Clear();
                    if (thumbnails != null && thumbnails.Length > 0)
                    {
                        var lastThumb = thumbnails[^1];
                        if (lastThumb != null)
                        {
                            var displayThumb = await DownscaleForDisplayAsync(lastThumb, 400) ?? lastThumb;
                            ThumbnailsPanel.Children.Add(new Image
                            {
                                Source = displayThumb,
                                HeightRequest = 80,
                                Aspect = Aspect.AspectFit,
                            });
                        }
                    }

                    // Show MRZ images (picture, signature, document, main)
                    ShowMrzImages(lastResult);

                    if (_barkoderView?.State == BarkoderState.Stopped)
                    {
                        UpdateButtonStates(scanning: false);
                        StateLabel.Text = "State: Stopped (auto-close on result)";
                        StopCameraPreview();
                    }
                });
            });

            StateLabel.Text = $"State: {_barkoderView.State}";
        }
        catch (Exception ex)
        {
            MauiProgram.Log($"StartScanning ERROR: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
            StateLabel.Text = "State: Error";
            NoCameraLabel.IsVisible = true;
            await DisplayAlert("Error", $"Failed to start scanning:\n{ex.Message}", "OK");
            UpdateButtonStates(scanning: false);
        }
    }

    private async void OnStopScanning(object sender, EventArgs e)
    {
        if (_barkoderView == null) return;

        try
        {
            await _barkoderView.StopScanning();
            StateLabel.Text = $"State: {_barkoderView.State}";
        }
        catch (Exception ex)
        {
            StateLabel.Text = "State: Error";
            await DisplayAlert("Error", $"Failed to stop scanning:\n{ex.Message}", "OK");
        }

        StopCameraPreview();
        UpdateButtonStates(scanning: false);
    }



    private void StopCameraPreview()
    {
        NoCameraLabel.IsVisible = true;
        ARTapTarget.IsVisible = false;

        if (_cameraProvider != null)
        {
            _cameraProvider.Dispose();
            _cameraProvider = null;
        }
    }

    private async void OnScanImage(object sender, EventArgs e)
    {
        try
        {
            var fileResult = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a barcode image",
                FileTypes = FilePickerFileType.Images
            });

            if (fileResult == null) return;

            StateLabel.Text = "State: Loading image...";

            byte[] bgra8;
            int imgWidth, imgHeight;

            using var stream = await fileResult.OpenReadAsync();
            using var memStream = new MemoryStream();
            await stream.CopyToAsync(memStream);
            var fileBytes = memStream.ToArray();

#if WINDOWS
            using var ras = new Windows.Storage.Streams.InMemoryRandomAccessStream();
            await ras.WriteAsync(fileBytes.AsBuffer());
            ras.Seek(0);
            var bitmapDecoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(ras);
            var softwareBitmap = await bitmapDecoder.GetSoftwareBitmapAsync(
                Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                Windows.Graphics.Imaging.BitmapAlphaMode.Premultiplied);
            imgWidth = softwareBitmap.PixelWidth;
            imgHeight = softwareBitmap.PixelHeight;
            bgra8 = new byte[imgWidth * imgHeight * 4];
            softwareBitmap.CopyToBuffer(bgra8.AsBuffer());
            softwareBitmap.Dispose();
#else
            throw new PlatformNotSupportedException("Image scanning only supported on Windows.");
#endif

            StateLabel.Text = $"State: Image loaded ({imgWidth}x{imgHeight}), decoding...";

            // Show the image in the camera preview area
            NoCameraLabel.IsVisible = false;
            ScanImageOverlay.IsVisible = false;
            CameraPreviewView.UpdateFrame(bgra8, imgWidth, imgHeight);

            // Build config and call SDK ScanImage
            var config = BuildConfig();

            await BarkoderView.ScanImage(bgra8, imgWidth, imgHeight, config,
                async (results, thumbnails, resultImage) =>
                {
                    if (results != null && results.Length > 0)
                    {
                        _totalScanCount += results.Length;
                        ResultCountLabel.Text = $"({_totalScanCount})";
                        var last = results[^1];
                        LastResultTypeLabel.Text = last.BarcodeTypeName;
                        LastResultDataLabel.Text = last.TextualData;

                        if (last.Extra != null && last.Extra.Count > 0)
                        {
                            LastResultExtrasLabel.Text = string.Join("\n", last.Extra.Select(kv => $"{kv.Key}: {kv.Value}"));
                            LastResultExtrasLabel.IsVisible = true;
                        }
                        else
                        {
                            LastResultExtrasLabel.Text = "";
                            LastResultExtrasLabel.IsVisible = false;
                        }

                        // Show result image
                        if (resultImage != null)
                        {
                            _lastResultImageSource = resultImage;
                            var displayImage = await DownscaleForDisplayAsync(resultImage, 1200) ?? resultImage;
                            ResultImageView.Source = displayImage;
                            ScanImageOverlay.Source = displayImage;
                            ScanImageOverlay.IsVisible = true;
                        }

                        // Show thumbnails
                        ThumbnailsPanel.Children.Clear();
                        if (thumbnails != null)
                        {
                            foreach (var thumb in thumbnails)
                            {
                                if (thumb == null) continue;
                                var displayThumb = await DownscaleForDisplayAsync(thumb, 400) ?? thumb;
                                ThumbnailsPanel.Children.Add(new Image
                                {
                                    Source = displayThumb,
                                    HeightRequest = 60,
                                    Aspect = Aspect.AspectFit,
                                    Margin = new Thickness(2)
                                });
                            }
                        }

                        LastScannedLabel.Text = string.IsNullOrEmpty(last.BarcodeTypeName) ? "(empty)" : last.BarcodeTypeName;
                        LastScannedTypeLabel.Text = last.TextualData;
                        StateLabel.Text = $"State: Found {results.Length} barcode(s) in image";

                        // Show MRZ images
                        ShowMrzImages(last);
                    }
                    else
                    {
                        StateLabel.Text = "State: No barcodes found in image";
                    }
                });
        }
        catch (Exception ex)
        {
            MauiProgram.Log($"ScanImage ERROR: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
            StateLabel.Text = "State: Error";
            await DisplayAlert("Error", $"Failed to scan image:\n{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}", "OK");
        }
    }

    private void OnClearResults(object sender, EventArgs e)
    {
        _totalScanCount = 0;
        ResultCountLabel.Text = "(0)";
        LastCallbackLengthLabel.Text = "";
        LastResultTypeLabel.Text = "";
        LastResultDataLabel.Text = "";
        ResultImageView.Source = null;
        _lastResultImageSource = null;
        ScanImageOverlay.IsVisible = false;
        ThumbnailsPanel.Children.Clear();
        MrzImagesPanel.Children.Clear();
        MrzImagesFrame.IsVisible = false;
    }

    private async void OnSaveResultImage(object sender, EventArgs e)
    {
        if (_lastResultImageSource is not StreamImageSource streamSource)
        {
            await DisplayAlert("Info", "No result image to save.", "OK");
            return;
        }

        try
        {
            using var stream = await streamSource.Stream(CancellationToken.None);
            if (stream == null)
            {
                await DisplayAlert("Error", "Could not read image stream.", "OK");
                return;
            }

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var fileName = $"barkoder_result_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var filePath = Path.Combine(desktop, fileName);

            using var fileStream = File.Create(filePath);
            stream.Position = 0;
            await stream.CopyToAsync(fileStream);

            await DisplayAlert("Saved", $"Image saved to:\n{filePath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save image: {ex.Message}", "OK");
        }
    }

#if WINDOWS
    /// <summary>
    /// Downscales a StreamImageSource to maxWidth using high-quality Fant interpolation.
    /// Returns null if the image is already small enough or on error.
    /// </summary>
    private static async Task<ImageSource?> DownscaleForDisplayAsync(ImageSource source, uint maxWidth)
    {
        if (source is not StreamImageSource streamSource)
            return null;

        try
        {
            using var stream = await streamSource.Stream(CancellationToken.None);
            if (stream == null) return null;
            stream.Position = 0;

            var ras = stream.AsRandomAccessStream();
            var decoder = await BitmapDecoder.CreateAsync(ras);

            if (decoder.PixelWidth <= maxWidth)
                return null; // no downscale needed

            double scale = (double)maxWidth / decoder.PixelWidth;
            uint newW = maxWidth;
            uint newH = (uint)(decoder.PixelHeight * scale);

            var transform = new BitmapTransform
            {
                ScaledWidth = newW,
                ScaledHeight = newH,
                InterpolationMode = BitmapInterpolationMode.Fant
            };

            var pixelData = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                transform, ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);

            var pixels = pixelData.DetachPixelData();

            using var outStream = new global::Windows.Storage.Streams.InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, outStream);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                newW, newH, 96, 96, pixels);
            await encoder.FlushAsync();

            outStream.Seek(0);
            var bytes = new byte[outStream.Size];
            await outStream.ReadAsync(bytes.AsBuffer(), (uint)outStream.Size,
                global::Windows.Storage.Streams.InputStreamOptions.None);

            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }
        catch
        {
            return null;
        }
    }
#endif

    private async void ShowMrzImages(BarcodeResult result)
    {
        MrzImagesPanel.Children.Clear();

        if (result.Images == null || result.Images.Count == 0)
        {
            MrzImagesFrame.IsVisible = false;
            return;
        }

        MrzImagesFrame.IsVisible = true;

        foreach (var descriptor in result.Images)
        {
            var container = new VerticalStackLayout { Spacing = 2 };

            container.Children.Add(new Label
            {
                Text = descriptor.Name,
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Gray
            });

            if (descriptor.Image != null)
            {
                var displayImage = await DownscaleForDisplayAsync(descriptor.Image, 600) ?? descriptor.Image;
                container.Children.Add(new Border
                {
                    Stroke = Colors.Gray,
                    StrokeThickness = 1,
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 4 },
                    Background = new SolidColorBrush(Colors.Black),
                    HeightRequest = 100,
                    Content = new Image
                    {
                        Source = displayImage,
                        Aspect = Aspect.AspectFit
                    }
                });
            }

            MrzImagesPanel.Children.Add(container);
        }
    }

    private void OnThresholdChanged(object sender, ValueChangedEventArgs e)
    {
        var val = (int)e.NewValue;
        ThresholdValueLabel.Text = val < 0 ? "off" : $"{val}s";
    }

    private void OnARDisappearDelayChanged(object sender, ValueChangedEventArgs e)
    {
        ARDisappearDelayLabel.Text = $"{(int)e.NewValue}ms";
    }

    private void OnARTransitionSpeedChanged(object sender, ValueChangedEventArgs e)
    {
        ARTransitionSpeedLabel.Text = $"{e.NewValue:F1}";
    }

    private void OnARTap(object sender, TappedEventArgs e)
    {
        if (_barkoderView == null) return;

        var now = DateTime.UtcNow;
        double elapsed = (now - _lastTapTime).TotalMilliseconds;
        _lastTapTime = now;

        // Double-tap detection: toggle freeze
        if (elapsed < DoubleTapThresholdMs && _barkoderView.Config?.ARConfig.DoubleTapToFreezeEnabled == true)
        {
            bool frozen = _barkoderView.ToggleFreeze();
            MauiProgram.Log($"AR DOUBLE-TAP: freeze={frozen}");
            return; // Don't process as single tap
        }

        // Single tap: select/deselect barcode
        // Get tap position relative to the tap target (which covers the camera preview)
        var position = e.GetPosition(ARTapTarget);
        if (position == null) return;

        float tapX = (float)position.Value.X;
        float tapY = (float)position.Value.Y;

        // Convert view coordinates to frame pixel coordinates
        float viewWidth = (float)CameraPreviewGrid.Width;
        float viewHeight = (float)CameraPreviewGrid.Height;
        int frameW = _barkoderView.LastFrameWidth;
        int frameH = _barkoderView.LastFrameHeight;

        if (viewWidth <= 0 || viewHeight <= 0 || frameW <= 0 || frameH <= 0)
            return;

        // The camera preview uses Uniform stretch, so compute the actual rendered area
        float viewAspect = viewWidth / viewHeight;
        float frameAspect = (float)frameW / frameH;

        float renderW, renderH, offsetX, offsetY;
        if (frameAspect > viewAspect)
        {
            // Frame wider than view — letterboxed top/bottom
            renderW = viewWidth;
            renderH = viewWidth / frameAspect;
            offsetX = 0;
            offsetY = (viewHeight - renderH) / 2f;
        }
        else
        {
            // Frame taller than view — pillarboxed left/right
            renderH = viewHeight;
            renderW = viewHeight * frameAspect;
            offsetX = (viewWidth - renderW) / 2f;
            offsetY = 0;
        }

        // Map tap to frame pixel coordinates
        float frameX = (tapX - offsetX) / renderW * frameW;
        float frameY = (tapY - offsetY) / renderH * frameH;

        // Clamp to frame bounds
        frameX = Math.Clamp(frameX, 0, frameW - 1);
        frameY = Math.Clamp(frameY, 0, frameH - 1);

        bool toggled = _barkoderView.ARToggleSelectionAt(frameX, frameY);

        if (toggled)
        {
            MauiProgram.Log($"AR TAP: toggled barcode at frame ({frameX:F0}, {frameY:F0})");

            // Update selected count display
            var selectedResults = _barkoderView.GetARSelectedResults();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ResultCountLabel.Text = $"({selectedResults.Length} selected)";
                if (selectedResults.Length > 0)
                {
                    var last = selectedResults[^1];
                    LastScannedLabel.Text = last.BarcodeTypeName;
                    LastScannedTypeLabel.Text = last.TextualData;
                    LastResultTypeLabel.Text = last.BarcodeTypeName;
                    LastResultDataLabel.Text = last.TextualData;
                }
            });
        }
    }

    private void UpdateButtonStates(bool scanning)
    {
        StartBtn.IsEnabled = !scanning;
        StopBtn.IsEnabled = scanning;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        StopCameraPreview();
        _barkoderView?.Dispose();
        _barkoderView = null;
    }
}

