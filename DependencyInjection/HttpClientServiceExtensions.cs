using EcgAi.Api.Configuration;
using EcgAi.Api.Features.Images.Queries.GetEcgImageByRecordId;

namespace EcgAi.Api.DependencyInjection;

public static class HttpClientServiceExtensions
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services,
        EcgAiDrawingAzureApiConfiguration configuration)
    {
        string address = configuration.BaseAddress+configuration.FunctionName+configuration.FunctionKey;
        services.AddHttpClient<AzureEcgDrawingFunction>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(address);
        });
        return services;
    }
}