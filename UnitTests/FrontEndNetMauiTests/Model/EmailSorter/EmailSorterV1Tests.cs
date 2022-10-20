using FrontEndNetMaui.Model;
using FrontEndNetMaui.Model.EmailSorter;

namespace FrontEndNetMauiTests.Model.EmailSorter;

public class EmailSorterV1Tests
{
    private readonly List<EmailData> _emails = new()
    {
        //Same sender
        new() {Id = 1, SenderAddress = "sender_1@email.com", Subject="Test message 1", Tags = new(){ "Tag_1" } },
        new() {Id = 2, SenderAddress = "sender_1@email.com", Subject="Test message 2", Tags = new(){ "Tag_2" } },
        new() {Id = 3, SenderAddress = "sender_1@email.com", Subject="Test message 3", Tags = new(){ "Tag_3" } },

        //Same Subject
        new() {Id = 4, SenderAddress = "sender_2@email.com", Subject="Test message 4", Tags = new(){ "Tag_4" } },
        new() {Id = 5, SenderAddress = "sender_3@email.com", Subject="Test message 4", Tags = new(){ "Tag_5" } },
        new() {Id = 6, SenderAddress = "sender_4@email.com", Subject="Test message 4", Tags = new(){ "Tag_6" } },

        //Same Tags
        new() {Id = 7, SenderAddress = "sender_5@email.com", Subject="Test message 5", Tags = new(){ "Tag_7" } },
        new() {Id = 8, SenderAddress = "sender_6@email.com", Subject="Test message 6", Tags = new(){ "Tag_7" } },
        new() {Id = 9, SenderAddress = "sender_7@email.com", Subject="Test message 7", Tags = new(){ "Tag_7" } },
    };

    [Fact]
    public void AddEmails_adds_emails_which_can_be_retrieved_grouped()
    {
        //Arrange
        var emailSorter = new EmailSorterV1();
        emailSorter.AddEmails(_emails);

        //Act
        var emailsBySubject = emailSorter.GetEmailGroups(GroupByMethods.GroupMethod.Subject);
        var emailsBySender = emailSorter.GetEmailGroups(GroupByMethods.GroupMethod.Sender);
        var emailsByTag = emailSorter.GetEmailGroups(GroupByMethods.GroupMethod.Tag);
        var emailsByNone = emailSorter.GetEmailGroups(GroupByMethods.GroupMethod.None);

        //Assert
        Assert.NotEmpty(emailsBySubject);
        Assert.NotEmpty(emailsBySender);
        //Assert.NotEmpty(emailsByTag);
        Assert.NotEmpty(emailsByNone);
    }

    [Fact]
    public void SortByNone_sorts_emails_into_separate_groups_for_each_email()
    {
        //Arrange

        //Act
        var groupedEmails = EmailSorterV1.SortByNone(_emails);

        //Assert
        Assert.Equal(_emails.Count, groupedEmails.Count);

        foreach (var groupedEmail in groupedEmails)
        {
            Assert.Single(groupedEmail.Emails);
            Assert.Equal(GroupByMethods.GroupMethod.None, groupedEmail.GroupedBy);
        }
    }

    [Fact]
    public void SortBySubject_sorts_emails_into_subject_groups()
    {
        //Arrange

        //Act
        var groupedEmails = EmailSorterV1.SortBySubject(_emails);

        //Assert
        Assert.Equal(7, groupedEmails.Count);

        foreach (var groupedEmail in groupedEmails)
        {
            Assert.Equal(GroupByMethods.GroupMethod.Subject, groupedEmail.GroupedBy);

            var distinctSubjects = groupedEmail.Emails
                                    .Select(e => e.Subject).Distinct();

            Assert.Single(distinctSubjects);
        }
    }

    [Fact]
    public void SortBySender_sorts_emails_into_sender_groups()
    {
        //Arrange

        //Act
        var groupedEmails = EmailSorterV1.SortBySender(_emails);

        //Assert
        Assert.Equal(7, groupedEmails.Count);

        foreach (var groupedEmail in groupedEmails)
        {
            Assert.Equal(GroupByMethods.GroupMethod.Sender, groupedEmail.GroupedBy);

            var distinctSubjects = groupedEmail.Emails
                                    .Select(e => e.SenderAddress).Distinct();

            Assert.Single(distinctSubjects);
        }
    }

    //TODO Add ByTag
}
