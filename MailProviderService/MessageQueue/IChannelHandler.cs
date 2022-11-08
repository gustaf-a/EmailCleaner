using RabbitMQ.Client;

namespace MailProviderService.MessageQueue;

public interface IChannelHandler
{
    public IModel GetChannel();
    public void StartCollecting(IBasicConsumer consumer);
    public void StopCollecting();
}
