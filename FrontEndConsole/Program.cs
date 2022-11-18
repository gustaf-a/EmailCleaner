using Microsoft.Extensions.DependencyInjection;
using Serilog;
using FrontEndConsole.View;
using Microsoft.Extensions.Hosting;
using FrontEndConsole.Model.OutputHandler;
using FrontEndConsole.Model.AppActionsFactory;
using FrontEndConsole.Services;
using EmailCleaner.Client.Email.Sort;
using FrontEndConsole.Model.OutputHandler.TextConverter;
using FrontEndConsole.Model.EmailActionProvider;

namespace FrontEndConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/frontendconsole.txt", rollingInterval: RollingInterval.Day)
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

        services.AddScoped<ITextConverter, CsvTextConverter>();

        services.AddScoped<IEmailSorter, EmailSorterV1>();

        services.AddSingleton<IEmailActionProvider, EmailActionProvider>();
        services.AddSingleton<IMainUserInterface, ConsoleMainUserInterfaceV1>();
    }
}