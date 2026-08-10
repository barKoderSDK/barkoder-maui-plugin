using Barkoder.Windows.Camera;
using Barkoder.Windows.Config;
using Barkoder.Windows.Decoder;
using Barkoder.Windows.Models;
using Barkoder.Windows.UI;
using System.Reflection;
#if WINDOWS
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Media.Core;
using Windows.Media.Playback;
#endif

namespace Barkoder.Windows
{
    /// <summary>
    /// Main entry point for the Barkoder barcode scanner.
    /// Manages the camera, decoder, and scanning lifecycle.
    /// Analogous to BarkoderView on Android and iOS.
    /// </summary>
    public class BarkoderView : IDisposable
    {
        private readonly CameraManager _cameraManager;
        private readonly BarcodeDecoder _decoder;
        private readonly SynchronizationContext? _syncContext;

        private BarkoderConfig? _config;
        private BarkoderResultCallback? _resultCallback;
        private BarkoderPerformanceCallback? _performanceCallback;
        private CameraCallback? _cameraCallback;

        private BarkoderState _state = BarkoderState.Idle;
        private bool _isDisposed;
        private CancellationTokenSource? _scanCts;

        // Duplicate filtering
        private readonly Dictionary<string, DateTime> _recentScans = new();

        // Beep sound
        private static string? _beepFilePath;

        // Latest barcode locations for overlay drawing
        private BarcodeLocation[]? _lastLocations;
        private DateTime _lastLocationTime = DateTime.MinValue;

        // AR mode manager
        private BarkoderARManager? _arManager;

        // Freeze state for AR double-tap
        private bool _isFrozen;
        private byte[]? _frozenFrameData;
        private int _frozenFrameWidth;
        private int _frozenFrameHeight;

        /// <summary>
        /// Current state of the scanner.
        /// </summary>
        public BarkoderState State => _state;

        /// <summary>
        /// Whether the AR scanning is currently frozen (double-tap freeze).
        /// </summary>
        public bool IsFrozen => _isFrozen;

        /// <summary>
        /// The frozen frame data, dimensions. Null when not frozen.
        /// </summary>
        public (byte[] Data, int Width, int Height)? FrozenFrame =>
            _isFrozen && _frozenFrameData != null
                ? (_frozenFrameData, _frozenFrameWidth, _frozenFrameHeight)
                : null;

        /// <summary>
        /// The active configuration. Must be set before scanning.
        /// Initializes the native decoder on first set or when the license key changes.
        /// </summary>
        public BarkoderConfig? Config
        {
            get => _config;
            set
            {
                _config = value;
                if (value != null)
                {
                    // Initialize or re-initialize native decoder if needed
                    if (!_decoder.IsInitialized)
                    {
                        _decoder.Initialize(value.LicenseKey);
                    }
                    _decoder.ApplyConfig(value.DecoderConfig, value);
                }
            }
        }

        /// <summary>
        /// Provides access to the underlying camera provider for UI binding.
        /// </summary>
        public ICameraProvider? CameraProvider => _cameraManager.Provider;

        /// <summary>
        /// Width of the last camera frame received. Useful for interpreting raw result images.
        /// </summary>
        public int LastFrameWidth { get; private set; }

        /// <summary>
        /// Height of the last camera frame received. Useful for interpreting raw result images.
        /// </summary>
        public int LastFrameHeight { get; private set; }

        /// <summary>
        /// Stores the last camera frame data for AR image/thumbnail generation.
        /// </summary>
        private byte[]? _lastFrameData;

