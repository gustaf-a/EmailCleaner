using EmailCleaner.Client.Data;

namespace EmailCleaner.Client.Email.Sort.SortBehaviors;

public interface ISortBehavior
{
    public SortedEmails Sort(List<EmailData> emails);
    public string GetName();
}
