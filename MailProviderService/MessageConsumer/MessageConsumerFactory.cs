using MailProviderService.EmailStore;
using RabbitMQ.Client;

namespace MailProviderService.MessageConsumer;

public class MessageConsumerFactory : IMessageConsumerFactory
{
    private readonly IEmailStore _emailStore;

    public MessageConsumerFactory(IEmailStore emailStore)
    {
        _emailStore = emailStore;
    }

    public IBasicConsumer Build(IModel channel)
    {
        return new RabbitMqMessageConsumerV1(channel, _emailStore);
    }
}
