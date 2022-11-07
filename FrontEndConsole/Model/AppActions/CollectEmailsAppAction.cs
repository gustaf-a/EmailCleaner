using EmailCleaner.Client.Data;
using EmailCleaner.Client.Email.Sort;
using FrontEndConsole.Model.Configuration;
using FrontEndConsole.Model.OutputHandler;
using FrontEndConsole.Services;
using FrontEndConsole.View;
using Serilog;

namespace FrontEndConsole.Model.AppActions;

internal class CollectEmailsAppAction : IAppAction
{
	private readonly AppActionOptions _appActionOptions;

	private readonly IApiGatewayService _apiGatewayService;

	private readonly IEmailSorter _emailSorter;
	private readonly IOutputHandler _outputHandler;

	private const string AfterCollectingMenuPrompt = "Emails grouped and found. Please go through and mark them for actions before proceeding.";
	private readonly List<string> AfterCollectingMenuAlternatives = new(){ "Start processing", "Exit" };
	private const int ProcessingAlternativeIndex = 0;

	public CollectEmailsAppAction(AppActionOptions appActionOptions, IApiGatewayService apiGatewayService, IEmailSorter emailSorter, IOutputHandler outputHandler)
	{
        _appActionOptions = appActionOptions;
		_apiGatewayService = apiGatewayService;
		_emailSorter = emailSorter;
        _outputHandler = outputHandler;
	}

	public async Task<AppActionResult> Execute(IMainUserInterface mainUI)
	{
		mainUI.SetStatus("Collecting emails");

		await DoCollectEmails();

		var emails = await FetchEmails();
		if (!emails.Any())
			return new AppActionResult(new Exception("Failed to get emails from services."));

        var emailsSortedResult = _emailSorter.SortByAllMethods(emails);
		if(!emailsSortedResult.IsSuccess)
            return new AppActionResult(new Exception($"Failed to sort emails: {emailsSortedResult.Message}"));

		var filePaths = await _outputHandler.SaveSortedEmails(emailsSortedResult.SortedEmails);
		
		mainUI.Display(emailsSortedResult.Message);

        var reply = mainUI.ShowMenu(AfterCollectingMenuPrompt, AfterCollectingMenuAlternatives);
		
		if (AfterCollectingMenuAlternatives[ProcessingAlternativeIndex].Equals(reply))
			return new AppActionResult(ApplicationAction.ProcessGroupedEmailFileAction, filePaths);

		return new AppActionResult(ApplicationAction.ExitApplicationAction);
    }

	private async Task<List<EmailData>> FetchEmails()
	{
        Log.Information("Fetching emails.");

        return await _apiGatewayService.GetEmails();
    }

    private async Task DoCollectEmails()
	{
        Log.Information("Starting to collect emails.");
        await _apiGatewayService.StartCollecting();

        Log.Information($"Waiting for collecting to finish: {_appActionOptions.CollectTimeout.TotalSeconds} seconds left.");
        Task.Delay(_appActionOptions.CollectTimeout).Wait();

        Log.Information("Stopping emails collecting.");
        await _apiGatewayService.StopCollecting();

        Task.Delay(_appActionOptions.AfterCollectingStoppedDelay).Wait();
    }
}
