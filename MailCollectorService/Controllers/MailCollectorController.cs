using MailCollectorService.CollectorHandler;
using Microsoft.AspNetCore.Mvc;

namespace MailCollectorService.Controllers;

[ApiController]
[Route("v1/mailcollector")]
public class MailCollectorController
{
    private ICollectorHandler _collectorHandler;

    public MailCollectorController(ICollectorHandler collectorHandler)
    {
        _collectorHandler = collectorHandler;
    }

    [HttpGet("ping")]
    public string Ping()
        => "pong";

    [HttpGet("start")]
    public async Task StartCollectingMessages()
    {
        await _collectorHandler.StartCollector();
    }

    [HttpGet("stop")]
    public async Task StopCollectingMessages()
    {
        await _collectorHandler.StopCollector();
    }
}
