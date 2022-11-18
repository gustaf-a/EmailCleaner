using EmailCleaner.Client.Data;

namespace FrontEndConsole.Services;

public interface IApiGatewayService
{
    public Task<List<EmailData>> GetEmails();
    public Task StartCollecting();
    public Task StopCollecting();
}
