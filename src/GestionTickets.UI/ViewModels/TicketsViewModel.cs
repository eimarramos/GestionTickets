using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GestionTickets.Application.Services;
using GestionTickets.Domain.Repositories;
using GestionTickets.UI.Models;

namespace GestionTickets.UI.ViewModels
{
    public partial class TicketsViewModel : BaseViewModel
    {
        private readonly TicketService _ticketService;

        public TicketsViewModel(TicketService ticketService)
        {
            Title = "Tickets";
            _ticketService = ticketService;

            LoadDataAsync();
        }

        private MonthYearItem? selectedMonthYear;

        public MonthYearItem? SelectedMonthYear
        {
            get => selectedMonthYear;
            set => SetProperty(ref selectedMonthYear, value);
        }

        private readonly ObservableCollection<MonthYearItem> availableMonths = new();
        public ObservableCollection<MonthYearItem> AvailableMonths => availableMonths;

        public async void LoadDataAsync()
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

        public async Task LoadAvailableMonthsAsync()
        {
            availableMonths.Clear();
            var items = await _ticketService.GetAvailableMonthsAndYearsAsync();
            foreach (var (year, month) in items)
            {
                availableMonths.Add(new MonthYearItem { Year = year, Month = month });
            }
            SelectedMonthYear = availableMonths.FirstOrDefault();
        }
    }
}
