namespace MailProviderService.Data;

public class Email
{
    public string origin;
    public string id;
    public string threadId;
    public IList<string> labelIds;
    public string snippet;
    public object payload;
}
