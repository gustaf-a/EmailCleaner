using FrontEndConsole.Model.EmailActions;

namespace FrontEndConsole.Model.EmailActionProvider;

public interface IEmailActionProvider
{
    public List<string> GetEmailActionInstructions();

    public IEmailAction GetEmailAction(string code);
}
