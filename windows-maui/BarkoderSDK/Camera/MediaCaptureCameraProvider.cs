#if WINDOWS
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.MediaProperties;
using Barkoder.Windows.Models;

namespace Barkoder.Windows.Camera;

public class MediaCaptureCameraProvider : ICameraProvider
{
    private MediaCapture? _mediaCapture;
    private MediaFrameReader? _frameReader;
    private bool _isInitialized;
    private string? _selectedDeviceId;

    private int _frameCount;
    private DateTime _lastFpsTime = DateTime.UtcNow;

    public bool IsInitialized => _isInitialized;
    public double CurrentFps { get; private set; }

    public MediaCapture? MediaCaptureInstance => _mediaCapture;

    /// <summary>
    /// List all available cameras on the system.
    /// Returns CameraDeviceInfo with Id and DisplayName for each video-capable device.
    /// </summary>
    public static async Task<List<CameraDeviceInfo>> GetAvailableCameras()
    {
        var result = new List<CameraDeviceInfo>();
        var groups = await MediaFrameSourceGroup.FindAllAsync();
        foreach (var group in groups)
        {
            bool hasVideo = false;
            foreach (var info in group.SourceInfos)
            {
                if (info.SourceKind == MediaFrameSourceKind.Color ||
                    info.MediaStreamType == MediaStreamType.VideoRecord ||
                    info.MediaStreamType == MediaStreamType.VideoPreview)
                {
                    hasVideo = true;
                    break;
                }
            }
            if (hasVideo)
            {
                result.Add(new CameraDeviceInfo
                {
                    Id = group.Id,
                    DisplayName = group.DisplayName
                });
            }
        }
        return result;
    }

    /// <summary>
    /// Set which camera to use by device ID (from GetAvailableCameras).
    /// The selected camera will be used on the next InitializeAsync call.
    /// </summary>
    public void SetCameraById(string deviceId)
    {
        _selectedDeviceId = deviceId;
    }

    public async Task InitializeAsync(BarkoderResolution resolution)
    {
        if (_isInitialized)
            await CleanupAsync();

        MediaFrameSourceGroup? sourceGroup = null;

        if (!string.IsNullOrEmpty(_selectedDeviceId))
        {
            var groups = await MediaFrameSourceGroup.FindAllAsync();
            foreach (var group in groups)
            {
                if (group.Id == _selectedDeviceId)
                {
                    sourceGroup = group;
                    break;
                }
            }
            if (sourceGroup == null)
                throw new InvalidOperationException($"Camera device not found: {_selectedDeviceId}");
        }
        else
        {
            sourceGroup = await FindSourceGroupAsync();
        }

        if (sourceGroup == null)
            throw new InvalidOperationException("No camera source group found.");

        _mediaCapture = new MediaCapture();
        var settings = new MediaCaptureInitializationSettings
        {
            SourceGroup = sourceGroup,
            SharingMode = MediaCaptureSharingMode.ExclusiveControl,
            StreamingCaptureMode = StreamingCaptureMode.Video,
            MemoryPreference = MediaCaptureMemoryPreference.Cpu
        };

        await _mediaCapture.InitializeAsync(settings);
        await ApplyResolutionAsync(resolution);

        _isInitialized = true;
    }

    public async Task StartFrameDeliveryAsync(Action<byte[], int, int> frameCallback)
    {
        if (!_isInitialized || _mediaCapture == null)
            throw new InvalidOperationException("Camera is not initialized.");

        // Find a color video source from FrameSources
        MediaFrameSource? colorSource = null;
        foreach (var source in _mediaCapture.FrameSources.Values)
        {
            if (source.Info.SourceKind == MediaFrameSourceKind.Color)
            {
                colorSource = source;
                break;
            }
        }

        // Fallback: any video source
        if (colorSource == null)
        {
            foreach (var source in _mediaCapture.FrameSources.Values)
            {
                if (source.Info.MediaStreamType == MediaStreamType.VideoRecord ||
                    source.Info.MediaStreamType == MediaStreamType.VideoPreview)
                {
                    colorSource = source;
                    break;
                }
            }
        }

        if (colorSource == null)
            throw new InvalidOperationException(
                $"No video frame source found. FrameSources count: {_mediaCapture.FrameSources.Count}");

        _frameReader = await _mediaCapture.CreateFrameReaderAsync(
            colorSource,
            MediaEncodingSubtypes.Bgra8);

        // Realtime mode: drop old frames instead of buffering them.
        // Without this, frames queue up and the pipeline falls behind.
        _frameReader.AcquisitionMode = MediaFrameReaderAcquisitionMode.Realtime;

        _lastFpsTime = DateTime.UtcNow;
        _frameCount = 0;

        _frameReader.FrameArrived += (sender, args) =>
        {
            using var frameRef = sender.TryAcquireLatestFrame();
            var softwareBitmap = frameRef?.VideoMediaFrame?.SoftwareBitmap;
            if (softwareBitmap == null) return;

            _frameCount++;
            var now = DateTime.UtcNow;
            var elapsed = (now - _lastFpsTime).TotalSeconds;
            if (elapsed >= 1.0)
            {
                CurrentFps = _frameCount / elapsed;
                _frameCount = 0;
                _lastFpsTime = now;
            }

            // Convert to BGRA8 if needed
            SoftwareBitmap bgra;
            bool needsDispose;
            if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8)
            {
                bgra = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                needsDispose = true;
            }
            else
            {
                bgra = softwareBitmap;
                needsDispose = false;
            }

            try
            {
                int w = bgra.PixelWidth;
                int h = bgra.PixelHeight;
                var buffer = new byte[w * h * 4];
                bgra.CopyToBuffer(buffer.AsBuffer());
                frameCallback(buffer, w, h);
            }
            finally
            {
                if (needsDispose) bgra.Dispose();
            }
        };

