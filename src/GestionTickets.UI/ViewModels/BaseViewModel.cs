using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GestionTickets.UI.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private partial bool IsBusy { get; set; }
    [ObservableProperty]
    public partial string Title { get; set; }

    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..", false);
    }

    [RelayCommand]
    private async Task GoToCreateTickets()
    {
        await Shell.Current.GoToAsync("create_tickets", false);
    }

    [RelayCommand]
    private async Task GoToRickets()
    {
        await Shell.Current.GoToAsync("tickets", false);
    }
}
