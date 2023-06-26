using Microsoft.Extensions.Logging;
using LabMobile.Data;
using LabMobile.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

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

        /*var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();*/
        var configuration = builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
    {
        { "AppSettings:AuthUrl", "https://auth20230614132728.azurewebsites.net/" },
        { "AppSettings:ChatUrl", "https://chatgptwebapi20230617153717.azurewebsites.net/" },
        { "AppSettings:MainApiUrl", "https://labwebapi20230601225432.azurewebsites.net/" }
    })
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
        builder.Services.AddSingleton<IChatGptService, ChatGptService>();
        builder.Services.AddSingleton<IEmailService, EmailService>();

        builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddLocalization();

        return builder.Build();
    }
}
