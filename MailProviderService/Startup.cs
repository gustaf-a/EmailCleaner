using MailProviderService.Data;
using MailProviderService.EmailStore;
using MailProviderService.MessageConsumer;
using MailProviderService.MessageQueue;
using MailProviderService.Repository;
using Microsoft.EntityFrameworkCore;

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

        services.AddScoped<IChannelFactory, RabbitMqChannelFactory>();
        services.AddScoped<IEmailStore, EmailStoreV1>();
        services.AddScoped<IEmailRepository, EmailRepositoryV1>();
        services.AddScoped<IMessageConsumerFactory, MessageConsumerFactory>();
        services.AddScoped<IMessageQueue, RabbitMqMessageQueue>();

        services.AddDbContext<MailProviderServiceContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("EmailDb")));
    }

    public void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MailProviderServiceContext>();
                //Ensures start with an empty database.
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
