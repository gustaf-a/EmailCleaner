using APIGateway.Data;

namespace APIGateway.Services
{
    public class MailProviderServiceV1 : IMailProviderService
    {
        public Task<List<Email>> GetCollectedEmails()
        {
            throw new NotImplementedException();
        }
    }
}