        public BarkoderView()
        {
            _cameraManager = new CameraManager();
            _decoder = new BarcodeDecoder();
            _syncContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// Set the camera provider. Must be called before StartScanning or StartCameraAsync.
        /// Different UI frameworks (WinUI, WPF, etc.) provide their own implementation.
        /// </summary>
        public void SetCameraProvider(ICameraProvider cameraProvider)
        {
            _cameraManager.SetProvider(cameraProvider);
        }

        /// <summary>
        /// Start the camera preview without decoding.
        /// </summary>
        public async Task StartCameraAsync()
        {
            EnsureNotDisposed();
            EnsureConfigSet();

            try
            {
                await _cameraManager.InitializeAsync(_config!);
                _state = BarkoderState.CameraReady;
                _cameraCallback?.Invoke(CameraEvent.Opened);
            }
            catch (Exception)
            {
                _state = BarkoderState.Error;
                _cameraCallback?.Invoke(CameraEvent.Error);
                throw;
            }
        }

        /// <summary>
        /// Start scanning for barcodes. The camera will be initialized if not already running.
        /// Results are delivered via the callback.
        /// </summary>
        /// <param name="resultCallback">Callback invoked when barcodes are decoded.</param>
        public async Task StartScanning(BarkoderResultCallback resultCallback)
        {
            EnsureNotDisposed();
            EnsureConfigSet();

            if (resultCallback == null)
                throw new ArgumentNullException(nameof(resultCallback));

            if (_state == BarkoderState.Scanning)
                return; // Already scanning

            _resultCallback = resultCallback;
            _scanCts = new CancellationTokenSource();
            _recentScans.Clear();

            // Initialize camera if needed
            if (!_cameraManager.IsInitialized)
            {
                try
                {
                    await _cameraManager.InitializeAsync(_config!);
                    _cameraCallback?.Invoke(CameraEvent.Opened);
                }
                catch (Exception)
                {
                    _state = BarkoderState.Error;
                    _cameraCallback?.Invoke(CameraEvent.Error);
                    throw;
                }
            }

            _state = BarkoderState.Scanning;

            // Start frame processing
            await _cameraManager.StartFrameReaderAsync(OnFrameArrived);
        }

        /// <summary>
        /// Stop scanning and release camera resources.
        /// </summary>
        public async Task StopScanning()
        {
            if (_state == BarkoderState.Stopped || _state == BarkoderState.Idle)
                return;

            _scanCts?.Cancel();

            // Emit deferred AR results before clearing
            if (_config?.ARConfig.ARMode != BarkoderARMode.Off &&
                _config.ARConfig.EmitResultsAtSessionEndOnly &&
                _arManager != null && _resultCallback != null)
            {
                EmitARSelectedResults(null, LastFrameWidth, LastFrameHeight);
            }

            await _cameraManager.StopFrameReaderAsync();
            await _cameraManager.CleanupAsync();

            _resultCallback = null;
            _state = BarkoderState.Stopped;
            _arManager?.Clear();
            _isFrozen = false;
            _frozenFrameData = null;
            _cameraCallback?.Invoke(CameraEvent.Closed);
        }

        /// <summary>
        /// Pause scanning (keeps camera alive but stops decoding).
        /// </summary>
        public async Task PauseScanning()
        {
            if (_state != BarkoderState.Scanning) return;

            await _cameraManager.StopFrameReaderAsync();
            _state = BarkoderState.Paused;
        }

        /// <summary>
        /// Resume scanning after a pause.
        /// </summary>
        public async Task ResumeScanning()
        {
            if (_state != BarkoderState.Paused) return;
            if (_resultCallback == null) return;

            _state = BarkoderState.Scanning;
            await _cameraManager.StartFrameReaderAsync(OnFrameArrived);
        }

        /// <summary>
        /// Scan an image for barcodes. Static method that does not require a BarkoderView instance or camera.
        /// Matches the scanImage API from Android/iOS SDKs.
        /// Scales down large images, decodes on a background thread, and delivers results via callback on the UI thread.
        /// </summary>
        public static async Task ScanImage(byte[] bgra8Data, int width, int height, BarkoderConfig config, BarkoderResultCallback resultCallback)
        {
            if (bgra8Data == null) throw new ArgumentNullException(nameof(bgra8Data));
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (resultCallback == null) throw new ArgumentNullException(nameof(resultCallback));

            var syncContext = SynchronizationContext.Current;

            await Task.Run(() =>
            {
                try
                {
                    // Scale down if needed (max 3000px on larger dimension) — matches Android/iOS
                    byte[] pixels = bgra8Data;
                    int w = width, h = height;
                    int largerDimension = Math.Max(w, h);

                    if (largerDimension > 3000)
                    {
                        int scaleFactor = largerDimension > 7000 ? 4 : (int)(largerDimension / 3000.0) + 1;
                        int newW = w / scaleFactor;
                        int newH = h / scaleFactor;
                        pixels = ScaleBgra8(bgra8Data, w, h, newW, newH);
                        w = newW;
                        h = newH;
                    }

                    // Initialize native decoder
                    var initResult = BarkoderNET.Config.InitializeWithLicenseKey(config.LicenseKey);
                    if (initResult.Configuration == null)
                    {
                        DeliverScanImageCallback(syncContext, resultCallback, Array.Empty<BarcodeResult>(), null, null);
                        return;
                    }

                    using var nativeConfig = initResult.Configuration;

                    // Map and enable decoders from config
                    var enabledDecoders = config.DecoderConfig.GetEnabledDecoders();
                    var nativeDecoders = new List<BarkoderNET.DecoderType>();
                    foreach (var dt in enabledDecoders)
                    {
                        var native = BarcodeDecoder.MapToNativeDecoderType(dt);
                        if (native != null)
                            nativeDecoders.Add(native.Value);
                    }
                    nativeConfig.SetEnabledDecoders(nativeDecoders);

                    // Apply config settings
                    nativeConfig.Formatting = BarcodeDecoder.MapFormatting(config.DecoderConfig.Formatting);
                    if (!string.IsNullOrEmpty(config.DecoderConfig.EncodingCharacterSet))
                        nativeConfig.EncodingCharacterSet = config.DecoderConfig.EncodingCharacterSet;
                    nativeConfig.EnableMisshaped1D = config.DecoderConfig.EnableMisshaped1D ? 1 : 0;
                    nativeConfig.UpcEanDeblur = config.DecoderConfig.UpcEanDeblur ? 1 : 0;
                    nativeConfig.EnableComposite = config.DecoderConfig.CompositeCodesEnabled ? 1 : 0;
                    nativeConfig.EnableVINRestrictions = config.DecoderConfig.EnableVINRestrictions ? 1 : 0;
                    BarcodeDecoder.ApplyPerDecoderConfigs(nativeConfig, config.DecoderConfig);

                    // Scan image overrides — matches Android/iOS behavior
                    nativeConfig.DecodingSpeed = BarkoderNET.DecodingSpeed.Rigorous;
                    nativeConfig.RegionOfInterest = new BarkoderNET.Rect { Left = 0, Top = 0, Width = 100, Height = 100 };
                    nativeConfig.MaximumResultsCount = 200;
                    nativeConfig.SetCustomOption("enable_ocr_functionality", 1);

                    // Global options
                    nativeConfig.SetGlobalOption(BarkoderNET.BKGlobalOption.SetMaximumThreads, config.ThreadsLimit);
                    nativeConfig.SetGlobalOption(BarkoderNET.BKGlobalOption.MulticodeCachingEnabled, config.MulticodeCachingEnabled ? 1 : 0);
                    nativeConfig.SetGlobalOption(BarkoderNET.BKGlobalOption.MulticodeCachingDuration, config.MulticodeCachingDuration);

                    System.Diagnostics.Debug.WriteLine($"[Barkoder] ScanImage: {w}x{h}, {nativeDecoders.Count} decoders enabled");
                    System.Diagnostics.Debug.WriteLine($"[Barkoder] ScanImage decoders: {string.Join(", ", nativeDecoders)}");
                    System.Diagnostics.Debug.WriteLine($"[Barkoder] ScanImage speed={nativeConfig.DecodingSpeed}, OCR custom option set, ROI=0,0,100,100");

                    var nativeResults = BarkoderNET.Barkoder.DecodeImageMemory(
                        nativeConfig, pixels, w, h, BarkoderNET.BKColorFormat.BGRA);

                    System.Diagnostics.Debug.WriteLine($"[Barkoder] ScanImage results: {nativeResults?.Count ?? 0}");

                    if (nativeResults == null || nativeResults.Count == 0)
                    {
                        DeliverScanImageCallback(syncContext, resultCallback, Array.Empty<BarcodeResult>(), null, null);
                        return;
                    }

                    // Suppress GC finalizer to prevent native crash
                    foreach (var nr in nativeResults)
                        GC.SuppressFinalize(nr);

                    // Map native results to SDK results
                    var results = nativeResults.Select(BarcodeDecoder.MapFromNativeResult).ToArray();

                    // Build result image with location overlays
                    byte[]? resultImageData = null;
                    if (config.ImageResultEnabled)
                    {
                        resultImageData = new byte[pixels.Length];
                        Buffer.BlockCopy(pixels, 0, resultImageData, 0, pixels.Length);
                        if (config.LocationInImageResultEnabled)
                            DrawLocationsOnFrame(resultImageData, w, h, results, config);
                    }

                    // Build thumbnails
                    byte[][]? rawThumbnails = null;
                    if (config.BarcodeThumbnailOnResult)
                        rawThumbnails = CropThumbnails(results, pixels, w, h);

                    // Extract MRZ sub-images
                    var mrzImages = ExtractMrzImages(results, pixels, w, h);

                    // Play beep
                    if (config.BeepOnSuccessEnabled)
                        PlayBeepSound();

                    // Deliver results with ImageSource conversion on UI thread
                    DeliverScanImageResults(syncContext, resultCallback, results, rawThumbnails, resultImageData, w, h, mrzImages);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Barkoder] ScanImage error: {ex.Message}");
                    DeliverScanImageCallback(syncContext, resultCallback, Array.Empty<BarcodeResult>(), null, null);
                }
            });
        }

