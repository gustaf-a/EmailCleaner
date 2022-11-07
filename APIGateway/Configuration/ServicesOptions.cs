namespace APIGateway.Configuration;

public class ServicesOptions
{
    public const string Services = "Services";

    public string MailCollectorServiceUri { get; set; } = "http://localhost:5201/";
    public string MailProviderServiceUri { get; set; } = "http://localhost:5202/";
}
