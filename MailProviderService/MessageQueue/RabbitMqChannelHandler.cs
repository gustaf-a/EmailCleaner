using MailProviderService.Configuration;
using RabbitMQ.Client;
using Serilog;

namespace MailProviderService.MessageQueue;

public class RabbitMqChannelHandler : IChannelHandler
{
    private MessageQueueOptions _messageQueueOptions;

    private IModel _channel;

    private string _tag;

    public RabbitMqChannelHandler(IConfiguration configuration)
    {
        _messageQueueOptions = configuration.GetSection(MessageQueueOptions.MessageQueue).Get<MessageQueueOptions>();

        var factory = new ConnectionFactory() { HostName = _messageQueueOptions.HostName };
        var connection = factory.CreateConnection();

        _channel = connection.CreateModel();

        _channel.QueueDeclare(queue: _messageQueueOptions.RoutingKeyCollected,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        _tag = string.Empty;
    }

    public IModel GetChannel()
        => _channel;

    private bool IsCollecting()
        => !string.IsNullOrEmpty(_tag);

    public void StartCollecting(IBasicConsumer consumer)
    {
        if (IsCollecting())
        {
            Log.Information($"Ignoring command to start collecting: Already collecting.");
            return;
        }

        //This elaborate call to BasicConsume is used to avoid the other calls that are extension methods
        //The extension method make it trickier to mock the IModel in tests.
        _tag = _channel.BasicConsume(queue: _messageQueueOptions.RoutingKeyCollected,
                                     autoAck: false,
                                     consumerTag: Guid.NewGuid().ToString(),
                                     noLocal: false,
                                     exclusive: false,
                                     arguments: null,
                                     consumer: consumer);

        Log.Information($"Started consuming with tag: {_tag}.");
    }

    public void StopCollecting()
    {
        if (!IsCollecting())
        {
            Log.Information($"Ignoring command to stop collecting: Currently not collecting.");
            return;
        }

        _channel.BasicCancel(_tag);

        Log.Information($"Stopped consuming with tag: {_tag}.");

        _tag = string.Empty;
    }
}
