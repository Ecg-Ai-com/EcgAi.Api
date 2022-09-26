// using System.Reflection;
// using EcgAi.Api.Data.Database.Cosmos;
// using Mapster;
// using MapsterMapper;
// using Microsoft.EntityFrameworkCore;
//
// namespace EcgAi.Api;
//
// public static class DependencyInjection
// {
//     public static IServiceCollection AddMappings(this IServiceCollection services)
//     {
//         var config = TypeAdapterConfig.GlobalSettings;
//         config.Scan(Assembly.GetExecutingAssembly());
//
//         services.AddSingleton(config);
//         services.AddScoped<IMapper, ServiceMapper>();
//
//         return services;
//     }
//     public static IServiceCollection AddDatabases(this IServiceCollection services,IConfiguration configuration)
//     {
//         // services.AddControllers();
//         services.AddDbContext<TrainingDbContext>(
//             options => options.UseCosmos(
//                 configuration["EndpointUri"],
//                 configuration["PrimaryKey"], 
//                 configuration["TrainingDatabase"]));
//                 //.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
//
//         return services;
//     }
//     // ApplicationDbContext
//     // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     //     => optionsBuilder.UseCosmos(
//     //         "https://localhost:8081",
//     //         "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
//     //         databaseName: "OrdersDB");
// }