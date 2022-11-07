using EmailCleaner.Client.Data;

namespace FrontEndConsole.Model.OutputHandler;

internal interface IOutputHandler
{
    public Task<List<MarkedEmailGroup>> GetSavedFileMarkedForProcessing(string filePath);
    public Task<List<string>> SaveSortedEmails(List<SortedEmails> sortedEmails);
    public void Show(string path);
    public void Show(List<string> paths);
}
