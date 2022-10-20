using MailProviderService.Data;

namespace MailProviderService.Services;

public interface IEmailStore
{
    public List<Email> GetEmails();
}
