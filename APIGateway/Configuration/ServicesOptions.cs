namespace APIGateway.Configuration;

public class ServicesOptions
{
    public const string Services = "Services";

    public string MailCollectorServiceUri { get; set; } = string.Empty;
    public string MailProviderServiceUri { get; set; } = string.Empty;
}
