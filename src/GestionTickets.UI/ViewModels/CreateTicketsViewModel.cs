using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionTickets.Application.Services;
using GestionTickets.Domain.Entities;

namespace GestionTickets.UI.ViewModels
{
    public partial class CreateTicketsViewModel : BaseViewModel
    {
        private TicketService _ticketService;

        public CreateTicketsViewModel(TicketService ticketService)
        {
            Title = "Registro de tickets";

            _ticketService = ticketService;
        }

        [ObservableProperty]
        public partial string TicketNumber { get; set; } = string.Empty;

        [ObservableProperty]
        public partial decimal TicketPrice { get; set; } = 0;

        [ObservableProperty]
        public partial DateTime TicketDate { get; set; } = DateTime.Now;

        public async Task InitializeAsync()
        {
            try
            {
                IsBusy = true;
                await GetNextDefaultTicket(TicketDate.Month, TicketDate.Year);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ClearFields()
        {
            await GetNextDefaultTicket(TicketDate.Month, TicketDate.Year);
            TicketPrice = 0;
        }

        partial void OnTicketDateChanged(DateTime value)
        {
            _ = GetNextDefaultTicket(value.Month, value.Year);
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

        public async Task GetNextDefaultTicket(int month, int year)
        {
            TicketNumber = (await _ticketService.GetNextTicketNumberFromMonthAsync(month, year)).ToString();
        }

        [RelayCommand]
        public async Task AddTicket()
        {
            try
            {
                IsBusy = true;

                await _ticketService.AddTicketAsync(new Ticket
                {
                    TicketNumber = int.Parse(string.IsNullOrEmpty(this.TicketNumber) ? "0" : this.TicketNumber),
                    Price = this.TicketPrice,
                    Date = this.TicketDate
                });

                await ClearFields();

                await DisplayToast("Ticket creado correctamente.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
