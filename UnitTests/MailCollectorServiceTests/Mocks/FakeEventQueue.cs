using MailCollectorService.EventQueue;

namespace MailCollectorServiceTests.Mocks
{
    internal class FakeEventQueue : IEventQueue
    {
        public int NumberOfTimesCalled = 0;

        public List<string> EventNamesToPublish = new();

        public List<object> EventDataToPublish = new();

        public void PublishToQueue(string eventName, object eventData)
        {
            NumberOfTimesCalled++;

            EventNamesToPublish.Add(eventName);
            EventDataToPublish.Add(eventData);
        }
    }
}
