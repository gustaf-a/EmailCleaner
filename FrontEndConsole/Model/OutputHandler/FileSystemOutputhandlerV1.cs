using EmailCleaner.Client.Data;
using FrontEndConsole.Model.Configuration;
using FrontEndConsole.Model.OutputHandler.TextConverter;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Diagnostics;

namespace FrontEndConsole.Model.OutputHandler;

internal class FileSystemOutputhandlerV1 : IOutputHandler
{
    private readonly ITextConverter _textConverter;

    private const string FileEnding = ".txt";

    private readonly ApplicationOptions _applicationOptions;

    private readonly string _outputBaseDirectory;

    public FileSystemOutputhandlerV1(IConfiguration configuration, ITextConverter textConverter)
    {
        _textConverter = textConverter;

        _applicationOptions = configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>();

        _outputBaseDirectory = _applicationOptions.OutputDirectory;

        if (!Path.IsPathFullyQualified(_outputBaseDirectory))
        {
            if (string.IsNullOrWhiteSpace(_outputBaseDirectory))
                _outputBaseDirectory = ApplicationOptions.DefaultOutputDirectory;

            _outputBaseDirectory = Path.Combine(Directory.GetCurrentDirectory(), _outputBaseDirectory);
        }

        if (!Directory.Exists(_outputBaseDirectory))
        {
            Log.Information($"Output directory not found. Creating directory {_outputBaseDirectory}");

            Directory.CreateDirectory(_outputBaseDirectory);
        }
    }

    public async Task<List<string>> SaveSortedEmails(List<SortedEmails> sortedEmailsList)
    {
        var savedFilePaths = new List<string>();

        var outputDirectory = Path.Combine(_outputBaseDirectory, GetSessionDirectory());
        Directory.CreateDirectory(outputDirectory);

        foreach (var sortedEmails in sortedEmailsList)
        {
            var fileName = Path.Combine(outputDirectory, $"{sortedEmails.SortMethodName}{FileEnding}");

            if (!await WriteToFile(sortedEmails.EmailGroups, fileName))
            {
                Log.Error($"Failed to output emailgroup. Skipping emails sorted by: {sortedEmails.SortMethodName}.");
                continue;
            }

            savedFilePaths.Add(fileName);
        }

        if (_applicationOptions.ShowOutputDirectoryAfterActions)
            Show(outputDirectory);

        return savedFilePaths;
    }

    private static string GetSessionDirectory()
        => DateTime.Now.ToString("yyyyMMddTHHmmss");

    private async Task<bool> WriteToFile(List<EmailGroup> emailGroups, string fileName)
    {
        Log.Information($"Starting to write to file {fileName}");

        try
        {
            using var streamWriter = new StreamWriter(Path.Combine(_outputBaseDirectory, fileName));

            foreach (var emailGroup in emailGroups)
                await streamWriter.WriteLineAsync(_textConverter.Serialize(emailGroup));
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to write an emailGroup to file {fileName}.");
            return false;
        }

        return true;
    }

    public void Show(string path)
    {
        if (!string.Empty.Equals(Path.GetExtension(path)))
            path = Path.GetDirectoryName(path);

        if (path is null)
        {
            Log.Error($"Failed to find directory path from: {path}");
            return;
        }

        ShowDirectory(path);
    }

    public void Show(List<string> paths)
    {
        var pathsToOpen = new List<string>();

        foreach (var path in paths)
        {
            var directoryPath = path;

            if (!string.Empty.Equals(Path.GetExtension(path)))
                directoryPath = Path.GetDirectoryName(path);

            if (!pathsToOpen.Contains(directoryPath))
                pathsToOpen.Add(directoryPath);
        }

        foreach (var pathToOpen in pathsToOpen)
            ShowDirectory(pathToOpen);
    }

    private static void ShowDirectory(string directoryPath)
    {
        Process.Start("explorer.exe", directoryPath);
    }

    public Task<List<MarkedEmailGroup>> GetSavedFileMarkedForProcessing(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Unable to find file: {filePath}");

        var markedEmailGroups = new List<MarkedEmailGroup>();

        using var streamReader = new StreamReader(filePath);

        string? line;
        while ((line = streamReader.ReadLine()) != null)
        {
            var markedEmailGroup = _textConverter.Deserialize(line);
            if (markedEmailGroup is null)
            {
                Log.Error($"Failed to deserialize {line}");
                continue;
            }

            markedEmailGroups.Add(markedEmailGroup);
        }

        return Task.FromResult(markedEmailGroups);
    }
}