        var status = await _frameReader.StartAsync();
        if (status != MediaFrameReaderStartStatus.Success)
            throw new InvalidOperationException($"Failed to start frame reader: {status}");
    }

    public async Task StopFrameDeliveryAsync()
    {
        if (_frameReader != null)
        {
            await _frameReader.StopAsync();
            _frameReader.Dispose();
            _frameReader = null;
        }
    }

    public async Task CleanupAsync()
    {
        await StopFrameDeliveryAsync();
        if (_mediaCapture != null)
        {
            _mediaCapture.Dispose();
            _mediaCapture = null;
        }
        _isInitialized = false;
    }

    public async Task SetCameraAsync(BarkoderCameraPosition position)
    {
        var wasInitialized = _isInitialized;
        await CleanupAsync();
        if (wasInitialized)
            await InitializeAsync(BarkoderResolution.HD);
    }

    public void SetZoomFactor(float zoomFactor)
    {
        if (_mediaCapture?.VideoDeviceController?.ZoomControl?.Supported == true)
        {
            var zoom = _mediaCapture.VideoDeviceController.ZoomControl;
            zoom.Value = Math.Clamp(zoomFactor, zoom.Min, zoom.Max);
        }
    }

    public float GetMaxZoomFactor()
    {
        if (_mediaCapture?.VideoDeviceController?.ZoomControl?.Supported == true)
            return _mediaCapture.VideoDeviceController.ZoomControl.Max;
        return 1.0f;
    }

    public bool IsFlashAvailable()
        => _mediaCapture?.VideoDeviceController?.TorchControl?.Supported == true;

    public void SetFlashEnabled(bool enabled)
    {
        if (_mediaCapture?.VideoDeviceController?.TorchControl?.Supported == true)
            _mediaCapture.VideoDeviceController.TorchControl.Enabled = enabled;
    }

    public Task<byte[]?> CaptureFrameAsync() => Task.FromResult<byte[]?>(null);

    public void Dispose() => _ = CleanupAsync();

    // --- Helpers ---

    private static async Task<MediaFrameSourceGroup?> FindSourceGroupAsync()
    {
        var groups = await MediaFrameSourceGroup.FindAllAsync();
        // Prefer a group with a Color video source
        foreach (var group in groups)
        {
            foreach (var info in group.SourceInfos)
            {
                if (info.SourceKind == MediaFrameSourceKind.Color &&
                    (info.MediaStreamType == MediaStreamType.VideoRecord ||
                     info.MediaStreamType == MediaStreamType.VideoPreview))
                {
                    return group;
                }
            }
        }
        // Fallback: first group with any video source
        foreach (var group in groups)
        {
            foreach (var info in group.SourceInfos)
            {
                if (info.MediaStreamType == MediaStreamType.VideoRecord ||
                    info.MediaStreamType == MediaStreamType.VideoPreview)
                {
                    return group;
                }
            }
        }
        return groups.Count > 0 ? groups[0] : null;
    }

    private async Task ApplyResolutionAsync(BarkoderResolution resolution)
    {
        if (_mediaCapture == null) return;

        uint targetW, targetH;
        switch (resolution)
        {
            case BarkoderResolution.FHD: targetW = 1920; targetH = 1080; break;
            case BarkoderResolution.UHD: targetW = 3840; targetH = 2160; break;
            default: targetW = 1280; targetH = 720; break;
        }

        // Try to set resolution on each color frame source
        foreach (var source in _mediaCapture.FrameSources.Values)
        {
            if (source.Info.SourceKind != MediaFrameSourceKind.Color) continue;

            MediaFrameFormat? bestFormat = null;
            uint bestDiff = uint.MaxValue;
            double bestFps = 0;

            foreach (var format in source.SupportedFormats)
            {
                if (format.VideoFormat == null) continue;

                double fmtFps = format.FrameRate.Denominator > 0
                    ? (double)format.FrameRate.Numerator / format.FrameRate.Denominator
                    : 0;
                System.Diagnostics.Debug.WriteLine(
                    $"[Camera] Available: {format.VideoFormat.Width}x{format.VideoFormat.Height} @ {fmtFps:F1} FPS ({format.Subtype})");

                var diff = (uint)Math.Abs((int)format.VideoFormat.Width - (int)targetW) +
                           (uint)Math.Abs((int)format.VideoFormat.Height - (int)targetH);

                double fps = format.FrameRate.Denominator > 0
                    ? (double)format.FrameRate.Numerator / format.FrameRate.Denominator
                    : 0;

                // Prefer closest resolution, then highest frame rate at that resolution
                if (diff < bestDiff || (diff == bestDiff && fps > bestFps))
                {
                    bestDiff = diff;
                    bestFps = fps;
                    bestFormat = format;
                }
            }

            if (bestFormat != null)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[Camera] Selected format: {bestFormat.VideoFormat?.Width}x{bestFormat.VideoFormat?.Height} @ {bestFps:F1} FPS (subtype: {bestFormat.Subtype})");
                try { await source.SetFormatAsync(bestFormat); } catch { }
            }
            break;
        }
    }
}
#endif
