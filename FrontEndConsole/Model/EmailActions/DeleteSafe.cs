
using Serilog;

namespace FrontEndConsole.Model.EmailActions;

public class DeleteSafe : IEmailAction
{
    private readonly List<string> Codes = new() { "d", "ds", "delete-safe" };
    private const string Description = "Deletes emails, but not if they're unread, important or starred.";
    private const int Priority = 5;

    public List<string> GetCodes()
        => Codes;

    public string GetDescription()
        => Description;

    public int GetPriority()
        => Priority;

    public async Task<EmailActionResult> Execute(EmailActionRequest request)
    {
        //TODO implement and send to services
        Log.Information($"Delete safe: Command received.");

        return null;
    }
}
