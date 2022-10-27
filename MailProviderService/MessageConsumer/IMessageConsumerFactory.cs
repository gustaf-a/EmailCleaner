using RabbitMQ.Client;

namespace MailProviderService.MessageConsumer;

public interface IMessageConsumerFactory
{
    public IBasicConsumer Build(IModel channel);
}
