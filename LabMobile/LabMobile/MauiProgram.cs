using Microsoft.Extensions.Logging;
using LabMobile.Data;
using LabMobile.Services;

namespace LabMobile;

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
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<IAirQualityMeasurementService, AirQualityMeasurementService>();
        builder.Services.AddSingleton<IAnalysisService, AnalysisService>();
        builder.Services.AddSingleton<IAnalysisReceptionPointService, AnalysisReceptionPointService>();
        builder.Services.AddSingleton<ILaboratoryAssistantService, LaboratoryAssistantService>();

        builder.Services.AddSingleton<WeatherForecastService>();

		return builder.Build();
	}
}