        // ---- Camera Controls ----

        /// <summary>
        /// Switch between front and back camera.
        /// </summary>
        public async Task SetCamera(BarkoderCameraPosition position)
        {
            EnsureNotDisposed();
            if (_config != null)
                _config.CameraPosition = position;
            await _cameraManager.SetCameraAsync(position);
        }

        /// <summary>
        /// Set the camera zoom factor.
        /// </summary>
        public void SetZoomFactor(float zoomFactor)
        {
            _cameraManager.SetZoomFactor(zoomFactor);
        }

        /// <summary>
        /// Get the maximum zoom factor supported by the current camera.
        /// </summary>
        public void GetMaxZoomFactor(MaxZoomAvailableCallback callback)
        {
            callback?.Invoke(_cameraManager.GetMaxZoomFactor());
        }

        /// <summary>
        /// Check if flash/torch is available on the current camera.
        /// </summary>
        public void IsFlashAvailable(FlashAvailableCallback callback)
        {
            callback?.Invoke(_cameraManager.IsFlashAvailable());
        }

        /// <summary>
        /// Enable or disable the camera flash/torch.
        /// </summary>
        public void SetFlashEnabled(bool enabled)
        {
            _cameraManager.SetFlashEnabled(enabled);
        }

        /// <summary>
        /// Capture the current camera frame as a PNG byte array.
        /// </summary>
        public async Task<byte[]?> CaptureImageAsync()
        {
            EnsureNotDisposed();
            return await _cameraManager.CaptureFrameAsync();
        }

        // ---- Callbacks ----

        /// <summary>
        /// Set a callback for performance metrics (FPS/DPS).
        /// </summary>
        public void SetPerformanceCallback(BarkoderPerformanceCallback? callback)
        {
            _performanceCallback = callback;
        }

        /// <summary>
        /// Set a callback for raw preview frames.
        /// </summary>
        public void SetPreviewFramesCallback(BarkoderPreviewFramesCallback? callback)
        {
            _cameraManager.SetPreviewFramesCallback(callback);
        }

        /// <summary>
        /// Set a callback for camera lifecycle events.
        /// </summary>
        public void SetCameraCallback(CameraCallback? callback)
        {
            _cameraCallback = callback;
        }

        // ---- Cleanup ----

        /// <summary>
        /// Get the native BarkoderNET library version.
        /// </summary>
        public static string GetLibVersion() => BarcodeDecoder.GetLibVersion();

        /// <summary>
        /// Check if the native decoder is currently busy processing a frame.
        /// </summary>
        public static bool IsDecoderBusy() => BarcodeDecoder.IsDecoderBusy();

        /// <summary>
        /// Check if a string is VIN compliant.
        /// </summary>
        public static bool IsStringVINCompliant(string vinString, int checkLevel = 1)
            => BarcodeDecoder.IsStringVINCompliant(vinString, checkLevel);

        /// <summary>
        /// Get the device ID from the native decoder (for licensing).
        /// </summary>
        public string? GetDeviceId() => _decoder.GetDeviceId();

        /// <summary>
        /// Get license info string.
        /// </summary>
        public string? GetLicenseInfo() => _decoder.GetLicenseInfo();

        /// <summary>
        /// Get license error message if any.
        /// </summary>
        public string? GetLicenseErrorMessage() => _decoder.GetLicenseErrorMessage();

        /// <summary>
        /// Set a custom native decoder option by name.
        /// </summary>
        public int SetCustomOption(string option, int value) => _decoder.SetCustomOption(option, value);

        /// <summary>
        /// Get a custom native decoder option by name.
        /// </summary>
        public int GetCustomOption(string option) => _decoder.GetCustomOption(option);

        /// <summary>
        /// Build an OverlayData from the current config and latest decode results.
        /// Attach this to CameraPreview.Overlay before calling UpdateFrame.
        /// </summary>
        public OverlayData? BuildOverlay()
        {
            if (_config == null) return null;

            var overlay = new OverlayData();

            // Freeze state
            overlay.IsFrozen = _isFrozen;

            // ROI center mark controls center-only scan mode and should be visible on preview.
            overlay.RoiCenterMark = _config.DecoderConfig.RoiCenterMark;

            // Keep ROI geometry available even when ROI border is hidden so the center marker
            // can still be positioned correctly.
            var roi = _config.GetRegionOfInterest();
            overlay.Roi = (roi.Left, roi.Top, roi.Width, roi.Height);

            // ROI
            if (_config.RegionOfInterestVisible)
            {
                overlay.RoiVisible = true;
                overlay.RoiLineWidth = (int)_config.RoiLineWidth;
                overlay.RoiColor = ParseArgbToBgra(_config.RoiLineColor);
                overlay.RoiDimColor = ParseArgbToBgra(_config.RoiOverlayBackgroundColor);
            }

            // AR mode overlay
            if (_config.ARConfig.ARMode != BarkoderARMode.Off && _arManager != null)
            {
                overlay.AREnabled = true;
                overlay.LocationEnabled = false; // Disable standard location when AR is active

                var snapshot = _arManager.GetSnapshot(_config.ARConfig.DisplayOnlyMatchedResults);
                var arConfig = _config.ARConfig;

                overlay.ARConfig = new AROverlayConfig
                {
                    SelectedColor = ParseArgbToBgra(arConfig.SelectedLocationColor),
                    NonSelectedColor = ParseArgbToBgra(arConfig.NonSelectedLocationColor),
                    SelectedLineWidth = (int)arConfig.SelectedLocationLineWidth,
                    NonSelectedLineWidth = (int)arConfig.NonSelectedLocationLineWidth,
                    LocationType = arConfig.LocationType,
                    HeaderShowMode = arConfig.HeaderShowMode,
                    HeaderHeight = arConfig.HeaderHeight,
                    HeaderTextColorSelected = ParseArgbToBgra(arConfig.HeaderTextColorSelected),
                    HeaderTextColorNonSelected = ParseArgbToBgra(arConfig.HeaderTextColorNonSelected)
                };

                overlay.AREntries = snapshot.Select(e => new AROverlayEntry
                {
                    Left = e.CurrentLocation.Left,
                    Top = e.CurrentLocation.Top,
                    Right = e.CurrentLocation.Right,
                    Bottom = e.CurrentLocation.Bottom,
                    P0 = e.CurrentLocation.P0,
                    P1 = e.CurrentLocation.P1,
                    P2 = e.CurrentLocation.P2,
                    P3 = e.CurrentLocation.P3,
                    HasPolygon = e.CurrentLocation.HasPolygon,
                    IsSelected = e.IsSelected,
                    Alpha = e.Alpha,
                    HeaderText = FormatARHeaderText(arConfig.HeaderTextFormat, e)
                }).ToArray();
            }
            else
            {
                // Standard location overlay
                if (_config.LocationInPreviewEnabled)
                {
                    overlay.LocationEnabled = true;
                    overlay.LocationLineWidth = (int)_config.LocationLineWidth;
                    overlay.LocationColor = ParseArgbToBgra(_config.LocationLineColor);

                    // Show locations for 500ms after last decode
                    if (_lastLocations != null && (DateTime.UtcNow - _lastLocationTime).TotalMilliseconds < 500)
                    {
                        overlay.Locations = _lastLocations;
                    }
                }
            }

            return overlay;
        }

