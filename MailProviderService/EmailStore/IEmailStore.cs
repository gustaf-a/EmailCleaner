using MailProviderService.Data;

namespace MailProviderService.EmailStore;

public interface IEmailStore
{
    public Task<List<Email>> GetEmails();
    public Task StoreEmails(List<Email> emails);
    public Task DeleteEmails(List<Email> emails);
}
