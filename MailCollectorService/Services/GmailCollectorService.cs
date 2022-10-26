using MailCollectorService.Data;
using MailCollectorService.Repository;

namespace MailCollectorService.Services;

public class GmailCollectorService : IEmailCollectorService
{
    private readonly IGmailRepository _gmailRepository;

    public GmailCollectorService(IGmailRepository gmailRepository)
    {
        _gmailRepository = gmailRepository;
    }

    public async Task<List<Email>> GetEmails(CancellationToken cancellationToken)
    {
        currentAttempts = 0;

        do
        {
            try
            {
                var undetailedMessages = await _gmailRepository.GetEmails(cancellationToken);

                var detailedMessages = await _gmailRepository.GetEmailDetails(undetailedMessages, cancellationToken);

                return detailedMessages.ToEmailList();
            }
            catch (Exception ex)
            {
                //TODO Check if is gmail exception

                if (ShouldRetryOn(ex))
                    continue;
                
                throw;
            }
        } while (!cancellationToken.IsCancellationRequested);

        return new();
    }

    private readonly int maxAttempts = 5;
    private int currentAttempts = 0;

    private bool ShouldRetryOn(Exception ex)
    {
        if(currentAttempts > maxAttempts)
            return false;

        currentAttempts++;
        //Pause before trying again

        return true;
    }
}
