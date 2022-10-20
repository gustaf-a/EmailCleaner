using FrontEndNetMaui.Model;

namespace FrontEndNetMaui.Services
{
    public interface IApiGatewayService
    {
        bool IsCollecting { get; }

        public Task StartCollectingEmails();

        public Task StopCollectingEmails();

        public Task<List<EmailData>> GetEmails();
    }
}
