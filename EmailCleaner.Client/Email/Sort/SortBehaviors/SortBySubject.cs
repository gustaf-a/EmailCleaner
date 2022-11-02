using EmailCleaner.Client.Data;

namespace EmailCleaner.Client.Email.Sort.SortBehaviors;

public class SortBySubject : ISortBehavior
{
    public SortedEmails Sort(List<EmailData> emails)
    {
        var emailGroups = CreateEmailGroups(emails.GroupBy(e => e.Subject));

        return new SortedEmails(emailGroups, GetName());
    }

    private static List<EmailGroup> CreateEmailGroups(IEnumerable<IGrouping<string, EmailData>> groups)
    {
        return groups.Select(group =>
                    new EmailGroup(
                        group.ToList(),
                        group.Key)
                    ).ToList();
    }

    public string GetName()
        => nameof(SortBySubject);
}
