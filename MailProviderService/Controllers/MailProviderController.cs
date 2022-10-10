using Microsoft.AspNetCore.Mvc;

namespace MailProviderService.Controllers
{
    [ApiController]
    [Route("mailprovider")]
    public class MailProviderController
    {
        public MailProviderController()
        {

        }

        [HttpGet("ping")]
        public string Ping()
            => "pong";
    }
}
