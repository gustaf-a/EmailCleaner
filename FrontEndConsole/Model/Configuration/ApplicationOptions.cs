namespace FrontEndConsole.Model.Configuration;

internal class ApplicationOptions
{
    public const string Application = "Application";

    public string ApiGatewayHostName { get; set; } = "localhost";

    public bool DoCollectMessages { get; set; } = false;
    public int CollectTimeout { get; set; } = 60;
    public int AfterCollectingStoppedDelay { get; set; } = 1;

    public bool DoProcessExistingFiles { get; set; } = false;
    public List<string> ExistingFilePaths { get; set; } = new();

    public const string DefaultOutputDirectory = "output";
    public string OutputDirectory { get; set; } = DefaultOutputDirectory;
    public bool ShowOutputDirectoryAfterActions { get; set; } = true;
}
