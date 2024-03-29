﻿using APIGateway.Configuration;
using Serilog;

namespace APIGateway.Services
{
    public class MailCollectorServiceV1 : IMailCollectorService
    {
        private const string BaseV1Route = "v1/mailcollector/";

        private const string CollectStartRoute = "start";
        private const string CollectStopRoute = "stop";

        private readonly ServicesOptions _servicesOptions; 

        private readonly HttpClient _httpClient;

        public MailCollectorServiceV1(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _servicesOptions = configuration.GetSection(ServicesOptions.Services).Get<ServicesOptions>();

            _httpClient = httpClientFactory.CreateClient();

            _httpClient.BaseAddress = new Uri(_servicesOptions.MailCollectorServiceUri + BaseV1Route);
        }

        public async Task StartCollecting()
        {
            await _httpClient.GetAsync(CollectStartRoute);
            Log.Information($"Requested start from {CollectStartRoute}");
        }

        public async Task StopCollecting()
        {
            await _httpClient.GetAsync(CollectStopRoute);
            Log.Information($"Requested stop from {CollectStartRoute}");
        }
    }
}
