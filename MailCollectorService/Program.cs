using Serilog;

namespace MailCollectorService;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs/mailcollector.txt", rollingInterval: RollingInterval.Day, shared: true)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();

        startup.Configure(app);

        app.Run();
    }
}