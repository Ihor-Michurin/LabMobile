using Microsoft.Extensions.Logging;
using LabMobile.Data;
using LabMobile.Services;
using Microsoft.Extensions.Configuration;

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

        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        builder.Services.AddSingleton<IConfiguration>(configuration);


        builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<IAirQualityMeasurementService, AirQualityMeasurementService>();
        builder.Services.AddSingleton<IAnalysisService, AnalysisService>();
        builder.Services.AddSingleton<IAnalysisReceptionPointService, AnalysisReceptionPointService>();
        builder.Services.AddSingleton<ILaboratoryAssistantService, LaboratoryAssistantService>();
        builder.Services.AddSingleton<INurseService, NurseService>();
        builder.Services.AddSingleton<IPatientService, PatientService>();
        builder.Services.AddSingleton<IRegistrarService, RegistrarService>();
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<ITimeslotService, TimeslotService>();
        builder.Services.AddSingleton<IAnalysisResultService, AnalysisResultService>();

        builder.Services.AddSingleton<WeatherForecastService>();

        return builder.Build();
	}
}
