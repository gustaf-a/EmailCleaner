using Serilog;

namespace MailCollectorService;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, shared: true)
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