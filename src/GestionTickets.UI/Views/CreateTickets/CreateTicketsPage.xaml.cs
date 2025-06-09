using GestionTickets.UI.ViewModels;

namespace GestionTickets.UI.Views.CreateTickets;

public partial class CreateTicketsPage : ContentPage
{
	public CreateTicketsPage(CreateTicketsViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}