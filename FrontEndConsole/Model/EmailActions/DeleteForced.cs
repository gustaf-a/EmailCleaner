using Serilog;

namespace FrontEndConsole.Model.EmailActions;

public class DeleteForced : IEmailAction
{
    private readonly List<string> Codes = new() { "df", "delete-forced" };
    private const string Description = "Deletes emails even if they're marked as unread, important or starred.";
    private const int Priority = 0;

    public List<string> GetCodes()
        => Codes;

    public string GetDescription()
        => Description;

    public int GetPriority()
        => Priority;

    public async Task<EmailActionResult> Execute(EmailActionRequest request)
    {
        //TODO implement and send to services
        Log.Information($"Delete forced: Command received.");

        return null;
    }

    
}
