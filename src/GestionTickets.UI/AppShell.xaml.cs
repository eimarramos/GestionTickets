
using GestionTickets.UI.Views.CreateTicket;
using GestionTickets.UI.Views.Tickets;

namespace GestionTickets.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("create_tickets", typeof(CreateTicketPage));
            Routing.RegisterRoute("tickets", typeof(TicketsPage));
        }
    }
}
