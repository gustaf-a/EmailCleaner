using MailCollectorService.Data;

namespace MailCollectorService.Services;

public interface IEmailCollectorService
{
    public Task<List<Email>> GetEmails(CancellationToken cancellationToken);
    public Task<List<Email>> GetEmailDetails(List<Email> emails, CancellationToken cancellationToken);
    public Task<Email> GetEmailDetails(Email email, CancellationToken cancellationToken);
    void EnsureWarm();
}
