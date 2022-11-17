using APIGateway.Configuration;
using APIGateway.Data;
using Serilog;

namespace APIGateway.Services
{
    public class MailProviderServiceV1 : IMailProviderService
    {
        private const string BaseV1Route = "v1/mailprovider/";

        private const string CollectStartRoute = "collect/start";
        private const string CollectStopRoute = "collect/stop";

        private const string GetEmailsRoute= "emails";

        private readonly ServicesOptions _servicesOptions;

        private readonly HttpClient _httpClient;

        public MailProviderServiceV1(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _servicesOptions = configuration.GetSection(ServicesOptions.Services).Get<ServicesOptions>();

            _httpClient = httpClientFactory.CreateClient();

            _httpClient.BaseAddress = new Uri(_servicesOptions.MailProviderServiceUri + BaseV1Route);
        }

        public async Task<List<Email>> GetCollectedEmails()
        {
            Log.Information($"Sending requst for emails from {GetEmailsRoute}.");

            var response = await _httpClient.GetAsync(GetEmailsRoute);
            response.EnsureSuccessStatusCode();

            if (response.Content is null)
            {
                Log.Warning("Response content was null. Unable to convert into emails.");
                return null;
            }

            var emails = await response.Content.ReadFromJsonAsync<List<Email>>();

            if (emails is null)
                Log.Error($"Failed to convert to list of emails from response content: {response.Content}");

            Log.Information($"Collected {emails.Count} emails from provider.");

            return emails;
        }

        public async Task StartCollecting()
        {
            await _httpClient.GetAsync(CollectStartRoute);
            Log.Information($"Requested start from {CollectStartRoute}");
        }

        public async Task StopCollecting()
        {
            await _httpClient.GetAsync(CollectStopRoute);
            Log.Information($"Requested stop from {CollectStopRoute}");
        }
    }
}
