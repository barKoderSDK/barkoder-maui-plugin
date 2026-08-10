using Barkoder.Windows.Config;
using Barkoder.Windows.Models;

namespace Barkoder.Windows.Camera
{
    /// <summary>
    /// Internal camera management layer that delegates to an ICameraProvider.
    /// Handles frame callbacks and preview frame distribution.
    /// </summary>
    internal class CameraManager : IDisposable
    {
        private ICameraProvider? _provider;
        private bool _isDisposed;
        private BarkoderPreviewFramesCallback? _previewFramesCallback;

        /// <summary>
        /// Whether the camera is initialized.
        /// </summary>
        public bool IsInitialized => _provider?.IsInitialized == true;

        /// <summary>
        /// Current FPS from the camera.
        /// </summary>
        public double CurrentFps => _provider?.CurrentFps ?? 0;

        /// <summary>
        /// The underlying camera provider (for UI binding, e.g., MediaCapture).
        /// </summary>
        public ICameraProvider? Provider => _provider;

        /// <summary>
        /// Set the camera provider implementation.
        /// Must be called before InitializeAsync.
        /// </summary>
        public void SetProvider(ICameraProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Initialize the camera with the given configuration.
        /// </summary>
        public async Task InitializeAsync(BarkoderConfig config)
        {
            EnsureProvider();
            await _provider!.InitializeAsync(config.BarkoderResolution);
        }

        /// <summary>
        /// Start reading frames from the camera.
        /// The callback receives BGRA8 pixel data, width, and height.
        /// </summary>
        public async Task StartFrameReaderAsync(Action<byte[], int, int> frameCallback)
        {
            EnsureProvider();

            // Wrap to deliver preview frames immediately and decode on background thread.
            // Decoding is synchronous and can take ~200ms; running it inline would block
            // the MediaFrameReader callback and limit both FPS and DPS to ~5.
            // BarcodeDecoder._isDecoding safely skips frames if already busy.
            Action<byte[], int, int> wrappedCallback = (data, w, h) =>
            {
                _previewFramesCallback?.Invoke(data, w, h);
                ThreadPool.QueueUserWorkItem(_ => frameCallback(data, w, h));
            };

            await _provider!.StartFrameDeliveryAsync(wrappedCallback);
        }

        /// <summary>
        /// Stop reading frames.
        /// </summary>
        public async Task StopFrameReaderAsync()
        {
            if (_provider != null)
                await _provider.StopFrameDeliveryAsync();
        }

        /// <summary>
        /// Set a callback for raw preview frames.
        /// </summary>
        public void SetPreviewFramesCallback(BarkoderPreviewFramesCallback? callback)
        {
            _previewFramesCallback = callback;
        }

        /// <summary>
        /// Switch camera position.
        /// </summary>
        public async Task SetCameraAsync(BarkoderCameraPosition position)
        {
            EnsureProvider();
            await _provider!.SetCameraAsync(position);
        }

        public void SetZoomFactor(float zoomFactor)
        {
            _provider?.SetZoomFactor(zoomFactor);
        }

        public float GetMaxZoomFactor() => _provider?.GetMaxZoomFactor() ?? 1.0f;

        public bool IsFlashAvailable() => _provider?.IsFlashAvailable() ?? false;

        public void SetFlashEnabled(bool enabled) => _provider?.SetFlashEnabled(enabled);

        public async Task<byte[]?> CaptureFrameAsync()
        {
            if (_provider == null) return null;
            return await _provider.CaptureFrameAsync();
        }

        /// <summary>
        /// Full cleanup.
        /// </summary>
        public async Task CleanupAsync()
        {
            if (_provider != null)
                await _provider.CleanupAsync();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _provider?.Dispose();
            }
        }

        private void EnsureProvider()
        {
            if (_provider == null)
                throw new InvalidOperationException(
                    "No camera provider set. Call SetCameraProvider() on BarkoderView with an ICameraProvider implementation.");
        }
    }
}
