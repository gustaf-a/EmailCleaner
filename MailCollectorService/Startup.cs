using MailCollectorService.CollectorHandler;
using MailCollectorService.MessageQueue;
using MailCollectorService.Repository;
using MailCollectorService.Services;

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
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MailCollectorService", Version = "v1" });
        });

        services.AddHttpClient();

        services.AddSingleton<IEmailCollectorService, GmailCollectorService>();
        services.AddSingleton<IMessageQueue, RabbitMqMessageQueueV1>();

        services.AddSingleton<IGmailRepository, GmailRepositoryV1>();
        //services.AddSingleton<ICollectorHandler, CollectorHandler.CollectorHandler>();
        services.AddSingleton<ICollectorHandler, CollectorHandler.CollectorHandlerRx>();

    }

    public void Configure(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();

        app.MapControllers();
    }
}
