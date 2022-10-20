using MailProviderService.Data;
using Microsoft.AspNetCore.Mvc;

namespace MailProviderService.Controllers
{
    [ApiController]
    [Route("v1/mailprovider")]
    public class MailProviderController
    {
        public MailProviderController()
        {

        }

        [HttpGet("ping")]
        public string Ping()
            => "pong";

        [HttpGet]
        public async Task<List<Email>> GetEmails()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task AddEmails()
        {

        }
        //TODO Add emails to store
    }
}
