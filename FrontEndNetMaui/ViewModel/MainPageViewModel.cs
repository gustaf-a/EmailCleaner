using CommunityToolkit.Mvvm.Input;
using FrontEndNetMaui.Model;
using FrontEndNetMaui.Model.EmailSorter;
using FrontEndNetMaui.Services;
using Serilog;
using static FrontEndNetMaui.Model.GroupByMethods;

namespace FrontEndNetMaui.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
    private IApiGatewayService _apiGatewayService;
    private IConnectivity _connectivity;

    private IEmailSorter _emailSorter;

    public MainPageViewModel(IApiGatewayService apiGatewayService, IConnectivity connectivity, IDisplayService displayService,
                            IEmailSorter emailSorter)
        : base(displayService)
    {
        _apiGatewayService = apiGatewayService;
        _connectivity = connectivity;

        _emailSorter = emailSorter;

        isCollectingEmails = true;

        GroupedEmails = new()
        {
            new EmailGroup(GroupMethod.None)
            {
                Emails = new(){ new() { Id = 0, SenderAddress = "Addresstext", Subject = "Subjecttext"} }
            }
        };

        SetAndUpdateGroupByClicked(GroupMethod.Sender);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotCollectingEmails))]
    bool isCollectingEmails;

    public bool IsNotCollectingEmails => !IsCollectingEmails;

    public GroupMethod _selectedGroupMethod;

    [ObservableProperty]
    public ObservableCollection<EmailGroup> groupedEmails;

    [RelayCommand]
    async Task StartCollectEmailsClickedAsync()
    {
        if (isBusy)
            return;

        isBusy = true;

        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayService.DisplayAlert("Connection problems", "Please ensure you're connected to the internet.", "OK");
                return;
            }

            isCollectingEmails = true;

            await _apiGatewayService.StartCollectingEmails();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception thrown when trying to start email collecting.");
            throw;
        }
        finally
        {
            isBusy = false;
        }
    }

    [RelayCommand]
    async Task StopCollectEmailsClickedAsync()
    {
        if (isBusy)
            return;

        isBusy = true;

        try
        {
            if (!_apiGatewayService.IsCollecting)
                Log.Warning("Service not collecting but command received to stop collecting.");

            if (isCollectingEmails == false)
                return;

            await _apiGatewayService.StopCollectingEmails();

            var collectedEmails = await _apiGatewayService.GetEmails();

            AddEmails(collectedEmails);

            UpdateGroupedEmails();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception thrown when trying to cancel email collecting.");
        }
        finally
        {
            isBusy = false;

            if (_apiGatewayService.IsCollecting)
                Log.Error("Failed to stop service from collecting.");
            else
                isCollectingEmails = false;
        }
    }

    public void SetAndUpdateGroupByClicked(GroupMethod groupMethod)
    {
        _selectedGroupMethod = groupMethod;

        UpdateGroupedEmails();
    }

    private void AddEmails(List<EmailData> emails)
    {
        _emailSorter.AddEmails(emails);
    }

    void UpdateGroupedEmails()
    {
        try
        {
            var groupedEmailsList = _emailSorter.GetEmailGroups(_selectedGroupMethod);

            var groupedEmailsObservable = new ObservableCollection<EmailGroup>();

            foreach (var emailGroup in groupedEmailsList)
                groupedEmailsObservable.Add(emailGroup);

            GroupedEmails = groupedEmailsObservable;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
        }
    }

    [RelayCommand]
    async Task DeleteSelectedCommand()
    {
        //TODO get selected

        //Send to correct service, probably MailCollectorService which should be renamed
    }
}
