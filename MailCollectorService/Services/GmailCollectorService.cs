using MailCollectorService.Data;
using MailCollectorService.Repository;
using Serilog;

namespace MailCollectorService.Services;

public class GmailCollectorService : IEmailCollectorService
{
    private readonly IGmailRepository _gmailRepository;

    private readonly int maxAttempts = 5;
    private int currentAttempts;

    public GmailCollectorService(IGmailRepository gmailRepository)
    {
        _gmailRepository = gmailRepository;
    }

    public async Task<List<Email>> GetEmailDetails(List<Email> emails, CancellationToken cancellationToken)
    {
        var result = new List<Email>();

        foreach (var email in emails)
            result.Add(await GetEmailDetails(email, cancellationToken));

        return result;
    }

    public async Task<Email> GetEmailDetails(Email email, CancellationToken cancellationToken)
    {
        currentAttempts = 0;

        do
        {
            try
            {
                var detailedMessage = await _gmailRepository.GetEmailDetails(email.Id, cancellationToken);

                return detailedMessage.ToEmail();
            }
            catch (Exception ex)
            {
                //TODO Check if is gmail exception

                if (ShouldRetryOn())
                {
                    Log.Warning(ex, $"Exception caught. Retrying");
                    continue;
                }

                Log.Error(ex, $"Exception encountered and no more retries left.");
                throw;
            }
        } while (!cancellationToken.IsCancellationRequested);

        return new();
    }

    public async Task<List<Email>> GetEmails(CancellationToken cancellationToken)
    {
        currentAttempts = 0;

        do
        {
            try
            {
                var undetailedMessages = await _gmailRepository.GetEmails(cancellationToken);

                Log.Information($"Got list of {undetailedMessages.Count} messages.");

                return undetailedMessages.ToEmailList();
            }
            catch (Exception ex)
            {
                //TODO Check if is gmail exception

                if (ShouldRetryOn())
                {
                    Log.Warning(ex, $"Exception caught. Retrying");
                    continue;
                }

                Log.Error(ex, $"Exception encountered and no more retries left.");
                throw;
            }
        } while (!cancellationToken.IsCancellationRequested);

        return new();
    }

    private bool ShouldRetryOn()
    {
        if (currentAttempts > maxAttempts)
            return false;

        currentAttempts++;

        return true;
    }
}
