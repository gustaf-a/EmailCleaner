using MailCollectorService;
using MailCollectorService.EventQueue;
using MailCollectorService.Repository;
using MailCollectorServiceTests.Mocks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MailCollectorServiceTests.Controller
{
    public class MailCollectorController_should
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private const string ControllerRoute = "v1/mailcollector";

        private readonly FakeEventQueue _fakeEventQueue;

        public MailCollectorController_should(WebApplicationFactory<Startup> factory)
        {
            _fakeEventQueue = new FakeEventQueue();

            var fakeGmailRepository = new FakeGmailRepository();

            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IEventQueue>(_fakeEventQueue);
                        services.AddSingleton<IGmailRepository>(fakeGmailRepository);
                    });
                }
                )
                .CreateClient();
        }

        [Fact]
        public async Task Collect_and_send_to_event_queue()
        {
            //Arrange

            //Act
            var responseStart = await _client.GetAsync($"{ControllerRoute}/start");
            responseStart.EnsureSuccessStatusCode();

            Thread.Sleep(300);

            var responseStop = await _client.GetAsync($"{ControllerRoute}/stop");

            //Assert
            responseStop.EnsureSuccessStatusCode();

            Assert.True(_fakeEventQueue.NumberOfTimesCalled > 0);
        }
    }
}