        /// <summary>
        /// Format the AR header text using the template and cache entry data.
        /// Supports [barcode_text] and [barcode_type] placeholders.
        /// </summary>
        private static string FormatARHeaderText(string format, ARCacheEntry entry)
        {
            if (string.IsNullOrEmpty(format)) return entry.TextualData;

            var text = format
                .Replace("[barcode_text]", entry.TextualData)
                .Replace("[barcode_type]", entry.BarcodeTypeName);

            // Replace extra key placeholders if the result has extras
            if (entry.Result?.Extra != null)
            {
                foreach (var kv in entry.Result.Extra)
                {
                    text = text.Replace($"[{kv.Key}]", kv.Value);
                }
            }

            return text;
        }

        /// <summary>
        /// Toggle AR selection for a barcode at the given pixel coordinates.
        /// In streaming mode, emits updated selected results via the callback.
        /// Returns true if a barcode was found and toggled.
        /// </summary>
        public bool ARToggleSelectionAt(float x, float y)
        {
            if (_arManager == null || _config?.ARConfig.ARMode == BarkoderARMode.Off)
                return false;

            // NonInteractive and MatchFilter: manual taps are ignored.
            // MatchFilter selection is controlled only by native match state.
            if (_config.ARConfig.ARMode == BarkoderARMode.NonInteractive ||
                _config.ARConfig.ARMode == BarkoderARMode.MatchFilter)
                return false;

            var key = _arManager.HitTest(x, y);
            if (key == null)
                return false;

            var (found, nowSelected) = _arManager.ToggleSelection(key, _config.ARConfig);
            if (!found)
                return false;

            // Play beep on selection
            if (nowSelected && _config.BeepOnSuccessEnabled)
                PlayBeepSound();

            // Streaming mode: emit updated selection
            if (!_config.ARConfig.EmitResultsAtSessionEndOnly)
            {
                EmitARSelectedResults(null, LastFrameWidth, LastFrameHeight);
            }

            // Check result limit after selection
            if (nowSelected && _config.ARConfig.ResultLimit > 0 &&
                _arManager.SelectedCount >= _config.ARConfig.ResultLimit)
            {
                if (!_config.ARConfig.ContinueScanningOnLimit)
                {
                    if (_config.ARConfig.EmitResultsAtSessionEndOnly)
                        EmitARSelectedResults(null, LastFrameWidth, LastFrameHeight);

                    _state = BarkoderState.Stopped;
                    _ = Task.Run(async () => await StopScanning());
                }
            }

            return true;
        }

        /// <summary>
        /// Get all selected AR results.
        /// </summary>
        public BarcodeResult[] GetARSelectedResults()
        {
            return _arManager?.GetSelectedResults() ?? Array.Empty<BarcodeResult>();
        }

        /// <summary>
        /// Reset the AR session (clear all cached barcodes).
        /// </summary>
        public void ResetARSession()
        {
            _arManager?.Clear();
        }

        /// <summary>\n        /// Freeze the AR scanning: captures the current frame and stops decoding,\n        /// but keeps AR overlays visible and interactive for selection.\n        /// Shows a darkened frozen frame. Another call to UnfreezeScanning() resumes.\n        /// </summary>
        public void FreezeScanning()
        {
            if (_config?.ARConfig.ARMode == BarkoderARMode.Off) return;
            if (_config?.ARConfig.ARMode == BarkoderARMode.NonInteractive ||
                _config?.ARConfig.ARMode == BarkoderARMode.MatchFilter) return;
            if (_isFrozen) return;

            // Capture a copy of the current frame
            if (_lastFrameData != null)
            {
                _frozenFrameData = new byte[_lastFrameData.Length];
                Buffer.BlockCopy(_lastFrameData, 0, _frozenFrameData, 0, _lastFrameData.Length);
            }
            _frozenFrameWidth = LastFrameWidth;
            _frozenFrameHeight = LastFrameHeight;
            _isFrozen = true;
        }

        /// <summary>
        /// Unfreeze the AR scanning: resumes live camera feed and decoding.
        /// </summary>
        public void UnfreezeScanning()
        {
            if (!_isFrozen) return;

            _isFrozen = false;
            _frozenFrameData = null;
        }

        /// <summary>
        /// Toggle freeze/unfreeze state.
        /// Returns true if now frozen, false if now unfrozen.
        /// </summary>
        public bool ToggleFreeze()
        {
            if (_isFrozen)
            {
                UnfreezeScanning();
                return false;
            }
            else
            {
                FreezeScanning();
                return _isFrozen;
            }
        }

