using GestionTickets.UI.ViewModels;
using Microsoft.Maui.Handlers;

namespace GestionTickets.UI.Views.EditTicket;

public partial class EditTicketPage : ContentPage
{
	public EditTicketPage(EditTicketViewModel vm)
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
}