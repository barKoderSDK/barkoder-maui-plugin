namespace Barkoder.Windows.Models
{
    /// <summary>
    /// Delegate invoked when scanning produces results.
    /// Matches the signature from Android/iOS SDKs.
    /// </summary>
    /// <param name="results">Decoded barcode results.</param>
    /// <param name="thumbnails">Cropped barcode thumbnail images as ImageSource, or null.</param>
    /// <param name="resultImage">Full camera frame at time of decode as ImageSource, or null.</param>
    public delegate void BarkoderResultCallback(BarcodeResult[] results, ImageSource[]? thumbnails, ImageSource? resultImage);

    /// <summary>
    /// Delegate for performance metrics reporting.
    /// </summary>
    /// <param name="fps">Frames per second from the camera.</param>
    /// <param name="dps">Decodes per second (decode attempts).</param>
    public delegate void BarkoderPerformanceCallback(double fps, double dps);

    /// <summary>
    /// Delegate for receiving raw preview frames.
    /// </summary>
    /// <param name="frameData">Raw frame bytes.</param>
    /// <param name="width">Frame width in pixels.</param>
    /// <param name="height">Frame height in pixels.</param>
    public delegate void BarkoderPreviewFramesCallback(byte[] frameData, int width, int height);

    /// <summary>
    /// Delegate for flash availability check.
    /// </summary>
    public delegate void FlashAvailableCallback(bool isAvailable);

    /// <summary>
    /// Delegate for max zoom factor query.
    /// </summary>
    public delegate void MaxZoomAvailableCallback(float maxZoomFactor);

    /// <summary>
    /// Delegate for camera lifecycle events.
    /// </summary>
    public delegate void CameraCallback(CameraEvent cameraEvent);

    /// <summary>
    /// Camera lifecycle event types.
    /// </summary>
    public enum CameraEvent
    {
        Opened,
        Closed,
        Error
    }

    /// <summary>
    /// Describes an available camera device.
    /// </summary>
    public class CameraDeviceInfo
    {
        /// <summary>
        /// Unique device/source group identifier. Pass this to SetCameraById to select a specific camera.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable display name of the camera.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        public override string ToString() => DisplayName;
    }
}
