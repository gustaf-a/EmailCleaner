using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MailProviderService;
using MailProviderService.Data;
using MailProviderService.Repository;
using MailProviderServiceTests.Mocks.MessageQueue;
using MailProviderServiceTests.Mocks.Repository;
using Newtonsoft.Json;
using System.Text;
using MailProviderService.MessageQueue;

namespace MailProviderServiceTests.Controller;

public class MailProviderController_should
    : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
{
    private readonly HttpClient _client;

    private const string ControllerRoute = "v1/mailprovider";

    private readonly FakeModel _fakeModel;

    private readonly FakeEmailRepository _repository;

    private SqliteConnection _connection;

    public MailProviderController_should(WebApplicationFactory<Startup> factory)
    {
        _fakeModel = new FakeModel();
        RabbitMqChannelHandlerMock.SetFakeModel(_fakeModel);

        //Context gets disposed when running tests, so using a FakeRepository as good enough solution.
        _repository = new FakeEmailRepository();

        _client = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    RemoveRegisteredDbContext<MailProviderServiceContext>(services);

                    RegisterSqliteInMemoryTestContext<MailProviderServiceContext>(services);

                    SetupDb(services);

                    services.AddSingleton<IChannelHandler, RabbitMqChannelHandlerMock>();

                    services.AddSingleton<IEmailRepository>(_repository);
                });
            }
            )
            .CreateClient();
    }

    private static void SetupDb(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;

            var db = scopedServices.GetRequiredService<MailProviderServiceContext>();
            db.Database.EnsureCreated();

            ////Seed db here if necessary:
            //db.Email.AddRange(testdata);
            //db.SaveChanges();
        };
    }

    /// <summary>
    /// Remove registration of production database through AddDbContext and replace with testing.
    /// Necessary after ASP.NET Core 3.0
    /// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
    /// </summary>
    private static void RemoveRegisteredDbContext<T>(IServiceCollection services) where T : DbContext
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<T>));

        if (descriptor is null)
            return;

        services.Remove(descriptor);
    }

    /// <summary>
    /// Creates an SQLite in-memory database
    /// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
    /// https://learn.microsoft.com/en-us/ef/core/testing/testing-without-the-database#sqlite-in-memory
    /// </summary>
    private void RegisterSqliteInMemoryTestContext<T>(IServiceCollection services) where T : DbContext
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        services.AddDbContext<T>(
            options =>
            {
                options.UseSqlite(_connection);
            }, ServiceLifetime.Singleton);
    }

    [Fact]
    public async Task GetPingResponse()
    {
        var pingResponse = await _client.GetAsync($"{ControllerRoute}");
        pingResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Call_message_consumer_when_startCollecting_and_stopCollecting_called()
    {
        //Arrange
        var fixture = new Fixture();
        var expectedEmails = fixture.CreateMany<Email>(20).ToList();

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(expectedEmails));

        //Act
        var pingResponseStart = await _client.GetAsync($"{ControllerRoute}/collect/start");
        pingResponseStart.EnsureSuccessStatusCode();

        _fakeModel.FakeMessage(1234, "email", "email.collect", body);

        var pingResponseStop = await _client.GetAsync($"{ControllerRoute}/collect/stop");
        pingResponseStop.EnsureSuccessStatusCode();

        var emailsResponse = await _client.GetAsync($"{ControllerRoute}/emails");
        emailsResponse.EnsureSuccessStatusCode();

        //Assert
        var resultEmails = JsonConvert.DeserializeObject<List<Email>>(await emailsResponse.Content.ReadAsStringAsync());

        Assert.NotNull(resultEmails);

        Assert.Equal(expectedEmails.Count, resultEmails.Count);
    }

    /// <summary>
    /// Closes db test database. 
    /// </summary>
    public void Dispose()
    {
        _connection.Close();
    }
}
