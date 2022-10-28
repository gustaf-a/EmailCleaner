using MailProviderService.EmailStore;
using MailProviderService.MessageConsumer;
using MailProviderService.MessageQueue;

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
        // Add services to the container.

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(s => 
        {
            s.SwaggerDoc("v1", 
                new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MailProviderService", Version = "v1" });
        });

        services.AddHttpClient();

        services.AddSingleton<IChannelBuilder, RabbitMqChannelBuilder>();
        services.AddSingleton<IEmailStore, EmailStoreV0>();
        services.AddSingleton<IMessageConsumerFactory, MessageConsumerFactory>();
        services.AddSingleton<IMessageQueue, RabbitMqMessageQueue>();
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
