using AutoFixture;
using EmailCleaner.Client.Data;
using FrontEndConsole.Model.OutputHandler.TextConverter;

namespace FrontEndConsoleTests.Model.OutputHandler.TextConverter;

public class CsvTextConverterTests
{
    [Fact]
    public void Serialize_and_deserialize_retains_all_values()
    {
        //Arrange
        var converter = new CsvTextConverter();

        var fixture = new Fixture();
        var emails = fixture.CreateMany<EmailData>(20).ToList();
        var testEmailGroup = new EmailGroup(emails, "FakeGroupedByValue");

        //Act
        var convertedToText = converter.Serialize(testEmailGroup);
        
        //Add marking
        
        var result = converter.Deserialize(convertedToText);

        //Assert
        Assert.NotNull(result);

        Assert.Equal(emails.Count, result.EmailIds.Count);
    }

    [Theory]
    [InlineData("[ ]", "")]
    [InlineData("[]", "")]
    [InlineData(" [     ] ", "")]
    [InlineData("[x]", "x")]
    [InlineData(" [delete ]", "delete")]
    [InlineData("[a x]", "a x")]
    [InlineData("[save-attachments delete]", "save-attachments delete")]
    public void Deserialize_converts_processing_code(string input, string code)
    {
        //Arrange
        var converter = new CsvTextConverter();

        //Add marking

        var result = converter.Deserialize(input);

        //Assert
        Assert.NotNull(result);

        Assert.Equal(code, result.ProcessingCode);
    }
}
