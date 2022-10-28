using MailProviderService.Configuration;
using RabbitMQ.Client;

namespace MailProviderService.MessageQueue;

public interface IChannelFactory
{
    public IModel Create(MessageQueueOptions messageQueueOptions);
}
