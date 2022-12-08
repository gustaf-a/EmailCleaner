using MailCollectorService.Configuration;
using MailCollectorService.Data;
using MailCollectorService.MessageQueue;
using MailCollectorService.Services;
using Serilog;
using System.Reactive.Linq;

namespace MailCollectorService.CollectorHandler
{
    public class CollectorHandlerRx : ICollectorHandler
    {
        private readonly MessageQueueOptions _messageQueueOptions;
        private readonly GmailOptions _gmailOptions;

        private readonly IEmailCollectorService _emailCollectorService;
        private readonly IMessageQueue _eventQueue;
        private bool _isRunning;

        private readonly IObservable<long> _getEmailTrigger;

        private IObservable<IList<Email>>? _emailsObservable;
        private CancellationTokenSource? _cancellationTokenSource;

        public CollectorHandlerRx(IConfiguration configuration, IEmailCollectorService emailCollectorService, IMessageQueue eventQueue)
        {
            _messageQueueOptions = configuration.GetSection(MessageQueueOptions.MessageQueue).Get<MessageQueueOptions>();

            _gmailOptions = configuration.GetSection(GmailOptions.Gmail).Get<GmailOptions>();

            _emailCollectorService = emailCollectorService;
            _emailCollectorService.EnsureWarm();

            _eventQueue = eventQueue;

            _getEmailTrigger = Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(_gmailOptions.UserRateLimitPeriodSeconds));
        }

        public async Task StartCollector()
        {
            if (_isRunning)
                return;

            _isRunning = true;

            Log.Information("Received command to start collecting.");

            _cancellationTokenSource = new();

            _emailsObservable = _getEmailTrigger
                                    .SelectMany(_ => _emailCollectorService.GetEmails(_cancellationTokenSource.Token))
                                    .SelectMany(emails => emails)
                                    .Select(e => _emailCollectorService.GetEmailDetails(e, _cancellationTokenSource.Token))
                                    .Select(e => e.Result)
                                    .Buffer(TimeSpan.FromMilliseconds(1000), _messageQueueOptions.EmailBufferSize);

            _emailsObservable
                        .Subscribe(
                            emails =>
                            {
                                if (emails != null && emails.Any())
                                    _eventQueue.PublishToQueue(_messageQueueOptions.Exchange, _messageQueueOptions.RoutingKeyCollected, emails);
                            },
                            ex => Log.Error(ex, "Exception encountered when trying to send to message queue."),
                            () => Log.Information("Message queue subscription completed."),
                            _cancellationTokenSource.Token);

            return;
        }

        public Task StopCollector()
        {
            Log.Information("Received command to stop collecting.");

            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();

            _cancellationTokenSource = null;

            _isRunning = false;

            return Task.CompletedTask;
        }
    }
}
