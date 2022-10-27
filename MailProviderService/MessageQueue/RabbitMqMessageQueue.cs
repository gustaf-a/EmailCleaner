using RabbitMQ.Client;
using MailProviderService.Configuration;
using MailProviderService.MessageConsumer;
using MailProviderService.EmailStore;
using Serilog;

namespace MailProviderService.MessageQueue
{
    public class RabbitMqMessageQueue : IMessageQueue
    {
        private readonly MessageQueueOptions _messageQueueOptions;

        private readonly IBasicConsumer _consumer;

        private readonly IModel _channel;

        private string _tag;

        public RabbitMqMessageQueue(IConfiguration configuration, IChannelBuilder channelBuilder, IMessageConsumerFactory messageConsumerFactory)
        {
            _messageQueueOptions = configuration.GetSection(MessageQueueOptions.MessageQueue).Get<MessageQueueOptions>();

            _channel = channelBuilder.BuildChannel(_messageQueueOptions);

            _channel.QueueDeclare(queue: _messageQueueOptions.RoutingKeyCollected,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _consumer = messageConsumerFactory.Build(_channel);

            _tag = string.Empty;
        }

        public void StartCollecting()
        {
            if (!string.IsNullOrEmpty(_tag))
            {
                Log.Information($"Ignoring command to start collecting because tag already exists: {_tag}.");
                return;
            }

            //This elaborate call to BasicConsume is used to avoid the other calls that are extension methods
            //The extension method make it trickier to mock the IModel in tests.
            _tag = _channel.BasicConsume(queue: _messageQueueOptions.RoutingKeyCollected,
                                         autoAck: false,
                                         consumerTag: Guid.NewGuid().ToString(),
                                         noLocal:false,
                                         exclusive: false,
                                         arguments: null,
                                         consumer: _consumer);

            Log.Information($"Started consuming with tag: {_tag}.");
        }

        public void StopCollecting()
        {
            if (string.IsNullOrEmpty(_tag))
            {
                Log.Information($"Ignoring command to stop collecting because no tag exists.");
                return;
            }

            _channel.BasicCancel(_tag);

            Log.Information($"Stopped consuming with tag: {_tag}.");

            _tag = string.Empty;
        }
    }
}
