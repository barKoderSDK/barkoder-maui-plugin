using Barkoder.Windows.Camera;
using Barkoder.Windows.Models;

namespace Barkoder.Windows.UI
{
    /// <summary>
    /// A mock camera provider for testing the BarkoderSDK without a physical camera.
    /// Generates synthetic white frames with a black crosshair pattern.
    /// </summary>
    public class MockCameraProvider : ICameraProvider
    {
        private CancellationTokenSource? _cts;
        private Task? _frameTask;

        private readonly int _width = 640;
        private readonly int _height = 480;
        private readonly int _frameIntervalMs;

        private int _frameCount;
        private DateTime _lastFpsTime = DateTime.UtcNow;

        public bool IsInitialized { get; private set; }
        public double CurrentFps { get; private set; }

        public MockCameraProvider(int frameIntervalMs = 500)
        {
            _frameIntervalMs = frameIntervalMs;
        }

        public Task InitializeAsync(BarkoderResolution resolution)
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task StartFrameDeliveryAsync(Action<byte[], int, int> frameCallback)
        {
            _cts = new CancellationTokenSource();
            _lastFpsTime = DateTime.UtcNow;
            _frameCount = 0;

            _frameTask = Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var frame = GenerateTestFrame(_width, _height);

                        _frameCount++;
                        var now = DateTime.UtcNow;
                        var elapsed = (now - _lastFpsTime).TotalSeconds;
                        if (elapsed >= 1.0)
                        {
                            CurrentFps = _frameCount / elapsed;
                            _frameCount = 0;
                            _lastFpsTime = now;
                        }

                        frameCallback(frame, _width, _height);
                        await Task.Delay(_frameIntervalMs, _cts.Token);
                    }
                    catch (OperationCanceledException) { break; }
                }
            }, _cts.Token);

            return Task.CompletedTask;
        }

        public async Task StopFrameDeliveryAsync()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                if (_frameTask != null)
                    try { await _frameTask; } catch (OperationCanceledException) { }
                _cts.Dispose();
                _cts = null;
            }
        }

        public async Task CleanupAsync()
        {
            await StopFrameDeliveryAsync();
            IsInitialized = false;
        }

        public Task SetCameraAsync(BarkoderCameraPosition position) => Task.CompletedTask;
        public void SetZoomFactor(float zoomFactor) { }
        public float GetMaxZoomFactor() => 5.0f;
        public bool IsFlashAvailable() => false;
        public void SetFlashEnabled(bool enabled) { }
        public Task<byte[]?> CaptureFrameAsync() => Task.FromResult<byte[]?>(null);
        public void SetCameraById(string deviceId) { }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private static byte[] GenerateTestFrame(int width, int height)
        {
            var frame = new byte[width * height * 4];

            // White background
            Array.Fill(frame, (byte)255);

            // Draw black crosshair
            int cx = width / 2, cy = height / 2;
            for (int x = 0; x < width; x++)
            {
                int idx = (cy * width + x) * 4;
                frame[idx] = 0; frame[idx + 1] = 0; frame[idx + 2] = 0;
            }
            for (int y = 0; y < height; y++)
            {
                int idx = (y * width + cx) * 4;
                frame[idx] = 0; frame[idx + 1] = 0; frame[idx + 2] = 0;
            }

            return frame;
        }
    }
}
