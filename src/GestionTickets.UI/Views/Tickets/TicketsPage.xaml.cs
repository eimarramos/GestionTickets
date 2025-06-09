using GestionTickets.UI.ViewModels;

namespace GestionTickets.UI.Views.Tickets;

public partial class TicketsPage : ContentPage
{
	public TicketsPage(TicketsViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}