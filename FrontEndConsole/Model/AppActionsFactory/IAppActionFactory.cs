using FrontEndConsole.Model.AppActions;
using FrontEndConsole.Model.Configuration;

namespace FrontEndConsole.Model.AppActionsFactory;

internal interface IAppActionFactory
{
    public IAppAction Create(ApplicationAction applicationActions);
    public IAppAction Create(ApplicationAction applicationAction, AppActionOptions appActionOptions);
    public IAppAction Create(ApplicationAction nextAction, AppActionResult appActionResult);
}
