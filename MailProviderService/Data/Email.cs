namespace MailProviderService.Data;

public class Email
{
    public string Origin { get; set; }
    public string Id { get; set; }
    public string ThreadId { get; set; }
    public IList<string> LabelIds { get; set; }
    public string Snippet { get; set; }
    public object Payload { get; set; }
}
