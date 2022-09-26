using EcgAi.Api.Configuration;
using EcgAi.Api.Data.Database.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace EcgAi.Api.DependencyInjection;

public static class DatabaseServiceExtension
{
    public static IServiceCollection AddTrainingDatabase(this IServiceCollection services,TrainingDbConfiguration configuration)
    {
        // var host = services.GetRequiredService<IOptionsMonitor<HostConfiguration>>().CurrentValue;

        // services.AddControllers();
        services.AddDbContext<TrainingDbContext>(
            options => options.UseCosmos(
                configuration.EndpointUri,
                configuration.PrimaryKey,
                configuration.DatabaseName));
        //.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

        return services;
    }
}