        /// <summary>
        /// Emit all currently selected AR results via the result callback.
        /// </summary>
        private void EmitARSelectedResults(byte[]? frameData, int width, int height)
        {
            if (_resultCallback == null || _arManager == null) return;

            BarcodeResult[] selectedResults;

            // MatchFilter mode: overlay selection is matched-only, but callback payload depends on
            // ReturnOnlyMatchedResults.
            if (_config?.ARConfig.ARMode == BarkoderARMode.MatchFilter &&
                _config.ARConfig.ReturnOnlyMatchedResults == false)
            {
                selectedResults = _arManager
                    .GetSnapshot(displayOnlyMatchedResults: false)
                    .Where(e => e.Result != null)
                    .Select(e => e.Result!)
                    .ToArray();
            }
            else
            {
                selectedResults = _arManager.GetSelectedResults();
            }

            if (selectedResults.Length == 0) return;

            if (_config?.ARConfig.ReturnOnlyMatchedResults == true)
            {
                selectedResults = selectedResults.Where(r => r.IsMatched).ToArray();
                if (selectedResults.Length == 0)
                    return;
            }

            // Use last stored frame if no frame provided
            var frame = frameData ?? _lastFrameData;

            // Build thumbnails if configured
            byte[][]? thumbnails = null;
            if (_config?.ARConfig.BarcodeThumbnailOnResult == true && frame != null)
            {
                thumbnails = CropThumbnails(selectedResults, frame, width, height);
            }

            // Build result image if configured
            byte[]? resultImage = null;
            if (_config?.ARConfig.ImageResultEnabled == true && frame != null)
            {
                resultImage = new byte[frame.Length];
                Buffer.BlockCopy(frame, 0, resultImage, 0, frame.Length);

                // Draw location overlays on the result image if configured
                if (_config.LocationInImageResultEnabled)
                {
                    DrawLocationsOnFrame(resultImage, width, height, selectedResults, _config);
                }
            }

            DeliverResults(selectedResults, thumbnails, resultImage, width, height);
        }

        /// <summary>
        /// Parse an ARGB hex color string (e.g. "#80FF0000") into a BGRA tuple.
        /// </summary>
        private static (byte B, byte G, byte R, byte A) ParseArgbToBgra(string argbHex)
        {
            if (string.IsNullOrEmpty(argbHex)) return (0, 0, 0, 255);
            argbHex = argbHex.TrimStart('#');
            if (argbHex.Length == 8) // AARRGGBB
            {
                byte a = Convert.ToByte(argbHex[..2], 16);
                byte r = Convert.ToByte(argbHex[2..4], 16);
                byte g = Convert.ToByte(argbHex[4..6], 16);
                byte b = Convert.ToByte(argbHex[6..8], 16);
                return (b, g, r, a);
            }
            if (argbHex.Length == 6) // RRGGBB
            {
                byte r = Convert.ToByte(argbHex[..2], 16);
                byte g = Convert.ToByte(argbHex[2..4], 16);
                byte b = Convert.ToByte(argbHex[4..6], 16);
                return (b, g, r, 255);
            }
            return (0, 0, 0, 255);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _scanCts?.Cancel();
                _cameraManager.Dispose();
                _decoder.Dispose();
            }
        }

        // ---- Private ----

        private void OnFrameArrived(byte[] frameData, int width, int height)
        {
            if (_state != BarkoderState.Scanning || _scanCts?.IsCancellationRequested == true)
                return;

            LastFrameWidth = width;
            LastFrameHeight = height;
            _lastFrameData = frameData;

            // When frozen, skip decoding but still render the frozen frame with AR overlays
            if (_isFrozen)
            {
                return;
            }

            try
            {
                var results = _decoder.Decode(frameData, width, height);
            
                // Report performance
                _performanceCallback?.Invoke(_cameraManager.CurrentFps, _decoder.CurrentDps);

                if (results != null && results.Length > 0)
                {
                    // Update location overlay data
                    var locs = results
                        .Where(r => r.Location != null)
                        .Select(r => r.Location!)
                        .ToArray();
                    if (locs.Length > 0)
                    {
                        _lastLocations = locs;
                        _lastLocationTime = DateTime.UtcNow;
                    }

                    // AR mode: update cache, skip normal result delivery
                    if (_config?.ARConfig.ARMode != BarkoderARMode.Off)
                    {
                        _arManager ??= new BarkoderARManager();
                        var newlySelected = _arManager.UpdateResults(results, _config.ARConfig);
                        _arManager.InterpolatePositions(_config.ARConfig.LocationTransitionSpeed);

                        // Streaming mode: emit when new barcodes are auto-selected
                        if (!_config.ARConfig.EmitResultsAtSessionEndOnly && newlySelected.Count > 0)
                        {
                            EmitARSelectedResults(frameData, width, height);
                        }

                        // Check result limit
                        if (_config.ARConfig.ResultLimit > 0 &&
                            _arManager.SelectedCount >= _config.ARConfig.ResultLimit)
                        {
                            if (!_config.ARConfig.ContinueScanningOnLimit)
                            {
                                if (_config.ARConfig.EmitResultsAtSessionEndOnly)
                                    EmitARSelectedResults(frameData, width, height);

                                _state = BarkoderState.Stopped;
                                _ = Task.Run(async () => await StopScanning());
                            }
                        }

                        return; // Don't fall through to normal result delivery
                    }

                    // --- Normal (non-AR) scanning flow ---

                    // Filter duplicates
                    var filteredResults = FilterDuplicates(results);
                    if (filteredResults.Length == 0)
                        return;

                    if (_config.DecoderConfig.ReturnOnlyMatchedResults)
                    {
                        filteredResults = filteredResults.Where(r => r.IsMatched).ToArray();
                        if (filteredResults.Length == 0)
                            return;
                    }

                    // Build result image if configured
                    byte[]? resultImage = null;
                    if (_config?.ImageResultEnabled == true)
                    {
                        if (_config.LocationInImageResultEnabled)
                        {
                            // Clone frame and draw location polygons on it
                            resultImage = new byte[frameData.Length];
                            Buffer.BlockCopy(frameData, 0, resultImage, 0, frameData.Length);
                            DrawLocationsOnFrame(resultImage, width, height, filteredResults, _config);
                        }
                        else
                        {
                            resultImage = frameData;
                        }
                    }

                    // Build thumbnails from barcode locations
                    byte[][]? thumbnails = null;
                    if (_config?.BarcodeThumbnailOnResult == true)
                    {
                        thumbnails = CropThumbnails(filteredResults, frameData, width, height);
                    }

                    // Extract MRZ sub-images (picture, signature, document, main)
                    var mrzImages = ExtractMrzImages(filteredResults, frameData, width, height);

                    // Play beep sound before delivering results
                    if (_config?.BeepOnSuccessEnabled == true)
                    {
                        PlayBeepSound();
                    }

                    DeliverResults(filteredResults, thumbnails, resultImage, width, height, mrzImages);

                    // Auto-close session if configured
                    if (_config?.CloseSessionOnResultEnabled == true)
                    {
                        _state = BarkoderState.Stopped;
                        _ = Task.Run(async () => await StopScanning());
                    }
                }
                else if (_config?.ARConfig.ARMode != BarkoderARMode.Off && _arManager != null)
                {
                    // No results this frame — still update AR cache for fade-out
                    _arManager.UpdateResults(Array.Empty<BarcodeResult>(), _config.ARConfig);
                    _arManager.InterpolatePositions(_config.ARConfig.LocationTransitionSpeed);
                }
            }
            catch
            {
                // Swallow decode errors to keep scanning
            }
        }

