using MailProviderService;
using MailProviderService.MessageQueue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using RabbitMQ.Client;
using MailProviderServiceTests.Mocks.MessageQueue;

namespace MailProviderServiceTests.Controller
{
    public class MailProviderController_should
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private const string ControllerRoute = "v1/mailprovider";

        private readonly Mock<IModel> _modelMock;

        public MailProviderController_should(WebApplicationFactory<Startup> factory)
        {
            _modelMock = new Mock<IModel>();

            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IChannelFactory>(new RabbitMqChannelFactoryMock(_modelMock));
                    });
                }
                )
                .CreateClient();
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

            //Act
            var pingResponseStart = await _client.GetAsync($"{ControllerRoute}/collect/start");
            pingResponseStart.EnsureSuccessStatusCode();

            var pingResponseStop = await _client.GetAsync($"{ControllerRoute}/collect/stop");
            pingResponseStop.EnsureSuccessStatusCode();

            //Assert
            var invocations = _modelMock.Invocations;

            Assert.Equal(3, invocations.Count); // QueueDeclare, BasicConsume, BasicCancel
        }

    }
}
