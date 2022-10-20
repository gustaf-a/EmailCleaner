using MailProviderService.Data;

namespace MailProviderService.Services;

public class EmailStore
{
	//TODO Connect to docker SQLite database
	private List<Email> _emails;
	public EmailStore()
	{
		_emails = new();

		_emails.Add(new Email
		{

		});
	}

	public List<Email> GetEmails()
	{
		return _emails;
	}
}
