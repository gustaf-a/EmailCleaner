namespace FrontEndConsole.Model.EmailActions;

public interface IEmailAction
{
    public List<string> GetCodes();
    public string GetDescription();

    /// <summary>
    /// Priority is the order in which actions will be executed.
    /// The lower numbers will be executed later.
    /// Removal of emails is 0.
    /// Nothing above 9 may be destructive.
    /// Negative numbers may be used for actions done after removal of emails.
    /// </summary>
    public int GetPriority();

    public Task<EmailActionResult> Execute(EmailActionRequest request);
}
