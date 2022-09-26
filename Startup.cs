using System.Reflection;
using EcgAi.Api.Configuration;
using EcgAi.Api.DependencyInjection;
using FastEndpoints.Swagger;

namespace EcgAi.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;

    }
    
    public void ConfigureServices(IServiceCollection services) {
        
        var trainingDbConfiguration = new TrainingDbConfiguration();
        _configuration.GetSection(TrainingDbConfiguration.SectionName).Bind(trainingDbConfiguration);
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
            .AddMediatR(Assembly.GetExecutingAssembly());
    }
    public void Configure(WebApplication app, IWebHostEnvironment env) {

        var hostConfiguration = new HostConfiguration();
        _configuration.GetSection(HostConfiguration.SectionName).Bind(hostConfiguration);
        Console.WriteLine($"host: {hostConfiguration}");
        app.Urls.Add(hostConfiguration.ToString());
        app.UseAuthorization();
        app.UseFastEndpoints();
        app.UseOpenApi(); 
        app.UseSwaggerUi3(s => s.ConfigureDefaults()); 

        // Console.WriteLine("Database {0}",userSecret);
        app.Run();
    }
}