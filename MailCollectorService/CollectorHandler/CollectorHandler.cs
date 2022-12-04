using MailCollectorService.Configuration;
using MailCollectorService.MessageQueue;
using MailCollectorService.Services;
using Serilog;

namespace MailCollectorService.CollectorHandler
{
    public class CollectorHandler : ICollectorHandler
    {
        private readonly MessageQueueOptions _messageQueueOptions;

        private readonly IEmailCollectorService _emailCollectorService;
        private readonly IMessageQueue _eventQueue;

        private CancellationTokenSource? _cancellationTokenSource;

        private bool isRunning;

        public CollectorHandler(IConfiguration configuration, IEmailCollectorService emailCollectorService, IMessageQueue eventQueue)
        {
            _messageQueueOptions = configuration.GetSection(MessageQueueOptions.MessageQueue).Get<MessageQueueOptions>();

            _emailCollectorService = emailCollectorService;

            _eventQueue = eventQueue;
        }

        public async Task StartCollector()
        {
            if (isRunning)
                return;

            Log.Information("Received command to start collecting.");

            _cancellationTokenSource = new();

            await CollectEmails(_cancellationTokenSource.Token);

            return;
        }

        public Task StopCollector()
        {
            if (_cancellationTokenSource is null)
                return Task.CompletedTask;

            Log.Information("Received command to stop collecting.");

            _cancellationTokenSource.Cancel();

            return Task.CompletedTask;
        }

        public async Task CollectEmails(CancellationToken cancellationToken)
        {
            try
            {
                isRunning = true;

                Log.Information("Email collection started.");

                do
                {
                    var emails = await _emailCollectorService.GetEmails(cancellationToken);

                    if (emails is null || emails.Count == 0)
                    {
                        Log.Information("No more emails found. Stopping collection of emails.");

                        break;
                    }

                    var detailedEmails = await _emailCollectorService.GetEmailDetails(emails, cancellationToken);

                    if (detailedEmails is null || detailedEmails.Count == 0)
                    {
                        Log.Information("No more emails found. Stopping collection of emails.");

                        break;
                    }

                    _eventQueue.PublishToQueue(_messageQueueOptions.Exchange, _messageQueueOptions.RoutingKeyCollected, detailedEmails);

                    Log.Information($"Email batch sent to queue with {detailedEmails.Count} emails.");

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
