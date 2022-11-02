namespace EmailCleaner.Client.Data;

public class EmailGroup
{
    public List<EmailData> Emails { get; set; }
    public string GroupedByValue { get; set; }

    public int EmailsCount => Emails.Count;

    public EmailGroup(List<EmailData> emails, string groupedByValue)
    {
        Emails = emails;
        GroupedByValue = groupedByValue;
    }
}
