using APIGateway;
using APIGateway.Data;
using ApiGatewayTests.Mocks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace ApiGatewayTests.Controller;

public class ApiGatewayController_should : 
    IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _apiGatewayclient;

    private Mock<HttpMessageHandler> _httpMessageHandlerMock;

    private const string ControllerRoute = "v1/emailcleaner";

    public ApiGatewayController_should(WebApplicationFactory<Startup> factory)
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var httpClientUsedByApiGateway = new HttpClient(_httpMessageHandlerMock.Object);
        var httpClientFactoryMock = new FakeHttpClientFactory(httpClientUsedByApiGateway);

        _apiGatewayclient = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IHttpClientFactory>((services) => httpClientFactoryMock);
                });
            })
            .CreateClient();
    }

    [Fact]
    public async Task GetPingResponse()
    {
        var response = await _apiGatewayclient.GetAsync($"{ControllerRoute}");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Send_request_to_mailCollectorService_to_start()
    {
        //Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            }));

        //Act
        var response = await _apiGatewayclient.GetAsync($"{ControllerRoute}/collect/start");
        response.EnsureSuccessStatusCode();

        //Assert
        Assert.Equal(2, _httpMessageHandlerMock.Invocations.Count);
    }

    [Fact]
    public async Task Send_request_to_mailCollectorService_to_stop()
    {
        //Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            }));

        //Act
        var response = await _apiGatewayclient.GetAsync($"{ControllerRoute}/collect/stop");
        response.EnsureSuccessStatusCode();

        //Assert
        Assert.Equal(2, _httpMessageHandlerMock.Invocations.Count);
    }

    [Fact]
    public async Task Get_emails_from_provider_service()
    {
        //Arrange
        var content = new StringContent(JsonConvert.SerializeObject(new List<Email>
        {
            new Email
            {
                Id = "5"
            }
        }));

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content
            }));

        //Act
        var response = await _apiGatewayclient.GetAsync($"{ControllerRoute}/collect");

        //Assert
        response.EnsureSuccessStatusCode();
        
        Assert.Equal(1, _httpMessageHandlerMock.Invocations.Count);

        var emails = await response.Content.ReadFromJsonAsync<List<Email>>();
        Assert.Single(emails);
        Assert.Equal("5", emails[0].Id);
    }
}
