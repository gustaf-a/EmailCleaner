using MailProviderService.Data;
using MailProviderService.EmailStore;
using MailProviderService.MessageQueue;
using Microsoft.AspNetCore.Mvc;

namespace MailProviderService.Controllers
{
    [ApiController]
    [Route("v1/mailprovider")]
    public class MailProviderController
    {
        private readonly IMessageQueue _messageQueue;

        private readonly IEmailStore _emailStore;

        public MailProviderController(IMessageQueue messageQueue, IEmailStore emailStore)
        {
            _emailStore = emailStore;
            _messageQueue = messageQueue;
        }

        [HttpGet]
        public string Ping()
            => "pong";

        [HttpGet("collect/start")]
        public void StartCollecting()
        {
            _messageQueue.StartCollecting();
        }

        [HttpGet("collect/stop")]
        public void StopCollecting()
        {
            _messageQueue.StopCollecting();
        }

        [HttpGet("emails")]
        public async Task<List<Email>> GetEmails()
        {
            return await _emailStore.GetEmails();
        }
    }
}
