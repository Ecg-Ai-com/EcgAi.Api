using System.Reflection;
using EcgAi.Api;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder();

    builder.Configuration.AddEnvironmentVariables()
        // .AddKeyVault()
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
    
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog();

    Log.Information("Application Starting up");

    var startup = new Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services); // calling ConfigureServices method
    var app = builder.Build();
    startup.Configure(app, builder.Environment);
    
}
catch (Exception e)
{
    Log.Fatal("The application failed to start {Exception}",e.ToString());
}
finally
{
    Log.CloseAndFlush();
}