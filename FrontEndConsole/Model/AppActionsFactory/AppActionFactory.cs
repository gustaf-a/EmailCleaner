using EmailCleaner.Client.Email.Sort;
using FrontEndConsole.Model.AppActions;
using FrontEndConsole.Model.Configuration;
using FrontEndConsole.Model.EmailActionProvider;
using FrontEndConsole.Model.OutputHandler;
using FrontEndConsole.Services;
using Microsoft.Extensions.Configuration;

namespace FrontEndConsole.Model.AppActionsFactory;

internal class AppActionFactory : IAppActionFactory
{
    private readonly ApplicationOptions _applicationOptions;

    private readonly IApiGatewayService _apiGatewayService;
    private readonly IEmailSorter _emailSorter;
    private readonly IOutputHandler _outputHandler;
    private readonly IEmailActionProvider _emailActionProvider;

    public AppActionFactory(IConfiguration configuration, IApiGatewayService apiGatewayService, IEmailSorter emailSorter, IOutputHandler outputHandler, IEmailActionProvider emailActionProvider)
    {
        _applicationOptions = configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>();
        _apiGatewayService = apiGatewayService;
        _emailSorter = emailSorter;
        _outputHandler = outputHandler;
        _emailActionProvider = emailActionProvider;
    }

    public IAppAction Create(ApplicationAction applicationAction)
        => Create(applicationAction, GetAppActions());

    public IAppAction Create(ApplicationAction applicationAction, AppActionResult appActionResult)
        => Create(applicationAction, GetAppActions(appActionResult.FilePaths));

    private AppActionOptions GetAppActions(List<string> filePaths = null)
        => new()
        {
            CollectTimeout = TimeSpan.FromSeconds(_applicationOptions.CollectTimeout),
            AfterCollectingStoppedDelay = TimeSpan.FromSeconds(_applicationOptions.AfterCollectingStoppedDelay),
            ExistingFilePaths = filePaths ?? _applicationOptions.ExistingFilePaths
        };

    public IAppAction Create(ApplicationAction applicationAction, AppActionOptions appActionOptions)
    {
        return applicationAction switch
        {
            ApplicationAction.CollectEmailsAction
                => new CollectEmailsAppAction(appActionOptions, _apiGatewayService, _emailSorter, _outputHandler),

            ApplicationAction.ProcessGroupedEmailFileAction
                => new ProcessGroupedEmailFileAppAction(appActionOptions, _outputHandler, _emailActionProvider),

            ApplicationAction.StartMenuAction
                => new StartMenuAppAction(appActionOptions),

            ApplicationAction.ExitApplicationAction
                => new ExitAppAction(),

            _ => throw new Exception($"Unrecognized Application Action: {applicationAction}"),
        };
    }
}
