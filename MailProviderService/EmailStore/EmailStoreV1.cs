using MailProviderService.Data;
using MailProviderService.Repository;
using Serilog;

namespace MailProviderService.EmailStore
{
    public class EmailStoreV1 : IEmailStore
    {
        private readonly IEmailRepository _repository;

        public EmailStoreV1(IEmailRepository repository)
        {
            _repository = repository;
        }

        public async Task DeleteEmails(List<Email> emails)
        {
            await _repository.Delete(emails);
        }

        public async Task<List<Email>> GetEmails()
        {
            var emails = await _repository.GetAll();

            Log.Information($"Providing {emails.Count} emails.");

            return emails;
        }

        public async Task StoreEmails(List<Email> emails)
        {
            await _repository.Add(emails);
        }
    }
}
