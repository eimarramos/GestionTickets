using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionTickets.Application.Services;
using GestionTickets.Domain.Entities;
using GestionTickets.UI.Models;
using GestionTickets.UI.Utils;
using System.Collections.ObjectModel;
using System.Net;

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
        public async Task GeneratePdf()
        {
            try
            {
                IsBusy = true;

                (bool IsSuccess, string Message, string pdfSource) = await UsePdfGenerator();

                if (!IsSuccess)
                {
                    await Shell.Current.DisplayAlert("Error", Message, "OK");
                    return;
                }

                await GoToPdfViewer(pdfSource);
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

        public async Task<(bool IsSuccess, string Error, string pdfSource)> UsePdfGenerator()
        {
            if (SelectedMonthYear == null)
            {
                return (false, "Asegúrate de tener un mes seleccionado.", string.Empty);
            }

            string pdfSource = string.Empty;
            var month = SelectedMonthYear.Month;
            var year = SelectedMonthYear.Year;

            string fileName = $"Tickets {month} - {year}.pdf";

            var tempDirectory = FileSystem.CacheDirectory;
            var filePath = Path.Combine(tempDirectory, fileName);

            var pdfBytes = PdfGenerator.GenerateTicketPdfIText(month, year, Tickets);

            await File.WriteAllBytesAsync(filePath, pdfBytes);

#if ANDROID
            pdfSource = $"file:///android_asset/pdfjs/web/viewer.html?file=file://{WebUtility.UrlEncode(filePath)}";
#else
            pdfSource = filePath;
#endif

            return (true, string.Empty, pdfSource);
        }

        private async Task GoToPdfViewer(string pdfSource)
        {
            var parameters = new ShellNavigationQueryParameters
                {
                    { "PdfSource", pdfSource }
                };
            await Shell.Current.GoToAsync("pdf_viewer", parameters);
        }
    }
}