        private BarcodeResult[] FilterDuplicates(BarcodeResult[] results)
        {
            if (_config == null) return results;

            var thresholdSeconds = _config.ThresholdBetweenDuplicatesScans;
            var now = DateTime.UtcNow;
            var filtered = new List<BarcodeResult>();

            foreach (var result in results)
            {
                var key = $"{result.BarcodeTypeName}:{result.TextualData}";

                if (_recentScans.TryGetValue(key, out var lastTime))
                {
                    // Duplicate found — check threshold
                    if (thresholdSeconds < 0)
                    {
                        // Negative threshold: never accept duplicates again
                        continue;
                    }

                    var elapsed = now - lastTime;
                    if (elapsed.TotalSeconds > thresholdSeconds)
                    {
                        // Enough time has passed — re-accept and update timestamp
                        _recentScans[key] = now;
                        filtered.Add(result);
                    }
                    // else: too soon, skip
                }
                else
                {
                    // Completely new barcode
                    _recentScans[key] = now;
                    filtered.Add(result);
                }
            }

            return filtered.ToArray();
        }

        private static void DrawLocationsOnFrame(byte[] pixels, int width, int height, BarcodeResult[] results, BarkoderConfig config)
        {
            var color = ParseArgbToBgra(config.LocationLineColor);
            int baseThickness = Math.Max(1, (int)config.LocationLineWidth);

            // Scale thickness proportionally to image size so lines survive display downscaling
            int largerDim = Math.Max(width, height);
            int thickness = largerDim > 1000
                ? Math.Max(baseThickness, (int)(baseThickness * (largerDim / 1000.0)))
                : baseThickness;

            foreach (var result in results)
            {
                // Draw main barcode location
                var loc = result.Location;
                if (loc != null)
                {
                    DrawLocationPolygon(pixels, width, height, loc, color, thickness);
                }

                // Draw MRZ zone locations (picture, signature, document, etc.)
                if (result.MrzLocations != null)
                {
                    foreach (var (name, zoneLoc) in result.MrzLocations)
                    {
                        DrawLocationPolygon(pixels, width, height, zoneLoc, color, thickness);
                    }
                }
            }
        }

        private static void DrawLocationPolygon(byte[] pixels, int width, int height,
            BarcodeLocation loc, (byte B, byte G, byte R, byte A) color, int thickness)
        {
            DrawLineOnBuffer(pixels, width, height, loc.TopLeft, loc.TopRight, color, thickness);
            DrawLineOnBuffer(pixels, width, height, loc.TopRight, loc.BottomRight, color, thickness);
            DrawLineOnBuffer(pixels, width, height, loc.BottomRight, loc.BottomLeft, color, thickness);
            DrawLineOnBuffer(pixels, width, height, loc.BottomLeft, loc.TopLeft, color, thickness);
        }

        private static void DrawLineOnBuffer(byte[] pixels, int w, int h,
            Models.PointF p1, Models.PointF p2,
            (byte B, byte G, byte R, byte A) color, int thickness)
        {
            // Use floating-point DDA line algorithm for gap-free thick lines
            float x0 = p1.X, y0 = p1.Y;
            float x1 = p2.X, y1 = p2.Y;
            float fdx = x1 - x0, fdy = y1 - y0;
            int steps = (int)Math.Max(Math.Abs(fdx), Math.Abs(fdy));
            if (steps == 0) steps = 1;
            float xInc = fdx / steps;
            float yInc = fdy / steps;
            int half = thickness / 2;
            float a = color.A / 255f;
            float invA = 1f - a;

            float cx = x0, cy = y0;
            for (int s = 0; s <= steps; s++)
            {
                int ix = (int)Math.Round(cx);
                int iy = (int)Math.Round(cy);
                for (int ty = -half; ty <= half; ty++)
                    for (int tx = -half; tx <= half; tx++)
                    {
                        int px = ix + tx, py = iy + ty;
                        if (px >= 0 && px < w && py >= 0 && py < h)
                        {
                            int i = (py * w + px) * 4;
                            pixels[i] = (byte)(pixels[i] * invA + color.B * a);
                            pixels[i + 1] = (byte)(pixels[i + 1] * invA + color.G * a);
                            pixels[i + 2] = (byte)(pixels[i + 2] * invA + color.R * a);
                            pixels[i + 3] = 255;
                        }
                    }
                cx += xInc;
                cy += yInc;
            }
        }

        private static byte[][]? CropThumbnails(BarcodeResult[] results, byte[] frameData, int width, int height)
        {
            var thumbnails = new List<byte[]>();
            int stride = width * 4;

            foreach (var result in results)
            {
                var loc = result.Location;
                if (loc == null) continue;

                // Compute bounding box from the four corner points
                float minX = Math.Min(Math.Min(loc.TopLeft.X, loc.TopRight.X), Math.Min(loc.BottomLeft.X, loc.BottomRight.X));
                float maxX = Math.Max(Math.Max(loc.TopLeft.X, loc.TopRight.X), Math.Max(loc.BottomLeft.X, loc.BottomRight.X));
                float minY = Math.Min(Math.Min(loc.TopLeft.Y, loc.TopRight.Y), Math.Min(loc.BottomLeft.Y, loc.BottomRight.Y));
                float maxY = Math.Max(Math.Max(loc.TopLeft.Y, loc.TopRight.Y), Math.Max(loc.BottomLeft.Y, loc.BottomRight.Y));

                // Add 10% padding
                float pw = (maxX - minX) * 0.1f;
                float ph = (maxY - minY) * 0.1f;

                int x0 = Math.Max(0, (int)(minX - pw));
                int y0 = Math.Max(0, (int)(minY - ph));
                int x1 = Math.Min(width, (int)(maxX + pw));
                int y1 = Math.Min(height, (int)(maxY + ph));

                int tw = x1 - x0;
                int th = y1 - y0;
                if (tw <= 0 || th <= 0) continue;

                // Crop: copy BGRA8 rows into a new buffer prefixed with width/height
                // Format: [4 bytes width LE][4 bytes height LE][BGRA8 pixel data]
                var thumbData = new byte[8 + tw * th * 4];
                BitConverter.GetBytes(tw).CopyTo(thumbData, 0);
                BitConverter.GetBytes(th).CopyTo(thumbData, 4);

                for (int row = 0; row < th; row++)
                {
                    int srcOffset = (y0 + row) * stride + x0 * 4;
                    int dstOffset = 8 + row * tw * 4;
                    Buffer.BlockCopy(frameData, srcOffset, thumbData, dstOffset, tw * 4);
                }

                thumbnails.Add(thumbData);
            }

            return thumbnails.Count > 0 ? thumbnails.ToArray() : null;
        }

