using MailCollectorService.Services;
using Microsoft.AspNetCore.Mvc;

namespace MailCollectorService.Controllers
{
    [ApiController]
    [Route("v1/mailcollector")]
    public class MailCollectorController
    {
        private IEmailCollectorService _collectorService;

        public MailCollectorController(IEmailCollectorService collectorService)
        {
            _collectorService = collectorService;
        }

        [HttpGet("ping")]
        public string Ping()
            => "pong";

        [HttpGet]
        public async Task StartCollectingMessages()
        {
            

            //TODO What input will be needed?
        }

        //TODO StopCollectingMessages
    }
}
