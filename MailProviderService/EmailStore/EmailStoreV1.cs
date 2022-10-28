using MailProviderService.Data;
using MailProviderService.Repository;

namespace MailProviderService.EmailStore
{
    public class EmailStoreV1 : IEmailStore
    {
        private IEmailRepository _repository;

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
            return await _repository.GetAll();
        }

        public async Task StoreEmails(List<Email> emails)
        {
            await _repository.Add(emails);
        }
    }
}