        private static void PlayBeepSound()
        {
#if WINDOWS
            try
            {
                if (_beepFilePath == null)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = assembly.GetManifestResourceNames()
                        .FirstOrDefault(n => n.EndsWith("beep.wav", StringComparison.OrdinalIgnoreCase));

                    if (resourceName == null) return;

                    var tempPath = Path.Combine(Path.GetTempPath(), "barkoder_beep.wav");
                    using var stream = assembly.GetManifestResourceStream(resourceName);
                    if (stream == null) return;

                    using var fs = File.Create(tempPath);
                    stream.CopyTo(fs);
                    _beepFilePath = tempPath;
                }

                var player = new MediaPlayer();
                player.Source = MediaSource.CreateFromUri(new Uri(_beepFilePath));
                player.MediaEnded += (s, e) => player.Dispose();
                player.MediaFailed += (s, e) => player.Dispose();
                player.Play();
            }
            catch
            {
                // Silently ignore beep errors
            }
#endif
        }

        private void DeliverResults(BarcodeResult[] results, byte[][]? rawThumbnails, byte[]? rawResultImage, int frameWidth, int frameHeight,
            List<(string Name, byte[] RawData)>? mrzImages = null)
        {
            if (_resultCallback == null) return;

            // Convert raw BGRA8 to ImageSource on the UI thread
            if (_syncContext != null)
            {
                _syncContext.Post(async _ =>
                {
                    var resultImage = rawResultImage != null
                        ? await Bgra8ToImageSourceAsync(rawResultImage, frameWidth, frameHeight)
                        : null;

                    ImageSource[]? thumbnails = null;
                    if (rawThumbnails != null)
                    {
                        thumbnails = new ImageSource[rawThumbnails.Length];
                        for (int i = 0; i < rawThumbnails.Length; i++)
                        {
                            var thumb = rawThumbnails[i];
                            if (thumb.Length < 8) continue;
                            int tw = BitConverter.ToInt32(thumb, 0);
                            int th = BitConverter.ToInt32(thumb, 4);
                            var pixels = new byte[thumb.Length - 8];
                            Buffer.BlockCopy(thumb, 8, pixels, 0, pixels.Length);
                            thumbnails[i] = await Bgra8ToImageSourceAsync(pixels, tw, th);
                        }
                    }

                    // Convert MRZ images to BKImageDescriptors
                    await PopulateMrzImageDescriptors(results, mrzImages);

                    _resultCallback?.Invoke(results, thumbnails, resultImage);
                }, null);
            }
            else
            {
                _resultCallback.Invoke(results, null, null);
            }
        }

        /// <summary>
        /// Convert raw BGRA8 pixel data to a MAUI ImageSource.
        /// </summary>
        private static async Task<ImageSource?> Bgra8ToImageSourceAsync(byte[] bgra8, int width, int height)
        {
            if (width <= 0 || height <= 0 || bgra8.Length < width * height * 4)
                return null;

#if WINDOWS
            var bitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, width, height, BitmapAlphaMode.Premultiplied);
            bitmap.CopyFromBuffer(bgra8.AsBuffer());

            using var stream = new global::Windows.Storage.Streams.InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
            encoder.SetSoftwareBitmap(bitmap);
            await encoder.FlushAsync();
            bitmap.Dispose();

            stream.Seek(0);
            var bytes = new byte[stream.Size];
            await stream.ReadAsync(bytes.AsBuffer(), (uint)stream.Size, global::Windows.Storage.Streams.InputStreamOptions.None);

            return ImageSource.FromStream(() => new MemoryStream(bytes));
#else
            return null;
