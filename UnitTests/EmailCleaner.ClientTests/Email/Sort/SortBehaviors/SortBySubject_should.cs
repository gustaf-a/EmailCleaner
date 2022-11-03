using EmailCleaner.Client.Data;
using EmailCleaner.Client.Email.Sort.SortBehaviors;

namespace EmailCleaner.ClientTests.Email.Sort.SortBehaviors;

public class SortBySubject_should
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
    public void Return_correct_name()
    {
        var sortBehavior = new SortBySubject();

        Assert.Equal("SortBySubject", sortBehavior.GetName());
    }

    [Fact]
    public void Sort_into_sortedEmails()
    {
        //Arrange
        var sortBehavior = new SortBySubject();

        //Act
        var sortedEmails = sortBehavior.Sort(_emails);

        //Assert
        Assert.Equal(7, sortedEmails.EmailGroups.Count);

        foreach (var emailGroup in sortedEmails.EmailGroups)
        {
            var distinctSubjects = emailGroup.Emails
                                    .Select(e => e.Subject).Distinct();

            Assert.Single(distinctSubjects);
        }
    }
}
