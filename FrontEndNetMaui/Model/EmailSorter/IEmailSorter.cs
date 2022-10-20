namespace FrontEndNetMaui.Model.EmailSorter
{
    public interface IEmailSorter
    {
        public void AddEmails(List<EmailData> emails);
        public List<EmailGroup> GetEmailGroups(GroupByMethods.GroupMethod selectedGroupMethod);
    }
}
