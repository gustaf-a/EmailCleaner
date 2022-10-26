namespace MailCollectorService.Configuration
{
    public class EventQueueOptions
    {
        public const string EventQueue = "EventQueue";

        public string Exchange { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
    }
}
