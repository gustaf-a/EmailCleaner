using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MailCollectorService.EventQueue;

public class RabbitMqEventQueue : IEventQueue
{
    private readonly IModel _channel;

    public RabbitMqEventQueue()
    {
        var factory = new ConnectionFactory();
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        ////Default port for RabbitMQ is used: 5672
    }

    public void PublishToQueue(string exchange, string eventName, object eventData)
    {
        var body = Encoding.UTF8.GetBytes(CreateEventBody(eventData));

        _channel.BasicPublish(exchange: exchange,
                                routingKey: eventName,
                                basicProperties: null,
                                body: body);
    }

    private static string CreateEventBody(object eventData)
        => JsonConvert.SerializeObject(eventData);
}
