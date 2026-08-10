using Barkoder.Windows;
using Microsoft.Extensions.Logging;

namespace BKDSampleMAUI;

public static class MauiProgram
{
	private static readonly string LogPath = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "bkd_crash.log");

	public static MauiApp CreateMauiApp()
	{
		// Global crash handlers
		AppDomain.CurrentDomain.UnhandledException += (s, e) =>
		{
			var ex = e.ExceptionObject as Exception;
			Log($"UNHANDLED: {ex?.GetType().Name}: {ex?.Message}\n{ex?.StackTrace}");
		};
		TaskScheduler.UnobservedTaskException += (s, e) =>
		{
			Log($"UNOBSERVED TASK: {e.Exception?.GetType().Name}: {e.Exception?.Message}\n{e.Exception?.StackTrace}");
			e.SetObserved();
		};

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseBarkoder()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	public static void Log(string message)
	{
		var line = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
		try { File.AppendAllText(LogPath, line + Environment.NewLine); } catch { }
		System.Diagnostics.Debug.WriteLine(line);
	}
}
