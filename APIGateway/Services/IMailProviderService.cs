using APIGateway.Data;

namespace APIGateway.Services
{
    public interface IMailProviderService
    {
        public Task<List<Email>> GetCollectedEmails();
        public Task StartCollect();
        public Task StopCollect();
    }
}
