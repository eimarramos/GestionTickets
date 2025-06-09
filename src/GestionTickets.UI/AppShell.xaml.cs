
using GestionTickets.UI.Views.CreateTickets;
using GestionTickets.UI.Views.Tickets;

namespace GestionTickets.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("create_tickets", typeof(CreateTicketsPage));
            Routing.RegisterRoute("tickets", typeof(TicketsPage));
        }
    }
}
