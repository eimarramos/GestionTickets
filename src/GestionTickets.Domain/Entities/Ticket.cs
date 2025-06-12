using GestionTickets.Domain.Common;

namespace GestionTickets.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public int TicketNumber { get; set; }   
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
