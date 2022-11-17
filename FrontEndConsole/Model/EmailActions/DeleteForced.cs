namespace FrontEndConsole.Model.EmailActions;

public class DeleteForced : IEmailAction
{
    private readonly List<string> Codes = new() { "df", "delete-forced" };
    private const string Description = "Deletes emails even if they're marked as unread, important or starred.";

    public List<string> GetCodes()
        => Codes;

    public string GetDescription()
        => Description;

    public Task<EmailActionResult> Execute(EmailActionRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
