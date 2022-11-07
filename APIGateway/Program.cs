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

        var url = $"http://{Environment.GetEnvironmentVariable("SERVICE_NAME")}:{Environment.GetEnvironmentVariable("PORT")}";

        Log.Information($"Starting on: {url}");

        app.Run(url);
    }
}