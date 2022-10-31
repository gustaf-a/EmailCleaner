namespace MailProviderService.Data;

public class Email
{
    public string Origin { get; set; }
    public string Id { get; set; }
    public string ThreadId { get; set; }
    public string LabelIds { get; set; }
    public string Snippet { get; set; }
    public string Payload { get; set; }
}
