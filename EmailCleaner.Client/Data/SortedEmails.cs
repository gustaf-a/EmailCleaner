namespace EmailCleaner.Client.Data;

public class SortedEmails
{
    public List<EmailGroup> EmailGroups { get; set; }
    public string SortMethodName { get; set; }

    public SortedEmails(List<EmailGroup> emailGroups, string sortMethodName)
    {
        EmailGroups = emailGroups;
        SortMethodName = sortMethodName;
    }
}
