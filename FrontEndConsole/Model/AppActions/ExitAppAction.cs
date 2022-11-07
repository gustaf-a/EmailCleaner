using FrontEndConsole.View;

namespace FrontEndConsole.Model.AppActions
{
    internal class ExitAppAction : IAppAction
    {
        public Task<AppActionResult> Execute(IMainUserInterface mainUI)
        {
            return Task.FromResult(new AppActionResult(ApplicationAction.None));
        }
    }
}