#endif
        }

        // ---- MRZ Image Extraction Helpers ----

        /// <summary>
        /// Crop a rectangular region from a BGRA8 frame based on a BarcodeLocation polygon.
        /// Returns raw BGRA8 data prefixed with [width:int32LE][height:int32LE], same format as CropThumbnails.
        /// </summary>
        private static byte[]? CropToLocation(byte[] frameData, int frameWidth, int frameHeight, BarcodeLocation loc)
        {
            float minX = Math.Min(Math.Min(loc.TopLeft.X, loc.TopRight.X), Math.Min(loc.BottomLeft.X, loc.BottomRight.X));
            float maxX = Math.Max(Math.Max(loc.TopLeft.X, loc.TopRight.X), Math.Max(loc.BottomLeft.X, loc.BottomRight.X));
            float minY = Math.Min(Math.Min(loc.TopLeft.Y, loc.TopRight.Y), Math.Min(loc.BottomLeft.Y, loc.BottomRight.Y));
            float maxY = Math.Max(Math.Max(loc.TopLeft.Y, loc.TopRight.Y), Math.Max(loc.BottomLeft.Y, loc.BottomRight.Y));

            int x0 = Math.Max(0, (int)minX);
            int y0 = Math.Max(0, (int)minY);
            int x1 = Math.Min(frameWidth, (int)Math.Ceiling(maxX));
            int y1 = Math.Min(frameHeight, (int)Math.Ceiling(maxY));

            int tw = x1 - x0;
            int th = y1 - y0;
            if (tw <= 0 || th <= 0) return null;

            int stride = frameWidth * 4;
            var data = new byte[8 + tw * th * 4];
            BitConverter.GetBytes(tw).CopyTo(data, 0);
            BitConverter.GetBytes(th).CopyTo(data, 4);

            for (int row = 0; row < th; row++)
            {
                int srcOffset = (y0 + row) * stride + x0 * 4;
                int dstOffset = 8 + row * tw * 4;
                Buffer.BlockCopy(frameData, srcOffset, data, dstOffset, tw * 4);
            }

            return data;
        }

        /// <summary>
        /// Extract MRZ sub-images (picture, signature, document, main) from frame data for IDDocument results.
        /// Returns a list of (name, rawBgra8WithHeader) tuples.
        /// </summary>
        private static List<(string Name, byte[] RawData)>? ExtractMrzImages(
            BarcodeResult[] results, byte[] frameData, int width, int height)
        {
            List<(string, byte[])>? allImages = null;

            foreach (var result in results)
            {
                if (result.BarcodeTypeName != "MRZ" || result.MrzLocations == null || result.MrzLocations.Count == 0)
                    continue;

                var images = new List<(string, byte[])>();

                foreach (var (name, loc) in result.MrzLocations)
                {
                    var cropped = CropToLocation(frameData, width, height, loc);
                    if (cropped != null)
                        images.Add((name, cropped));
                }

                // Also add main thumbnail from the barcode location itself
                if (result.Location != null)
                {
                    var mainCrop = CropToLocation(frameData, width, height, result.Location);
                    if (mainCrop != null && !images.Any(i => i.Item1 == "main"))
                        images.Add(("main", mainCrop));
                }

                if (images.Count > 0)
                {
                    allImages ??= new List<(string, byte[])>();
                    allImages.AddRange(images);
                }
            }

            return allImages;
        }

        /// <summary>
        /// Scale down BGRA8 raw pixel data using nearest-neighbor interpolation.
        /// </summary>
        private static byte[] ScaleBgra8(byte[] src, int srcW, int srcH, int dstW, int dstH)
        {
            var dst = new byte[dstW * dstH * 4];
            float xRatio = (float)srcW / dstW;
            float yRatio = (float)srcH / dstH;

            for (int y = 0; y < dstH; y++)
            {
                int srcY = Math.Min((int)(y * yRatio), srcH - 1);
                for (int x = 0; x < dstW; x++)
                {
                    int srcX = Math.Min((int)(x * xRatio), srcW - 1);
                    int srcIdx = (srcY * srcW + srcX) * 4;
                    int dstIdx = (y * dstW + x) * 4;
                    dst[dstIdx] = src[srcIdx];
                    dst[dstIdx + 1] = src[srcIdx + 1];
                    dst[dstIdx + 2] = src[srcIdx + 2];
                    dst[dstIdx + 3] = src[srcIdx + 3];
                }
            }

            return dst;
        }

        /// <summary>
        /// Deliver ScanImage results with ImageSource conversion on UI thread.
        /// </summary>
        private static void DeliverScanImageResults(
            SynchronizationContext? syncContext,
            BarkoderResultCallback resultCallback,
            BarcodeResult[] results,
            byte[][]? rawThumbnails,
            byte[]? rawResultImage,
            int imgWidth,
            int imgHeight,
            List<(string Name, byte[] RawData)>? mrzImages = null)
        {
            if (syncContext != null)
            {
                syncContext.Post(async _ =>
                {
                    try
                    {
                    var resultImage = rawResultImage != null
                        ? await Bgra8ToImageSourceAsync(rawResultImage, imgWidth, imgHeight)
                        : null;

                    ImageSource[]? thumbnails = null;
                    if (rawThumbnails != null)
                    {
                        thumbnails = new ImageSource[rawThumbnails.Length];
                        for (int i = 0; i < rawThumbnails.Length; i++)
                        {
                            var thumb = rawThumbnails[i];
                            if (thumb.Length < 8) continue;
                            int tw = BitConverter.ToInt32(thumb, 0);
                            int th = BitConverter.ToInt32(thumb, 4);
                            var px = new byte[thumb.Length - 8];
                            Buffer.BlockCopy(thumb, 8, px, 0, px.Length);
                            thumbnails[i] = await Bgra8ToImageSourceAsync(px, tw, th);
                        }
                    }

                    // Convert MRZ images to BKImageDescriptors
                    await PopulateMrzImageDescriptors(results, mrzImages);

                    resultCallback(results, thumbnails, resultImage);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Barkoder] DeliverScanImageResults error: {ex}");
                        resultCallback(results, null, null);
                    }
                }, null);
            }
            else
            {
                resultCallback(results, null, null);
            }
        }

        /// <summary>
        /// Simple callback delivery without image conversion (for empty/error results).
        /// </summary>
        private static void DeliverScanImageCallback(
            SynchronizationContext? syncContext,
            BarkoderResultCallback resultCallback,
            BarcodeResult[] results,
            ImageSource[]? thumbnails,
            ImageSource? resultImage)
        {
            if (syncContext != null)
            {
                syncContext.Post(_ => resultCallback(results, thumbnails, resultImage), null);
            }
            else
            {
                resultCallback(results, thumbnails, resultImage);
            }
        }

        /// <summary>
        /// Convert raw MRZ image data to BKImageDescriptor objects and assign to the MRZ results.
        /// Must be called on UI thread (for ImageSource creation).
        /// </summary>
        private static async Task PopulateMrzImageDescriptors(
            BarcodeResult[] results, List<(string Name, byte[] RawData)>? mrzImages)
        {
            if (mrzImages == null || mrzImages.Count == 0)
                return;

            var descriptors = new List<BKImageDescriptor>();
            foreach (var (name, rawData) in mrzImages)
            {
                if (rawData.Length < 8) continue;
                int tw = BitConverter.ToInt32(rawData, 0);
                int th = BitConverter.ToInt32(rawData, 4);
                var pixels = new byte[rawData.Length - 8];
                Buffer.BlockCopy(rawData, 8, pixels, 0, pixels.Length);
                var imgSrc = await Bgra8ToImageSourceAsync(pixels, tw, th);
                descriptors.Add(new BKImageDescriptor(name, imgSrc));
            }

            if (descriptors.Count > 0)
            {
                // Assign to the first MRZ result
                var mrzResult = results.FirstOrDefault(r => r.BarcodeTypeName == "MRZ");
                if (mrzResult != null)
                    mrzResult.Images = descriptors;
            }
        }

        private void EnsureNotDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(BarkoderView));
        }

        private void EnsureConfigSet()
        {
            if (_config == null)
                throw new InvalidOperationException("Config must be set before starting the scanner. Set the Config property first.");
        }
    }
}
