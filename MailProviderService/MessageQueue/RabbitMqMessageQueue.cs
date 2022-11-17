using RabbitMQ.Client;
using MailProviderService.MessageConsumer;

namespace MailProviderService.MessageQueue
{
    public class RabbitMqMessageQueue : IMessageQueue
    {
        private readonly IBasicConsumer _consumer;

        private readonly IChannelHandler _channelHandler;

        public RabbitMqMessageQueue(IChannelHandler channelHandler, IMessageConsumerFactory messageConsumerFactory)
        {
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
