using APIGateway.Data;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers;

[Route("/emailcleaner/v1")]
public class EmailCleanerV1Controller : Controller
{
    public EmailCleanerV1Controller()
    {

    }

    [HttpGet]
    public string Ping()
        => "pong";

    [HttpGet("collect/start")]
    public async Task<bool> StartCollecting()
    {

        return true;
    }

    [HttpGet("collect/stop")]
    public async Task<bool> StopCollecting()
    {
        return true;
    }

    [HttpGet("collect")]
    public async Task<List<Email>> GetCollectedEmails()
    {
        return new() 
        { 
            new Email()
        };
    }

    //maybe through mailprovider?
    //Download attachments(emailIds)
}
