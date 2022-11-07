using FrontEndConsole.View;

namespace FrontEndConsole.Model.AppActions;

internal interface IAppAction
{
    public Task<AppActionResult> Execute(IMainUserInterface mainUI);
}
