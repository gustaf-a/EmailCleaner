using MailProviderService.Configuration;
using RabbitMQ.Client;

namespace MailProviderService.MessageQueue;

public class RabbitMqChannelBuilder : IChannelBuilder
{
    public IModel BuildChannel(MessageQueueOptions messageQueueOptions)
    {
        var factory = new ConnectionFactory() { HostName = messageQueueOptions.HostName };
        using var connection = factory.CreateConnection();

        return connection.CreateModel();
    }
}
