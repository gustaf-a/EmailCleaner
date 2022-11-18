using Serilog;

namespace FrontEndConsole.Model.EmailActions;

public class LabelsAdd : IEmailAction
{
    private readonly List<string> Codes = new() { "la", "labels-add" };
    private const string Description = "Add label(s) to the selected emails.";
    private const int Priority = 75;

    public List<string> GetCodes()
        => Codes;

    public string GetDescription()
        => Description;

    public int GetPriority()
        => Priority;

    public async Task<EmailActionResult> Execute(EmailActionRequest request)
    {
        //TODO implement and send to services
        Log.Information($"{Description}: Command received.");

        return null;
    }
}
