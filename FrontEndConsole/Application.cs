using FrontEndConsole.Model.AppActions;
using FrontEndConsole.Model.AppActionsFactory;
using FrontEndConsole.Model.Configuration;
using FrontEndConsole.View;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FrontEndConsole
{
    internal class Application : IHostedService
    {
        private readonly ApplicationOptions _applicationOptions;

        private readonly IMainUserInterface _mainUi;

        private readonly IAppActionFactory _appActionFactory;

        private readonly LinkedList<IAppAction> _appActions;

        public Application(IConfiguration configuration, IMainUserInterface mainView, IAppActionFactory appActionFactory)
        {
            _applicationOptions = configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>();

            _mainUi = mainView;

            _appActionFactory = appActionFactory;

            _appActions = new();

            CreateAppActions(appActionFactory);
        }

        private void CreateAppActions(IAppActionFactory appActionFactory)
        {
            //Starts with existing files to avoid collecting emails already marked for removing.
            if (_applicationOptions.DoProcessExistingFiles)
                _appActions.AddLast(appActionFactory.Create(ApplicationAction.ProcessGroupedEmailFileAction));

            if (_applicationOptions.DoCollectMessages)
                _appActions.AddLast(appActionFactory.Create(ApplicationAction.CollectEmailsAction));

            if (!_appActions.Any())
                _appActions.AddLast(appActionFactory.Create(ApplicationAction.StartMenuAction));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _mainUi.Start();

            while (_appActions.Any())
            {
                try
                {
                    var currentAction = _appActions.First();
                    if (currentAction is null)
                        throw new Exception("Current AppAction was null.");
                    
                    _appActions.RemoveFirst();

                    var appActionResult = await currentAction.Execute(_mainUi);
                    appActionResult.EnsureSuccess();

                    var nextAction = appActionResult.NextAction ?? ApplicationAction.None;
                    if (nextAction == ApplicationAction.None)
                        continue;

                    //Actions being queued by the last action directly has higher priority than previously queued actions.
                    _appActions.AddFirst(_appActionFactory.Create(nextAction, appActionResult));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error encountered while executing action.");
                    throw;
                }
            }
            
            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
