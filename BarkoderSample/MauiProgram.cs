using Microsoft.Extensions.Logging;
using Plugin.Maui.Barkoder.Controls;

namespace BarkoderSample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				// Your custom fonts
			})
			.ConfigureMauiHandlers(handlers =>
			{
				// Your other maui handlers
				// *
				handlers.AddHandler(typeof(BarkoderView), typeof(BarkoderViewHandler));
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
	}
}

