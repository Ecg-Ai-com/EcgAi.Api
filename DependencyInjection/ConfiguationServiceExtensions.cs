// using EcgAi.Api.Configuration;
//
// namespace EcgAi.Api.DependencyInjection;
//
// public static class ConfigurationServiceExtensions
// {
//     public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
//     {
//         
//         services.AddOptions<TrainingDbConfiguration>()
//             .Bind(config.GetSection(TrainingDbConfiguration.SectionName))
//             .ValidateDataAnnotations();
//         
//         // builder.Services.Configure<MyConfigOptions>(builder.Configuration.GetSection(
//         //     MyConfigOptions.MyConfig));
//         //
//         // builder.Services.AddSingleton<IValidateOptions
//         //     <MyConfigOptions>, MyConfigValidation>();
//         
//         
//         services.AddOptions<HostConfiguration>()
//             .Bind(config.GetSection(HostConfiguration.SectionName))
//             .ValidateDataAnnotations();
//         // services.Configure<TrainingDbConfiguration>(
//         //     config.GetSection(TrainingDbConfiguration.SectionName));    
//         // services.Configure<HostConfiguration>(
//         //     config.GetSection(HostConfiguration.SectionName));
//         // services.Configure<ColorOptions>(
//         //     config.GetSection(ColorOptions.Color));
//
//         return services;
//     }
// }