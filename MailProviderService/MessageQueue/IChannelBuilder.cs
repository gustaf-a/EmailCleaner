using MailProviderService.Configuration;
using RabbitMQ.Client;

namespace MailProviderService.MessageQueue;

public interface IChannelBuilder
{
    public IModel BuildChannel(MessageQueueOptions messageQueueOptions);
}
