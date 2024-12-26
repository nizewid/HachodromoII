using Hachodromo.Maui.Services;
using Hachodromo.Shared;
using Hachodromo.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hachodromo.Maui
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			InteractiveRenderSettings.ConfigureBlazorHybridRenderModes();

			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddMauiBlazorWebView();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif
			builder.Services.AddSingleton<IFormFactor, FormFactor>();

			return builder.Build();
		}
	}
}
