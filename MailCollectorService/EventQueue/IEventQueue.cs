namespace MailCollectorService.EventQueue
{
    public interface IEventQueue
    {
        public void PublishToQueue(string eventName, object eventData);
    }
}
