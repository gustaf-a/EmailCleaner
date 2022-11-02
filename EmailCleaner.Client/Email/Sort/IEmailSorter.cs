using EmailCleaner.Client.Data;

namespace EmailCleaner.Client.Email.Sort;

public interface IEmailSorter
{
    public EmailsSortedResult SortByAllMethods(List<EmailData> emails);
}
