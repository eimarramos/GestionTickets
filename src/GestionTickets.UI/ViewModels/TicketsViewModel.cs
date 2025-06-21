using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionTickets.Application.Services;
using GestionTickets.Domain.Entities;
using GestionTickets.UI.Models;
using GestionTickets.UI.Utils;
using QuestPDF.Infrastructure;

namespace GestionTickets.UI.ViewModels
{
    public partial class TicketsViewModel : BaseViewModel
    {
        private readonly TicketService _ticketService;

        public TicketsViewModel(TicketService ticketService)
        {
            Title = "Tickets";
            _ticketService = ticketService;
        }

        [ObservableProperty]
        public partial decimal Total { get; set; } = 0;

        [ObservableProperty]
        public partial MonthYearItem? SelectedMonthYear { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<MonthYearItem> AvailableMonths { get; set; } = [];

        [ObservableProperty]
        public partial ObservableCollection<Ticket> Tickets { get; set; } = [];

        [ObservableProperty]
        public partial Ticket? SelectedTicket { get; set; } = null;

        public async Task InitializeAsync()
        {
            try
            {
                IsBusy = true;
                await LoadAvailableMonthsAsync();
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

        [RelayCommand]
        public async Task DeleteTicket(int id)
        {
            try
            {
                var shouldDelete = await DisplayAlert("Borrar ticket", "¿Seguro que quieres borrar el ticket?", "Si", "No");

                if (!shouldDelete) return;

                IsBusy = true;

                await _ticketService.DeleteTicketAsync(id);

                var ticketToRemove = Tickets.FirstOrDefault(t => t.Id == id);
                if (ticketToRemove != null)
                {
                    Tickets.Remove(ticketToRemove);

                    CalculateTotal(Tickets);
                }
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

        [RelayCommand]
        public async Task GoToEditTicket(int id)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var parameters = new ShellNavigationQueryParameters
                {
                    { "TicketId", id }
                };
                await Shell.Current.GoToAsync("edit_ticket", parameters);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadAvailableMonthsAsync()
        {
            AvailableMonths.Clear();
            var items = await _ticketService.GetAvailableMonthsAndYearsAsync();
            foreach (var (year, month) in items)
            {
                AvailableMonths.Add(new MonthYearItem { Year = year, Month = month });
            }
            SelectedMonthYear = AvailableMonths.FirstOrDefault();
        }

        partial void OnSelectedMonthYearChanged(MonthYearItem? value)
        {
            if (value == null) return;
            _ = LoadTicketsByMonthAsync(value);
        }

        public async Task LoadTicketsByMonthAsync(MonthYearItem month)
        {
            try
            {
                IsBusy = true;
                var tickets = await _ticketService.GetTicketsByMonthAsync(month.Month, month.Year);
                Tickets = new ObservableCollection<Ticket>(tickets);
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

        partial void OnTicketsChanged(ObservableCollection<Ticket> value)
        {
            CalculateTotal(value);
        }

        public void CalculateTotal(ObservableCollection<Ticket> tickets)
        {
            Total = tickets.Sum(t => t.Price);
        }

        [RelayCommand]
        public async Task SharePdf()
        {
            try
            {
                IsBusy = true;

                QuestPDF.Settings.License = LicenseType.Community;

                await GneratePdf();
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

        public async Task GneratePdf()
        {
#if WINDOWS
            if (SelectedMonthYear == null || SelectedMonthYear.Month == 0 || SelectedMonthYear.Year == 0)
            {
                await DisplayToast("Por favor, selecciona un mes y año válidos.");
                return;
            }

            var fileName = $"Ticket_{SelectedMonthYear.Display}.pdf";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            var pdfBytes = PdfGenerator.GenerateTicketPdf(
                            "Tickets del mes",
                            SelectedMonthYear.Month,
                            SelectedMonthYear.Year,
                            Tickets);
            File.WriteAllBytes(filePath, pdfBytes);

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
#else
            await DisplayToast("La generación de PDF solo está disponible en Windows.");
#endif
        }
    }
}
