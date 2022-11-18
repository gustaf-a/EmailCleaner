using Serilog;

namespace FrontEndConsole.Model.EmailActions;

internal class NotMarkedEmailAction : IEmailAction
{
    private readonly List<string> _codes = new() { "", "na" ,"no-action" };
    private const string Description = "No action.";
    private const int Priority = int.MinValue;

    public Task<EmailActionResult> Execute(EmailActionRequest request)
    {
        Log.Information($"No action selected for: {request.MarkedEmails.Count}");

        return Task.FromResult(new EmailActionResult());
    }

    public List<string> GetCodes()
        => _codes;

    public string GetDescription()
        => Description;

    public int GetPriority()
        => Priority;
    
}
