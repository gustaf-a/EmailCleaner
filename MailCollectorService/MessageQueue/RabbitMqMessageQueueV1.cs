using MailCollectorService.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
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

        ////Default port for RabbitMQ is used: 5672
    }

    public void PublishToQueue(string exchange, string routingKey, object messageData)
    {
        var body = Encoding.UTF8.GetBytes(CreateMessageBody(messageData));

        _channel.BasicPublish(exchange: exchange,
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);
    }

    private static string CreateMessageBody(object eventData)
        => JsonConvert.SerializeObject(eventData);
}
