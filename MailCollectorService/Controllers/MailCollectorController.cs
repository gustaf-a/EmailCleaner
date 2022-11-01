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
    public Task StartCollectingMessages()
    {
        _collectorHandler.StartCollector();

        return Task.CompletedTask;
    }

    [HttpGet("stop")]
    public Task StopCollectingMessages()
    {
        _collectorHandler.StopCollector();

        return Task.CompletedTask;
    }
}
