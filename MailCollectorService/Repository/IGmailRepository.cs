namespace MailCollectorService.Repository;

public interface IGmailRepository
{
    public Task<List<Google.Apis.Gmail.v1.Data.Message>> GetEmails(CancellationToken cancellationToken);
    public Task<List<Google.Apis.Gmail.v1.Data.Message>> GetEmailDetails(List<string> messageIds, CancellationToken cancellationToken);
}
