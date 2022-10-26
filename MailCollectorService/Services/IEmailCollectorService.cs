using MailCollectorService.Data;

namespace MailCollectorService.Services;

public interface IEmailCollectorService
{
    public Task<List<Email>> GetEmails(CancellationToken cancellationToken);
}
