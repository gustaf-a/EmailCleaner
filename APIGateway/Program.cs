using APIGateway.Configuration;
using Serilog;

namespace APIGateway;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();

        startup.Configure(app);

        app.Run(GetUrl(builder.Configuration));
    }

    private static string GetUrl(IConfiguration config)
    {
        var serviceOptions = config.GetSection(ServiceOptions.Service).Get<ServiceOptions>();

        var url = serviceOptions.ApplicationUrl;
        if (string.IsNullOrWhiteSpace(url))
            throw new Exception("Unable to find value for ApplicationUrl in appsettings");

        Log.Information($"Starting on: {url}");

        return url;
    }
}