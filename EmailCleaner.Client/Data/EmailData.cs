namespace EmailCleaner.Client.Data;

public class EmailData
{
    public int Id { get; set; }

    public int ThreadId { get; set; }

    public string Subject { get; set; }

    public string SenderAddress { get; set; }

    public List<string> Tags { get; set; }
}
