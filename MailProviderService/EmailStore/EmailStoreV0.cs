using MailProviderService.Data;

namespace MailProviderService.EmailStore;

public class EmailStoreV0 : IEmailStore
{
    public Task<List<Email>> GetEmails()
    {
        throw new NotImplementedException();
    }

    public void StoreEmails(List<Email> emails)
    {
        throw new NotImplementedException();
    }
}
