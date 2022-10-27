using APIGateway;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ApiGatewayTests.Controller;

public class ApiGatewayController_should : 
    IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    private const string ControllerRoute = "v1/emailcleaner";

    public ApiGatewayController_should(WebApplicationFactory<Startup> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    
                });
            })
            .CreateClient();
    }

    [Fact]
    public async Task GetPingResponse()
    {
        var pingResponse = await _client.GetAsync($"{ControllerRoute}");
        pingResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Send_request_to_mailCollectorService_to_start()
    {
        var pingResponse = await _client.GetAsync($"{ControllerRoute}/collect/start");
        pingResponse.EnsureSuccessStatusCode();

        //TODO Ensure sends to service
    }

    [Fact]
    public async Task Send_request_to_mailCollectorService_to_stop()
    {
        var pingResponse = await _client.GetAsync($"{ControllerRoute}/collect/stop");
        pingResponse.EnsureSuccessStatusCode();

        //TODO Ensure sends to service
    }

    [Fact]
    public async Task Get_emails_from_provider_service()
    {
        var pingResponse = await _client.GetAsync($"{ControllerRoute}/collect");
        pingResponse.EnsureSuccessStatusCode();

        //TODO Ensure sends to service
    }
}
