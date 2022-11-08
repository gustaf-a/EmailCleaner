using RabbitMQ.Client;
using MailProviderService.Configuration;
using MailProviderService.MessageConsumer;
using Serilog;

namespace MailProviderService.MessageQueue
{
    public class RabbitMqMessageQueue : IMessageQueue
    {
        private readonly MessageQueueOptions _messageQueueOptions;

        private readonly IBasicConsumer _consumer;

        private readonly IChannelHandler _channelHandler;

        public RabbitMqMessageQueue(IConfiguration configuration, IChannelHandler channelHandler, IMessageConsumerFactory messageConsumerFactory)
        {
            _messageQueueOptions = configuration.GetSection(MessageQueueOptions.MessageQueue).Get<MessageQueueOptions>();

            _channelHandler = channelHandler;

            _consumer = messageConsumerFactory.Build(_channelHandler.GetChannel()) ;
        }

        public void StartCollecting()
        {
            _channelHandler.StartCollecting(_consumer);
        }

        public void StopCollecting()
        {
            _channelHandler.StopCollecting();
        }
    }
}
