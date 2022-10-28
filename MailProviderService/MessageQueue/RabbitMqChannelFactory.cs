using MailProviderService.Configuration;
using RabbitMQ.Client;

namespace MailProviderService.MessageQueue;

public class RabbitMqChannelFactory : IChannelFactory
{
    public IModel Create(MessageQueueOptions messageQueueOptions)
    {
        var factory = new ConnectionFactory() { HostName = messageQueueOptions.HostName };
        using var connection = factory.CreateConnection();

        return connection.CreateModel();
    }
}
