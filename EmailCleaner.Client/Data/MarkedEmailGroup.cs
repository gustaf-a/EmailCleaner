namespace EmailCleaner.Client.Data;

public class MarkedEmailGroup
{
    public string ProcessingCode { get; private set; }
    public List<string> EmailIds { get; private set; }

    public MarkedEmailGroup(string processingCode, List<string> emailIds)
    {
        ProcessingCode = processingCode;
        EmailIds = emailIds;
    }
}
