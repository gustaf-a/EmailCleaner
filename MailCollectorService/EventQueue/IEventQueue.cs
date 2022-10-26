namespace MailCollectorService.EventQueue
{
    public interface IEventQueue
    {
        public void PublishToQueue(string exchange, string eventName, object eventData);
    }
}
