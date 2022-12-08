namespace MailCollectorService.Configuration;

public class GmailOptions
{
    public const string Gmail = "Gmail";

    public string ApplicationName { get; set; } = string.Empty;
    public string CredentialsFileName { get; set; } = string.Empty;
    public string UserId { get; set; } = "me";
    
    public int UserRateLimitPeriodSeconds { get; set; } = 60;
    public int UserRateLimit { get; set; } = 100;
}
