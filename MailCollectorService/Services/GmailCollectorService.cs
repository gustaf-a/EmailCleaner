using MailCollectorService.Data;
using MailCollectorService.Repository;

namespace MailCollectorService.Services;

public class GmailCollectorService : IEmailCollectorService
{
    private IGmailRepository _gmailRepository;

    public GmailCollectorService(IGmailRepository gmailRepository)
    {
        _gmailRepository = gmailRepository;
    }

    public async Task<List<Email>> GetEmails()
    {
        try
        {
            var undetailedMessages = await _gmailRepository.GetEmails();

            var detailedMessages = await _gmailRepository.GetEmailDetails(undetailedMessages);

            return detailedMessages.ToEmailList();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
