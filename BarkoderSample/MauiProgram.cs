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
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.ConfigureMauiHandlers(handlers =>
			{
				handlers.AddHandler(typeof(BarkoderView), typeof(BarkoderViewHandler));
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
	}
}

