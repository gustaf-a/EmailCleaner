using MailCollectorService.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace MailCollectorService.MessageQueue;

public class RabbitMqMessageQueueV1 : IMessageQueue
{
    private readonly IModel _channel;

    public RabbitMqMessageQueueV1(IConfiguration configuration)
    {
        var messageQueueOptions = configuration.GetSection(MessageQueueOptions.MessageQueue).Get<MessageQueueOptions>();

        var factory = new ConnectionFactory(){ HostName = messageQueueOptions.HostName };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }

    public void PublishToQueue(string exchange, string routingKey, object messageData)
    {
        if (messageData is null)
        {
            Log.Warning($"Tried to publish null object to exchange {exchange} with routingKey {routingKey}. Ignoring message.");
            return;
        }

        var body = Encoding.UTF8.GetBytes(CreateMessageBody(messageData));

        if (body?.Length < 5)
        {
            Log.Warning($"Tried to send impossibly small body to exchange {exchange} with routingKey {routingKey}. Ignoring message {messageData.ToString()}.");
            return;
        }

        _channel.BasicPublish(exchange: exchange,
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);
    }

    private static string CreateMessageBody(object eventData)
        => JsonConvert.SerializeObject(eventData);
}
