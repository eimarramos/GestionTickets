using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionTickets.Application.Services;
using GestionTickets.Domain.Entities;

namespace GestionTickets.UI.ViewModels
{
    public partial class EditTicketViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly TicketService _ticketService;

        [ObservableProperty]
        public partial Ticket Ticket { get; set; } = new Ticket();

        public EditTicketViewModel(TicketService ticketService)
        {
            _ticketService = ticketService;
            Title = $"Editar Ticket";
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("TicketId", out var ticketId) && ticketId is int id)
            {
                await GetTicketById(id);
            }
        }

        private async Task GetTicketById(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket != null)
            {
                Ticket = ticket;
                Title = $"Editar Ticket Nº {Ticket.TicketNumber}";
            }
        }

        [RelayCommand]
        public async Task UpdateTicket()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                await _ticketService.UpdateTicketAsync(Ticket);
                await DisplayToast("Ticket actualizado correctamente.");
                await Shell.Current.GoToAsync("..", false);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                await DisplayAlert("Error", "Ocurrió un error al actualizar el ticket.", "Aceptar");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
