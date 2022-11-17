
namespace ApiGatewayTests.Mocks
{
    internal class FakeHttpClientFactory : IHttpClientFactory
    {
        private HttpClient _httpClient;

        public FakeHttpClientFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}
