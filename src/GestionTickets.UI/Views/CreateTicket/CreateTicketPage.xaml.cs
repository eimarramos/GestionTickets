using GestionTickets.UI.ViewModels;
using Microsoft.Maui.Handlers;

namespace GestionTickets.UI.Views.CreateTicket;

public partial class CreateTicketPage : ContentPage
{
    public CreateTicketPage(CreateTicketViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    private void OnCalendarTapped(object sender, EventArgs e)
    {
        var handler = datePicker.Handler as IDatePickerHandler;
#if ANDROID
        handler!.PlatformView.PerformClick();
#elif IOS || MACCATALYST
        datePicker.Focus();
#endif
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CreateTicketViewModel vm)
            await vm.InitializeAsync();
    }
}