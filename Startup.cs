using System.Reflection;
using EcgAi.Api.Configuration;
using EcgAi.Api.DependencyInjection;
using EcgAi.Api.FastEndPoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;

namespace EcgAi.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var trainingDbConfiguration = new TrainingDbConfiguration();
        _configuration.GetSection(TrainingDbConfiguration.SectionName).Bind(trainingDbConfiguration);
        
        var ecgAiDrawingAzureApiConfiguration = new EcgAiDrawingAzureApiConfiguration();
        _configuration.GetSection(EcgAiDrawingAzureApiConfiguration.SectionName)
            .Bind(ecgAiDrawingAzureApiConfiguration);

        services
            .AddFastEndpoints()
            .AddSwaggerDoc()
            .AddSwaggerDoc(settings =>
            {
                settings.Title = "EcgAi Api";
                settings.Version = "v1";
            })
            .AddMappings()
            .AddTrainingDatabase(trainingDbConfiguration)
            .AddExternalApis(ecgAiDrawingAzureApiConfiguration)
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("MyResponseHeader");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        var hostConfiguration = new HostConfiguration();
        _configuration.GetSection(HostConfiguration.SectionName).Bind(hostConfiguration);
        Console.WriteLine($"host: {hostConfiguration}");
        app.Urls.Add(hostConfiguration.ToString());
        app.UseAuthorization();
        app.UseFastEndpoints(c => { c.Endpoints.Configurator = ep => { ep.PostProcessors(new ErrorLogger()); }; });
        app.UseDefaultExceptionHandler();
        app.UseOpenApi();
        app.UseSwaggerUi3(s => s.ConfigureDefaults());
        app.UseHttpLogging();
        app.UseSerilogRequestLogging();
        // app.UseAuthentication();
        // app.UseMvc();
        // Console.WriteLine("Database {0}",userSecret);
        app.Run();
    }
}