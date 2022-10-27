namespace MailProviderService.MessageQueue
{
    public interface IMessageQueue
    {
        public void StartCollecting();
        public void StopCollecting();
    }
}
