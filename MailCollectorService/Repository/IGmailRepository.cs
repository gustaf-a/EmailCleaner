namespace MailCollectorService.Repository;

public interface IGmailRepository
{
    public Task<bool> EnsureWarm();

    public Task<List<Google.Apis.Gmail.v1.Data.Message>> GetEmails(CancellationToken cancellationToken);
    public Task<Google.Apis.Gmail.v1.Data.Message> GetEmailDetails(string messageId, CancellationToken cancellationToken);
}
