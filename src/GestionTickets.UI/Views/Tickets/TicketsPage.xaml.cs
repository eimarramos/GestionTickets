using GestionTickets.UI.ViewModels;
using Microsoft.Maui.Handlers;

namespace GestionTickets.UI.Views.Tickets;

public partial class TicketsPage : ContentPage
{
    public TicketsPage(TicketsViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    private void OnMonthPickerTapped(object sender, EventArgs e)
    {
        var handler = monthPicker.Handler as IPickerHandler;
#if ANDROID
        handler!.PlatformView.PerformClick();
#elif IOS || MACCATALYST
        monthPicker.Focus();
#endif
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is TicketsViewModel vm)
            await vm.InitializeAsync();
    }
}