using MailCollectorService.CollectorHandler;
using MailCollectorService.EventQueue;
using MailCollectorService.Repository;
using MailCollectorService.Services;
using Microsoft.AspNetCore.Builder;

namespace MailCollectorService;

public class Startup
{
	public IConfiguration Configuration { get; }

	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services)
	{
        // Add services to the container.

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpClient();

        //TODO replace with factory using key from appsettings
        services.AddSingleton<IEmailCollectorService, GmailCollectorService>();
        services.AddSingleton<IEventQueue, RabbitMqEventQueue>();
        
        services.AddSingleton<IGmailRepository, GmailRepositoryV1>();
        services.AddSingleton<ICollectorHandler, CollectorHandler.CollectorHandler>();

    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
	{
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
