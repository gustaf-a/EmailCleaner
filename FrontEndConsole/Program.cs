using Microsoft.Extensions.DependencyInjection;
using Serilog;
using FrontEndConsole.View;
using Microsoft.Extensions.Hosting;
using FrontEndConsole.Model.OutputHandler;
using FrontEndConsole.Model.AppActionsFactory;
using FrontEndConsole.Services;
using EmailCleaner.Client.Email.Sort;

namespace FrontEndConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();

        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(
            services =>
            {
                services.AddHostedService<Application>();
                ConfigureServices(services);
            }
            );

        var host = builder.Build();

        try
        {
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected exception.");
            throw;
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<Application>();

        services.AddScoped<IAppActionFactory, AppActionFactory>();
        services.AddScoped<IApiGatewayService, ApiGatewayServiceV1>();
        services.AddScoped<IOutputHandler, FileSystemOutputhandlerV1>();
        services.AddScoped<HttpClient>();

        services.AddScoped<IEmailSorter, EmailSorterV1>();

        services.AddSingleton<IMainUserInterface, ConsoleMainUserInterfaceV1>();
    }
}