namespace FrontEndConsole.Model.EmailActions;

public interface IEmailAction
{
    public List<string> GetCodes();
    public string GetDescription();

    public Task<EmailActionResult> Execute(EmailActionRequest request, CancellationToken cancellationToken);
}
