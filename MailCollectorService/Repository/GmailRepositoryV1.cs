using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MailCollectorService.Configuration;
using Polly;
using Polly.Retry;
using Serilog;

namespace MailCollectorService.Repository;

public class GmailRepositoryV1 : IGmailRepository
{
    private static readonly string[] Scopes = { GmailService.Scope.MailGoogleCom };

    private string _nextPageToken = string.Empty;
    private bool _reachedEndOfEmails = false;

    private readonly GmailOptions _gmailOptions;

    private readonly GmailService _gmailService;

    private readonly long _maxEmailsPerRequest = 50;

    private const int MaxRetries = 5;
    private AsyncRetryPolicy _retryPolicy;

    public GmailRepositoryV1(IConfiguration configuration)
    {
        _gmailOptions = configuration.GetSection(GmailOptions.Gmail).Get<GmailOptions>();

        if (_gmailOptions is null)
            throw new Exception("Failed to read GmailOptions from configuration.");

        if (string.IsNullOrWhiteSpace(_gmailOptions.ApplicationName))
            throw new Exception($"Required configuration missing or empty: {nameof(_gmailOptions.ApplicationName)}");

        if (string.IsNullOrWhiteSpace(_gmailOptions.CredentialsFileName))
            throw new Exception($"Required configuration missing or empty: {nameof(_gmailOptions.CredentialsFileName)}");

        _gmailService = CreateService();

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(MaxRetries, retryCount => TimeSpan.FromMilliseconds(100 * retryCount));
    }

    private GmailService CreateService()
    {
        try
        {
            UserCredential credential;
            using (var stream =
                   new FileStream(_gmailOptions.CredentialsFileName, FileMode.Open, FileAccess.Read))
            {
                /* The file token.json stores the user's access and refresh tokens, and is created
                 automatically when the authorization flow completes for the first time. */
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            return new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _gmailOptions.ApplicationName
            });
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<bool> EnsureWarm()
    {
        return Task.FromResult(_gmailService != null);
    }

    public async Task<List<Message>> GetEmails(CancellationToken cancellationToken)
    {
        if (_reachedEndOfEmails)
            return new();

        var listMessagesResponse = await ListMessages(cancellationToken);

        if (listMessagesResponse.NextPageToken is null)
            _reachedEndOfEmails = true;

        return listMessagesResponse.Messages.ToList();
    }

    private async Task<ListMessagesResponse> ListMessages(CancellationToken cancellationToken)
    {
        var gmailSendersRequest = _gmailService.Users.Messages.List(_gmailOptions.UserId);

        gmailSendersRequest.MaxResults = _maxEmailsPerRequest;

        gmailSendersRequest.PageToken = _nextPageToken;

        if (cancellationToken.IsCancellationRequested)
            return new();

        var listMessagesResponse = await _retryPolicy.ExecuteAsync(async () =>
        {
            return await gmailSendersRequest.ExecuteAsync(cancellationToken);
        });

        if (listMessagesResponse is null)
            throw new Exception("Failed to get list of messages.");

        _nextPageToken = listMessagesResponse.NextPageToken;

        return listMessagesResponse;
    }

    public async Task<Message> GetEmailDetails(string messageId, CancellationToken cancellationToken)
    {
        var getMessagesRequest = _gmailService.Users.Messages.Get(_gmailOptions.UserId, messageId);

        if (cancellationToken.IsCancellationRequested)
            return null;

        var message = await _retryPolicy.ExecuteAsync(async () =>
        {
            return await getMessagesRequest.ExecuteAsync(cancellationToken);
        });

        if (message is null)
            Log.Warning($"Failed to get message details for id: '{messageId}'");

        return message;
    }
}
