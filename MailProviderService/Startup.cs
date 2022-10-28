using MailProviderService.Data;
using MailProviderService.EmailStore;
using MailProviderService.MessageConsumer;
using MailProviderService.MessageQueue;
using MailProviderService.Repository;

namespace MailProviderService;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(s => 
        {
            s.SwaggerDoc("v1", 
                new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MailProviderService", Version = "v1" });
        });

        services.AddHttpClient();

        services.AddSingleton<IChannelFactory, RabbitMqChannelFactory>();
        services.AddSingleton<IEmailStore, EmailStoreV1>();
        services.AddSingleton<IMessageConsumerFactory, MessageConsumerFactory>();
        services.AddSingleton<IMessageQueue, RabbitMqMessageQueue>();
        services.AddSingleton<IEmailRepository, EmailRepositoryV1>();
    }

    public void Configure(WebApplication app)
    {
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
