using GestionTickets.Domain.Entities;

namespace GestionTickets.Domain.Repositories
{
    public interface ITicketRepository
    {
        Task<ICollection<Ticket>> GetTicketsByMonthAsync(int month, int year);
        Task<int> GetNextTicketNumberFromMonthAsync(int month, int year);
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task AddTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id);
        Task<ICollection<Tuple<int, int>>> GetAvailableMonthsAndYearsAsync();
    }
}
