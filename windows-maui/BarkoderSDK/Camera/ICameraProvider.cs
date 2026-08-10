using Barkoder.Windows.Models;

namespace Barkoder.Windows.Camera
{
    /// <summary>
    /// Abstraction for camera operations.
    /// 
    /// Platform-specific implementations (e.g., WinUI/MAUI CaptureElement, 
    /// MediaCapture, OpenCV, etc.) should implement this interface.
    /// This allows the SDK to work across different Windows UI frameworks.
    /// </summary>
    public interface ICameraProvider : IDisposable
    {
        /// <summary>
        /// Whether the camera has been initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Current frames per second from the camera.
        /// </summary>
        double CurrentFps { get; }

        /// <summary>
        /// Initialize the camera with the specified resolution.
        /// </summary>
        Task InitializeAsync(BarkoderResolution resolution);

        /// <summary>
        /// Start delivering frames to the callback.
        /// Each frame is provided as BGRA8 pixel data with width and height.
        /// </summary>
        Task StartFrameDeliveryAsync(Action<byte[], int, int> frameCallback);

        /// <summary>
        /// Stop delivering frames.
        /// </summary>
        Task StopFrameDeliveryAsync();

        /// <summary>
        /// Fully release camera resources.
        /// </summary>
        Task CleanupAsync();

        /// <summary>
        /// Switch camera (front/back). Re-initializes the underlying device.
        /// </summary>
        Task SetCameraAsync(BarkoderCameraPosition position);

        /// <summary>
        /// Set the zoom factor.
        /// </summary>
        void SetZoomFactor(float zoomFactor);

        /// <summary>
        /// Get the maximum supported zoom factor.
        /// </summary>
        float GetMaxZoomFactor();

        /// <summary>
        /// Check if flash/torch is available.
        /// </summary>
        bool IsFlashAvailable();

        /// <summary>
        /// Enable or disable flash/torch.
        /// </summary>
        void SetFlashEnabled(bool enabled);

        /// <summary>
        /// Capture the current frame as PNG bytes.
        /// </summary>
        Task<byte[]?> CaptureFrameAsync();

        /// <summary>
        /// Set which camera to use by device ID (from GetAvailableCameras).
        /// The selected camera will be used on the next InitializeAsync call.
        /// </summary>
        void SetCameraById(string deviceId);
    }
}
