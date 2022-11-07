using FrontEndConsole.Model.AppActions;

namespace FrontEndConsole.Model.Configuration;

internal class AppActionOptions
{
    public Dictionary<string, ApplicationAction> AskUserAlternatives { get; set; } = new();

    public TimeSpan CollectTimeout { get; set; }
    public TimeSpan AfterCollectingStoppedDelay { get; set; }

    public List<string> ExistingFilePaths { get; set; } = new();
}
