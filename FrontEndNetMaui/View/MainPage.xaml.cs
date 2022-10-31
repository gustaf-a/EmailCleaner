using FrontEndNetMaui.Model;
using FrontEndNetMaui.ViewModel;

namespace FrontEndNetMaui.View;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        _viewModel = viewModel;
    }

    void OnEmailGroupsCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_viewModel is null)
            return;

        if (sender is not CollectionView collectionView)
            return;

        _viewModel.SelectedObjects = collectionView.SelectedItems.ToList();

        var itemsAreSelected = collectionView.SelectedItems.Count > 0;

        DeleteSelectedBtn.IsVisible = itemsAreSelected;
    }

    void OnGroupByClicked(object sender, EventArgs e)
    {
        if (_viewModel is null)
            return;

        if (sender is not RadioButton radioButtonSender || !radioButtonSender.IsChecked)
            return;

        var groupByMethod = GetGroupByMethod(radioButtonSender.ContentAsString());

        _viewModel.SetAndUpdateGroupByClicked(groupByMethod);
    }

    private static GroupByMethods.GroupMethod GetGroupByMethod(string content)
    {
        var contentAllLowerCase = content.ToLower();

        if (contentAllLowerCase.Contains(GroupByMethods.GroupMethod.Sender.ToString().ToLower()))
            return GroupByMethods.GroupMethod.Sender;

        if (contentAllLowerCase.Contains(GroupByMethods.GroupMethod.Subject.ToString().ToLower()))
            return GroupByMethods.GroupMethod.Subject;

        if (contentAllLowerCase.Contains(GroupByMethods.GroupMethod.Tag.ToString().ToLower()))
            return GroupByMethods.GroupMethod.Tag;

        throw new NotImplementedException($"RadioButton not connected to grouped by method.");
    }
}
