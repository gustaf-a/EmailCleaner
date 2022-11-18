﻿using EmailCleaner.Client.Data;
using FrontEndConsole.Model.Configuration;
using FrontEndConsole.Model.EmailActionProvider;
using FrontEndConsole.Model.EmailActions;
using FrontEndConsole.Model.OutputHandler;
using FrontEndConsole.View;
using Serilog;

namespace FrontEndConsole.Model.AppActions;

internal class ProcessGroupedEmailFileAppAction : IAppAction
{
    private readonly AppActionOptions _appActionOptions;
    private readonly IOutputHandler _outputHandler;
    private readonly IEmailActionProvider _emailActionProvider;

    public ProcessGroupedEmailFileAppAction(AppActionOptions appActionOptions, IOutputHandler outputHandler, IEmailActionProvider emailActionProvider)
    {
        _appActionOptions = appActionOptions;
        _outputHandler = outputHandler;
        _emailActionProvider = emailActionProvider;
    }

    public async Task<AppActionResult> Execute(IMainUserInterface mainUI)
    {
        if (!_appActionOptions.ExistingFilePaths.Any())
            throw new Exception("No filepaths for processing found.");

        var markedEmailGroupWithEmailAction = await GetMarkedEmailGroupsSortedByEmailAction(_appActionOptions.ExistingFilePaths);

        //TODO Ask for confirmation

        //TODO unknown actions handling

        Log.Information("Starting actions.");

        foreach (var emailActionMarkedEmailsPair in markedEmailGroupWithEmailAction)
        {
            var emailAction = emailActionMarkedEmailsPair.Key;
            var emails = emailActionMarkedEmailsPair.Value;

            Log.Information($"Executing following action on {emails.Count} emails: {emailAction.GetDescription}.");

            var request = new EmailActionRequest
            {
                MarkedEmails = emails
            };

            var result = await emailAction.Execute(request);

            Log.Information($"Action following action on {emails.Count} emails: {emailAction.GetDescription}.");
        }

        return new AppActionResult(ApplicationAction.None);
    }

    private async Task<SortedDictionary<IEmailAction, List<MarkedEmailGroup>>> GetMarkedEmailGroupsSortedByEmailAction(List<string> existingFilePaths)
    {
        var allEmailsSortedByActionCode = new Dictionary<string, List<MarkedEmailGroup>>();

        foreach (var filePath in _appActionOptions.ExistingFilePaths)
        {
            Log.Information($"Collecting marked email groups from: {filePath}.");

            var allEmailGroupsFromFile = await _outputHandler.GetSavedFileMarkedForProcessing(filePath);

            var markedEmailGroups = GetMarkedEmailGroups(allEmailGroupsFromFile);

            foreach (var markedEmailGroupKeyValuePair in markedEmailGroups)
            {
                var key = markedEmailGroupKeyValuePair.Key;

                if (string.IsNullOrWhiteSpace(key))
                    continue;

                //TODO Avoid duplicates?
                if (!allEmailsSortedByActionCode.ContainsKey(key))
                    allEmailsSortedByActionCode.Add(key, new List<MarkedEmailGroup>());

                allEmailsSortedByActionCode[key].AddRange(markedEmailGroupKeyValuePair.Value);
            }
        }

        Log.Information($"All marked email groups collected.");

        //TODO RemoveDuplicates(allEmailsSortedByActionCode);

        var emailsSortedByEmailAction = GetSortedEmailActions(allEmailsSortedByActionCode);

        return emailsSortedByEmailAction;
    }

    private static Dictionary<string, List<MarkedEmailGroup>> GetMarkedEmailGroups(List<MarkedEmailGroup> allEmailGroupsFromFile)
        => allEmailGroupsFromFile
            .GroupBy(e => e.ProcessingCode)
            .ToDictionary(g => g.Key, g => g.ToList());

    private SortedDictionary<IEmailAction, List<MarkedEmailGroup>> GetSortedEmailActions(Dictionary<string, List<MarkedEmailGroup>> allEmailsSortedByActionCode)
    {
        var groupedByEmailActions = new Dictionary<IEmailAction, List<MarkedEmailGroup>>();

        foreach (var actionCodeGroup in allEmailsSortedByActionCode)
            groupedByEmailActions.Add(_emailActionProvider.GetEmailAction(actionCodeGroup.Key), actionCodeGroup.Value);

        return Sort(groupedByEmailActions);

    }

    private static SortedDictionary<IEmailAction, List<MarkedEmailGroup>> Sort(Dictionary<IEmailAction, List<MarkedEmailGroup>> markedEmailGroups)
    {
        var emailActionComparer = new EmailActionComparer();

        return new SortedDictionary<IEmailAction, List<MarkedEmailGroup>>(markedEmailGroups, emailActionComparer);
    }
}
