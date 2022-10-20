using static FrontEndNetMaui.Model.GroupByMethods;

namespace FrontEndNetMaui.Model.EmailSorter;

public class EmailSorterV1 : IEmailSorter
{
    private Dictionary<GroupMethod, List<EmailGroup>> _sortedEmailsGroupCache = new();

    private List<EmailData> _unsortedEmails = new();

    private bool _emailsHasBeenUpdated;

    public void AddEmails(List<EmailData> emails)
    {
        if (emails is null || emails.Count == 0)
            return;

        _unsortedEmails.AddRange(emails);

        _emailsHasBeenUpdated = true;
    }

    public List<EmailGroup> GetEmailGroups(GroupMethod groupMethod)
    {
        if (_unsortedEmails.Count == 0)
            return new();

        if (_emailsHasBeenUpdated)
        {
            _sortedEmailsGroupCache.Clear();

            _emailsHasBeenUpdated = false;
        }

        if (_sortedEmailsGroupCache.ContainsKey(groupMethod))
            return _sortedEmailsGroupCache[groupMethod];

        var sortedEmails = groupMethod switch
        {
            GroupMethod.Sender => SortBySender(_unsortedEmails),
            GroupMethod.Subject => SortBySubject(_unsortedEmails),
            GroupMethod.Tag => SortByTag(_unsortedEmails),
            GroupMethod.None => SortByNone(_unsortedEmails),
            _ => throw new NotImplementedException($"No email sorting found for method: {groupMethod}."),
        };

        _sortedEmailsGroupCache.Add(groupMethod, sortedEmails);

        return sortedEmails;
    }

    public static List<EmailGroup> SortBySender(List<EmailData> emails)
    {
        return CreateEmailGroups(emails.GroupBy(e => e.SenderAddress), GroupMethod.Sender);
    }

    public static List<EmailGroup> SortBySubject(List<EmailData> emails)
    {
        return CreateEmailGroups(emails.GroupBy(e => e.Subject), GroupMethod.Subject);
    }

    private static List<EmailGroup> CreateEmailGroups(IEnumerable<IGrouping<string, EmailData>> groups, GroupMethod groupMethod)
    {
        var groupedEmails = new List<EmailGroup>();

        foreach (var group in groups)
            groupedEmails.Add(new EmailGroup(groupMethod, group.Key)
            {
                Emails = group.ToList(),
                GroupedByValue = group.Key
            });

        return groupedEmails;
    }

    //TODO How handle tags? 
    //Idea: All user tags sorted A-z. So, if multiple tags, then the can be found. 
    //Group by if they have all tags or only one of them?
    public static List<EmailGroup> SortByTag(List<EmailData> emails)
    {
        var groupedEmails = new List<EmailGroup>();

        var tagSet = new HashSet<string>();

        return groupedEmails;
    }

    public static List<EmailGroup> SortByNone(List<EmailData> emails)
    {
        var groupedEmails = new List<EmailGroup>();

        foreach (var email in emails)
            groupedEmails.Add(new(GroupMethod.None)
            {
                Emails = new() { email }
            });

        return groupedEmails;
    }
}
