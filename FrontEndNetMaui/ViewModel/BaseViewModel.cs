using FrontEndNetMaui.Services;

namespace FrontEndNetMaui.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        string title;

        [ObservableProperty]
        public bool isBusy;

        protected IDisplayService DisplayService;

        public BaseViewModel(IDisplayService displayService)
        {
            DisplayService = displayService;
        }
    }
}
