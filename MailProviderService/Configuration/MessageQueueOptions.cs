namespace MailProviderService.Configuration;

public class MessageQueueOptions
{
    public const string MessageQueue = "MessageQueue";

    public string Exchange { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
    public string RoutingKeyCollected { get; set; } = string.Empty;
}