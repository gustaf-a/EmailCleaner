namespace MailCollectorService.CollectorHandler;

public interface ICollectorHandler
{
    public Task StartCollector();
    public Task StopCollector();
}
