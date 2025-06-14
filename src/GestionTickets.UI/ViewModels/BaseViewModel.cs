using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GestionTickets.UI.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public partial bool IsBusy { get; set; }
    [ObservableProperty]
    public partial string Title { get; set; }

    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..", false);
    }

    public async Task DisplayAlert(string title, string message, string cancel)
    {
        await Shell.Current.DisplayAlert(title, message, cancel);
    }

    public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
    {
        return await Shell.Current.DisplayAlert(title, message, accept, cancel);
    }

    public async Task DisplayToast(string message)
    {
        var toast = Toast.Make(message, ToastDuration.Short, 16);

        await toast.Show();
    }
}
