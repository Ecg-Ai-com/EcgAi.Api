using System.Reflection;
using MapsterMapper;

namespace EcgAi.Api.DependencyInjection;

public static class MappingsServiceExtensions
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}