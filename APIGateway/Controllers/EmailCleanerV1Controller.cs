using APIGateway.Data;
using APIGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers;

[Route("v1/emailcleaner")]
public class EmailCleanerV1Controller : Controller
{
    private readonly IMailCollectorService _mailCollectorService;
    private readonly IMailProviderService _mailProviderService;

    public EmailCleanerV1Controller(IMailCollectorService mailCollectorService, IMailProviderService mailProviderService)
    {
        _mailCollectorService = mailCollectorService;
        _mailProviderService = mailProviderService;
    }

    [HttpGet]
    public string Ping()
        => "pong";

    [HttpGet("collect/start")]
    public async Task StartCollecting()
    {
        await _mailCollectorService.StartCollecting();
    }

    [HttpGet("collect/stop")]
    public async Task StopCollecting()
    {
        await _mailCollectorService.StopCollecting();
    }

    [HttpGet("collect")]
    public async Task<List<Email>> GetCollectedEmails()
    {
        return await _mailProviderService.GetCollectedEmails();
    }
}
