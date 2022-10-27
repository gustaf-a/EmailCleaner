using MailProviderService.Data;
using MailProviderService.EmailStore;
using RabbitMQ.Client;
using Serilog;

namespace MailProviderService.MessageConsumer;

public class RabbitMqMessageConsumerV1 : DefaultBasicConsumer
{
    private readonly IModel _channel;

    private readonly IEmailStore _emailStore;

    public RabbitMqMessageConsumerV1(IModel channel, IEmailStore emailStore)
    {
        _channel = channel;
        _emailStore = emailStore;
    }

    public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
    {
        Log.Information($"Message collected: {routingKey}");

        if (!body.TryParseListOf<Email>(out var emails))
            throw new Exception("Failed to parse message body.");

        _emailStore.StoreEmails(emails);

        _channel.BasicAck(deliveryTag, false);
    }
}
