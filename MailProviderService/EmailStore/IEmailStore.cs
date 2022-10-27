using MailProviderService.Data;

namespace MailProviderService.EmailStore;

public interface IEmailStore
{
    public Task<List<Email>> GetEmails();
    public void StoreEmails(List<Email> emails);
}
