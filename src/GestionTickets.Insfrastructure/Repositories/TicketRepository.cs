using GestionTickets.Domain.Entities;
using GestionTickets.Domain.Repositories;
using GestionTickets.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionTickets.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(int id)
        {
            await _context.Tickets
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<ICollection<Ticket>> GetTicketsByMonthAsync(int month, int year)
        {
            var tickets = await _context.Tickets
                .Where(t => t.Date.Month == month && t.Date.Year == year)
                .ToListAsync();

            return tickets;
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            await _context.Tickets
                .Where(t => t.Id == ticket.Id)
                .ExecuteUpdateAsync(oldTicket => oldTicket
                    .SetProperty(t => t.TicketNumber, ticket.TicketNumber)
                    .SetProperty(t => t.Price, ticket.Price)
                    .SetProperty(t => t.Date, ticket.Date));
        }

        public async Task<ICollection<Tuple<int, int>>> GetAvailableMonthsAndYearsAsync()
        {
            var result = await _context.Tickets
                .Select(t => new { t.Date.Year, t.Date.Month })
                .Distinct() 
                .ToListAsync();

            return result.Select(t => new Tuple<int, int>(t.Year, t.Month)).ToList();
        }
    }
}
