using EmailCleaner.Client.Data;
using FrontEndConsole.Model.Configuration;
using FrontEndConsole.Model.OutputHandler;
using FrontEndConsole.View;

namespace FrontEndConsole.Model.AppActions;

internal class ProcessGroupedEmailFileAppAction : IAppAction
{
    private readonly AppActionOptions _appActionOptions;
	private readonly IOutputHandler _outputHandler;

	public ProcessGroupedEmailFileAppAction(AppActionOptions appActionOptions, IOutputHandler outputHandler)
	{
		_appActionOptions = appActionOptions;
		_outputHandler = outputHandler;
	}

	public async Task<AppActionResult> Execute(IMainUserInterface mainUI)
	{
		if (!_appActionOptions.ExistingFilePaths.Any())
			throw new Exception("No filepaths for processing found.");

		var emailsForProcessing = new Dictionary<string, List<string>>();

		foreach (var filePath in _appActionOptions.ExistingFilePaths)
		{
			var markedEmailGroups = await _outputHandler.GetSavedFileMarkedForProcessing(filePath);

			//TODO Some testing smart!
			//TODO design before coding!

			//Go through all marked emailGroups and sort according to processing code.

			//Save in dictionary


			//Add Group by processing code 
		}

		//foreach file

		//load 
		//find markings and group


		throw new NotImplementedException();
	}
}
