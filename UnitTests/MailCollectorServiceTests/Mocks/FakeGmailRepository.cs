using Google.Apis.Gmail.v1.Data;
using MailCollectorService.Repository;
using Newtonsoft.Json;
using System.Reflection;

namespace MailCollectorServiceTests.Mocks
{
    internal class FakeGmailRepository : IGmailRepository
    {
        private const string TestDataFolderName = "TestData";

        private string _gmailsJsonFilePath;
        private string _gmailsDetailedJsonFilePath;

        public FakeGmailRepository()
        {
            var currentDirectory = Path.GetDirectoryName(path: Assembly.GetEntryAssembly().Location);

            var samplesFolderPath = Path.Combine(currentDirectory, TestDataFolderName);

            _gmailsJsonFilePath = Path.Combine(samplesFolderPath, "gmail_emails_1_list.txt");

            _gmailsDetailedJsonFilePath = Path.Combine(samplesFolderPath, "gmail_emails_1_details.txt");
        }

        public Task<List<Message>> GetEmailDetails(List<Message> messages)
        {
            var message = GetFileContents<Message>(_gmailsDetailedJsonFilePath);

            return Task.FromResult(new List<Message> { message });
        }

        public Task<List<Message>> GetEmails()
        {
            var listMessagesResponse = GetFileContents<ListMessagesResponse>(_gmailsJsonFilePath);

            return Task.FromResult(listMessagesResponse.Messages.ToList());
        }

        private T GetFileContents<T>(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(jsonString))
                throw new Exception($"Failed to read file contents: {filePath}");

            var deserializedObject = JsonConvert.DeserializeObject<T>(jsonString);

            if (deserializedObject is null)
                throw new Exception($"Failed to deserialize file contents into type '{nameof(T)}' for file '{filePath}'.");

            return deserializedObject;
        }
    }
}
