using EmailCleaner.Client.Data;
using EmailCleaner.Client.Email.Sort.SortBehaviors;
using System.Text;

namespace EmailCleaner.Client.Email.Sort;

public class EmailSorterV1 : IEmailSorter
{
    private readonly List<ISortBehavior> _sortBehaviors;

    public EmailSorterV1()
    {
        _sortBehaviors = new()
        {
            new SortBySender(),
            new SortBySubject()
        };
    }

    public EmailsSortedResult SortByAllMethods(List<EmailData> emails)
    {
        var result = new EmailsSortedResult();
        var sb = new StringBuilder();

        foreach (var sortBehavior in _sortBehaviors)
            SortByBehavior(sortBehavior, emails, result, sb);

        result.Message = sb.ToString();
        return result;
    }

    private static void SortByBehavior(ISortBehavior sortBehavior, List<EmailData> emails, EmailsSortedResult result, StringBuilder sb)
    {
        var sortedEmails = sortBehavior.Sort(emails);

        if (sortedEmails is null)
        {
            sb.AppendLine($"Failed to sort emails using sort: {sortBehavior.GetName()}");
            return;
        }

        result.SortedEmails.Add(sortedEmails);

        sb.AppendLine($"Emails sorted by: {sortedEmails.SortMethodName} into {sortedEmails.EmailGroups.Count} groups.");
    }
}
