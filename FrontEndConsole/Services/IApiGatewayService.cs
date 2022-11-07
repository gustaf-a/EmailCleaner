using EmailCleaner.Client.Data;

namespace FrontEndConsole.Services;

internal interface IApiGatewayService
{
    public Task<List<EmailData>> GetEmails();
    public Task StartCollecting();
    public Task StopCollecting();
}
