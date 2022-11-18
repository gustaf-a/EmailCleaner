using EmailCleaner.Client.Data;

namespace FrontEndConsole.Model.EmailActions;

public class EmailActionRequest
{
    public List<MarkedEmailGroup> MarkedEmails { get; internal set; }
}
