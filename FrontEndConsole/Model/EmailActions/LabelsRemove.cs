using Serilog;

namespace FrontEndConsole.Model.EmailActions;

public class LabelsRemove : IEmailAction
{
    private readonly List<string> Codes = new() { "lr", "labels-remove" };
    private const string Description = "Removes label(s) from the selected emails.";
    private const int Priority = 70;

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