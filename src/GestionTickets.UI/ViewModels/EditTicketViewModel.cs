using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionTickets.Application.Services;
using GestionTickets.Domain.Entities;

namespace GestionTickets.UI.ViewModels
{
    public partial class EditTicketViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly TicketService _ticketService;

        private int TicketId { get; set; }

        [ObservableProperty]
        public partial string TicketNumber { get; set; } = string.Empty;

        [ObservableProperty]
        public partial decimal TicketPrice { get; set; } = 0;

        [ObservableProperty]
        public partial DateTime TicketDate { get; set; } = DateTime.Now;

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
                TicketId = ticket.Id;
                TicketNumber = ticket.TicketNumber.ToString();
                TicketPrice = ticket.Price;
                TicketDate = ticket.Date;
                Title = $"Editar Ticket # {ticket.TicketNumber}";
            }
        }

        partial void OnTicketPriceChanged(decimal value)
        {
            var rounded = Math.Round(value, 2);
            if (rounded != TicketPrice)
                TicketPrice = rounded;
        }

        partial void OnTicketNumberChanged(string value)
        {
            if (decimal.TryParse(value, out var ticketNumber))
            {
                var rounded = Math.Round(ticketNumber, 0);
                if (rounded.ToString() != TicketNumber)
                    TicketNumber = rounded.ToString();
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

                await _ticketService.UpdateTicketAsync(new Ticket
                {
                    Id = this.TicketId,
                    TicketNumber = int.Parse(string.IsNullOrEmpty(this.TicketNumber) ? "0" : this.TicketNumber),
                    Price = this.TicketPrice,
                    Date = this.TicketDate
                });

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
