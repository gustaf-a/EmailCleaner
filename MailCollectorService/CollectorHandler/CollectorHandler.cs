using MailCollectorService.EventQueue;
using MailCollectorService.Services;
using Serilog;

namespace MailCollectorService.CollectorHandler
{
    public class CollectorHandler : ICollectorHandler
    {
        private readonly IEmailCollectorService _emailCollectorService;
        private readonly IEventQueue _eventQueue;

        private readonly string _eventName;

        private CancellationTokenSource? _cancellationTokenSource;

        private bool isRunning;

        public CollectorHandler(IEmailCollectorService emailCollectorService, IEventQueue eventQueue)
        {
            _emailCollectorService = emailCollectorService;

            _eventQueue = eventQueue;

            //TODO Get from configuration see repo
            _eventName = "collected";
        }

        public void StartCollector()
        {
            if (isRunning)
                return;

            Log.Information("Received command to start collecting.");

            _cancellationTokenSource = new();

            Task.Run(async () => await CollectEmails(_cancellationTokenSource.Token)).ConfigureAwait(false);

            return;
        }

        public void StopCollector()
        {
            if (_cancellationTokenSource is null)
                return;

            Log.Information("Received command to stop collecting.");

            _cancellationTokenSource.Cancel();

            return;
        }

        public async Task CollectEmails(CancellationToken cancellationToken)
        {
            try
            {
                isRunning = true;

                Log.Information("Email collection started.");

                do
                {
                    var emails = await _emailCollectorService.GetEmails();

                    if(emails.Count == 0)
                    {
                        Log.Information("No more emails found. Stopping collection of emails.");

                        break;
                    }

                    _eventQueue.PublishToQueue(_eventName, emails);

                    Log.Information($"Email batch sent to queue with {emails.Count} emails.");

                } while (!cancellationToken.IsCancellationRequested);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Exception thrown when collecting emails: {ex.Message}.");
            }
            finally
            {
                isRunning = false;

                Log.Information("Email collection stopped.");
            }
        }
    }
}
