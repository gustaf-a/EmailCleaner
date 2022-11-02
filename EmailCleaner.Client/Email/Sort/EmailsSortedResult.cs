using EmailCleaner.Client.Data;

namespace EmailCleaner.Client.Email.Sort;

public class EmailsSortedResult
{
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;

    public List<SortedEmails> SortedEmails { get; set; } = new();
}
