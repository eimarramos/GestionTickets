namespace GestionTickets.UI.Models
{
    public class MonthYearItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Display => $"{ToMonthName(Month)} {Year}";

        private static string ToMonthName(int month)
        {
            return new[]
            {
            "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
        }[month - 1];
        }
    }
}
