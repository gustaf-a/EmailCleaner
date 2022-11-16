using MailProviderService.Data;
using MailProviderService.EmailStore;
using MailProviderService.MessageConsumer;
using MailProviderService.MessageQueue;
using MailProviderService.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

        services.AddScoped<IEmailStore, EmailStoreV1>();
        services.AddScoped<IEmailRepository, EmailRepositoryV1>();
        services.AddScoped<IMessageConsumerFactory, MessageConsumerFactory>();
        services.AddScoped<IMessageQueue, RabbitMqMessageQueue>();

        services.AddSingleton<IChannelHandler, RabbitMqChannelHandler>();

        RegisterSqliteInMemoryTestContext<MailProviderServiceContext>(services);
        SetupDb(services);
    }

    private static void SetupDb(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;

            var db = scopedServices.GetRequiredService<MailProviderServiceContext>();
            db.Database.EnsureCreated();
        };
    }

    private static void RegisterSqliteInMemoryTestContext<T>(IServiceCollection services) where T : DbContext
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        services.AddDbContext<T>(
            options =>
            {
                options.UseSqlite(connection);
            }, ServiceLifetime.Singleton);
    }

    public void Configure(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();

        app.MapControllers();
    }
}
