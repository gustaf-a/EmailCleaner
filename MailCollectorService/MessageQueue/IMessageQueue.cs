namespace MailCollectorService.MessageQueue;

public interface IMessageQueue
{
    public void PublishToQueue(string exchange, string routingKey, object messageData);
}
