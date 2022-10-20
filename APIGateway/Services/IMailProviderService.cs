using APIGateway.Data;

namespace APIGateway.Services
{
    public interface IMailProviderService
    {
        Task<List<Email>> GetCollectedEmails();
    }
}
