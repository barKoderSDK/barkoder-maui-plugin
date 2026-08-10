using Barkoder.Windows.UI;

namespace Barkoder.Windows
{
    /// <summary>
    /// Extension methods for integrating the Barkoder SDK into a MAUI app.
    /// </summary>
    public static class BarkoderMauiExtensions
    {
        /// <summary>
        /// Registers the Barkoder SDK MAUI handlers (CameraPreview, etc.).
        /// Call this in your MauiProgram.CreateMauiApp() builder.
        /// <example>
        /// <code>
        /// builder.UseBarkoder();
        /// </code>
        /// </example>
        /// </summary>
        public static MauiAppBuilder UseBarkoder(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<CameraPreview, CameraPreviewHandler>();
            });

            return builder;
        }
    }
}
