using FrontEndNetMaui.Model;

namespace FrontEndNetMaui.Services
{
    internal class ApiGatewayV1Service : IApiGatewayService
    {
        private HttpClient _httpClient;

        private CancellationTokenSource _collectEmailsCancellationTokenSource;

        public ApiGatewayV1Service(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _collectEmailsCancellationTokenSource = new();
        }

        //public bool IsCollecting => _collectEmailsCancellationTokenSource.IsCancellationRequested;
        public bool IsCollecting { get; set; }

        public async Task<List<EmailData>> GetEmails()
        {
            var result = new List<EmailData>();

            var randomNumber = Random.Shared.Next(0, 200);
            var randomNumber2 = Random.Shared.Next(randomNumber, 250);

            for (int i = randomNumber; i < randomNumber2; i++)
            {
                result.Add(new EmailData
                {
                    Id = i,
                    SenderAddress = $"sender{i}@email.com",
                    ThreadId = i,
                    Tags = new() { "Tag1", "Tag2" },
                    Subject = $"Message from me to you {i}"
                });
            }

            return result;
        }

        public async Task StartCollectingEmails()
        {
            _collectEmailsCancellationTokenSource = new();
            IsCollecting = true;

            //Send call to microservice

            //start task to send back found emails to viewModel. Via Action<EmailData>?
            //No. First do the simplest possible implementation -> Get emails at the end.

            return;
        }

        public Task StopCollectingEmails()
        {
            _collectEmailsCancellationTokenSource.Cancel();

            IsCollecting = false;

            return Task.CompletedTask;
        }

        public Task DeleteEmails(List<EmailGroup> selectedGroupedEmails)
        {
            return Task.CompletedTask;
        }
    }
}
