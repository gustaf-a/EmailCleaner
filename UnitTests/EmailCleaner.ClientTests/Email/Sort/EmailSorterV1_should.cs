using EmailCleaner.Client.Data;
using EmailCleaner.Client.Email.Sort;

namespace EmailCleaner.ClientTests.Email.Sort;

public class EmailSorterV1_should
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
    public void SortByAllMethods_Return_one_sortedEmails_for_each_sortbehavior()
    {
        //Arrange
        var emailSorter = new EmailSorterV1();

        var expectedNumberOfSortbehaviors = 2;

        //Act
        var emailResult = emailSorter.SortByAllMethods(_emails);

        //Assert
        Assert.Equal(expectedNumberOfSortbehaviors, emailResult.SortedEmails.Count);
    }



}
