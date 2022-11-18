using Serilog;

namespace FrontEndConsole.Model.EmailActions;

public class SaveAttachments : IEmailAction
{
    private readonly List<string> Codes = new() { "sa", "save-attachments" };
    private const string Description = "Save any attachments found.";
    private const int Priority = 50;

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
