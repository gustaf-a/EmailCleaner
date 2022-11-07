using FrontEndConsole.Model.Configuration;
using FrontEndConsole.View;

namespace FrontEndConsole.Model.AppActions;

internal class StartMenuAppAction : IAppAction
{
    private AppActionOptions _appActionOptions;

    private readonly Dictionary<string, ApplicationAction> _menuAlternatives;

    public StartMenuAppAction(AppActionOptions appActionOptions)
    {
        _appActionOptions = appActionOptions;

        _menuAlternatives = new()
        {
            { "Collect emails", ApplicationAction.CollectEmailsAction },
            //{ "Process existing file", ApplicationAction.ProcessGroupedEmailFileAction },
            { "Exit", ApplicationAction.ExitApplicationAction }
        };
    }

    public Task<AppActionResult> Execute(IMainUserInterface mainUI)
    {
        mainUI.Clear();

        mainUI.SetStatus("Start Menu");

        var pickedKey = mainUI.ShowMenu(prompt: "Please chose an alternative", _menuAlternatives.Keys.ToList());

        if (!_menuAlternatives.ContainsKey(pickedKey))
            throw new Exception($"Failed to parse result from UI menu: {pickedKey}");

        var pickedAppAction = _menuAlternatives[pickedKey];

        if(ApplicationAction.ProcessGroupedEmailFileAction.Equals(pickedAppAction))
        {
            //TODO get filepaths and send in
            throw new NotImplementedException("Please use appsettings.json to directly specify filepaths.");
        }

        return Task.FromResult(new AppActionResult(pickedAppAction));
    }
}
