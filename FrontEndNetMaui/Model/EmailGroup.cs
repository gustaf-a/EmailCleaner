using static FrontEndNetMaui.Model.GroupByMethods;

namespace FrontEndNetMaui.Model;

public class EmailGroup
{
    public GroupMethod GroupedBy { get; set; }
    public string GroupedByValue { get; set; }
    
    public List<EmailData> Emails { get; set; }
    public int EmailsCount => Emails.Count;


    public EmailGroup(GroupMethod groupedBy, string groupedByValue = "")
    {
        GroupedBy = groupedBy;
        GroupedByValue = groupedByValue;

        Emails = new();
    }
}
