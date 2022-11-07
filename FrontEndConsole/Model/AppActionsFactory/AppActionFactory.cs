using EmailCleaner.Client.Email.Sort;
using FrontEndConsole.Model.AppActions;
using FrontEndConsole.Model.Configuration;
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

    public AppActionFactory(IConfiguration configuration, IApiGatewayService apiGatewayService, IEmailSorter emailSorter, IOutputHandler outputHandler)
    {
        _applicationOptions = configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>();
        _apiGatewayService = apiGatewayService;
        _emailSorter = emailSorter;
        _outputHandler = outputHandler;
    }

    public IAppAction Create(ApplicationAction applicationAction)
    {
        var appActionOptions = new AppActionOptions
        {
            CollectTimeout = TimeSpan.FromSeconds(_applicationOptions.CollectTimeout),
            AfterCollectingStoppedDelay = TimeSpan.FromSeconds(_applicationOptions.AfterCollectingStoppedDelay),
            ExistingFilePaths = _applicationOptions.ExistingFilePaths
        };

        return Create(applicationAction, appActionOptions);
    }

    public IAppAction Create(ApplicationAction applicationAction, AppActionResult appActionResult)
    {
        var appActionOptions = new AppActionOptions
        {
            CollectTimeout = TimeSpan.FromSeconds(_applicationOptions.CollectTimeout),
            AfterCollectingStoppedDelay = TimeSpan.FromSeconds(_applicationOptions.AfterCollectingStoppedDelay),

            ExistingFilePaths = appActionResult.FilePaths
        };

        return Create(applicationAction, appActionOptions);
    }

    public IAppAction Create(ApplicationAction applicationAction, AppActionOptions appActionOptions)
    {
        return applicationAction switch
        {
            ApplicationAction.CollectEmailsAction
                => new CollectEmailsAppAction(appActionOptions, _apiGatewayService, _emailSorter, _outputHandler),

            ApplicationAction.ProcessGroupedEmailFileAction
                => new ProcessGroupedEmailFileAppAction(appActionOptions),

            ApplicationAction.StartMenuAction
                => new StartMenuAppAction(appActionOptions), 

            ApplicationAction.ExitApplicationAction
                => new ExitAppAction(),

            _ => throw new Exception($"Unrecognized Application Action: {applicationAction}"),
        };
    }


}
