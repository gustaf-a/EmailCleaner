using MailProviderService.Data;
using MailProviderService.Repository;

namespace MailProviderServiceTests.Mocks.Repository
{
    internal class FakeEmailRepository : IEmailRepository
    {
        public List<Email> Emails = new();

        public Task Add(List<Email> emails)
        {
            Emails.AddRange(emails);

            return Task.FromResult(true);
        }

        public Task Delete(List<Email> emails)
        {
            foreach (var email in emails)
                if(Emails.Contains(email))
                    Emails.Remove(email);

            return Task.FromResult(true);
        }

        public Task<List<Email>> GetAll()
        {
            return Task.FromResult(Emails);
        }
    }
}
