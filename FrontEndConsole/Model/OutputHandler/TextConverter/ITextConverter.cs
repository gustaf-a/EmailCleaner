using EmailCleaner.Client.Data;

namespace FrontEndConsole.Model.OutputHandler.TextConverter
{
    internal interface ITextConverter
    {
        public string Serialize(EmailGroup emailGroup);
        public MarkedEmailGroup Deserialize(string text);
        public string GetInstructions(List<string> emailActionDescriptions);
    }
}
