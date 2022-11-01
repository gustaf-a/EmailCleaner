namespace MailCollectorService.Configuration;

public class GmailOptions
{
    public const string Gmail = "Gmail";

    public string ApplicationName { get; set; } = string.Empty;
    public string CredentialsFileName { get; set; } = string.Empty;
    public string UserId { get; set; } = "me";
}
