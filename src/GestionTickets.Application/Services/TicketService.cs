using GestionTickets.Domain.Entities;
using GestionTickets.Domain.Repositories;

namespace GestionTickets.Application.Services
{
    public class TicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<ICollection<Ticket>> GetTicketsByMonthAsync(int month, int year)
        {
            return await _ticketRepository.GetTicketsByMonthAsync(month, year);
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            await _ticketRepository.AddTicketAsync(ticket);
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            await _ticketRepository.UpdateTicketAsync(ticket);
        }

        public async Task DeleteTicketAsync(int id)
        {
            await _ticketRepository.DeleteTicketAsync(id);
        }

        public async Task<ICollection<Tuple<int, int>>> GetAvailableMonthsAndYearsAsync()
        {
            return await _ticketRepository.GetAvailableMonthsAndYearsAsync();
        }
    }
}
