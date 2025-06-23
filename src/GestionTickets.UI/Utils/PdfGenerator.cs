using GestionTickets.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace GestionTickets.UI.Utils
{
    public static class PdfGenerator
    {
        private static readonly string PrimaryColor = "#2B365E";
        private static readonly string SecondaryColor = "#0F172A";
        private static readonly string Gray100 = "#E1E1E1";
        private static readonly string Gray400 = "#6e6e6e";
        private static readonly string White = "#FFFFFF";

        public static byte[] GenerateTicketPdf(int month, int year, IEnumerable<Ticket> tickets)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(White);

                    page.Header().Background(PrimaryColor).Padding(10).Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text("Tickets del mes")
                            .FontSize(22).Bold().FontColor(White);
                        row.ConstantItem(120).AlignRight().Text($"{month:D2}/{year}")
                            .FontSize(16).FontColor(White);
                    });

                    page.Content().Column(col =>
                    {
                        col.Spacing(20);

                        col.Item().Background(Gray100).Padding(10).Column(summary =>
                        {
                            summary.Spacing(10);

                            summary.Item().Text("Resumen de Tickets")
                                .FontSize(16).Bold().FontColor(SecondaryColor);
                            summary.Item().Text($"Total de tickets: {tickets.Count()}")
                                .FontSize(12).FontColor(Gray400);
                            summary.Item().Text($"Total recaudado: {tickets.Sum(t => t.Price):C2}")
                                .FontSize(12).FontColor(Gray400);
                        });

                        col.Item().Text("Detalle de Tickets")
                            .FontSize(16).Bold().FontColor(PrimaryColor);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);  
                                columns.RelativeColumn(2);  
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellHeaderStyle).Text("Nº").FontColor(White).Bold();
                                header.Cell().Element(CellHeaderStyle).Text("Fecha").FontColor(White).Bold();
                                header.Cell().Element(CellHeaderStyle).Text("Precio").FontColor(White).Bold();
                            });

                            foreach (var ticket in tickets.OrderBy(t => t.TicketNumber))
                            {
                                table.Cell().Element(CellStyle).Text(ticket.TicketNumber.ToString());
                                table.Cell().Element(CellStyle).Text(ticket.Date.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(ticket.Price.ToString("C2", CultureInfo.CurrentCulture));
                            }

                            static IContainer CellHeaderStyle(IContainer container) =>
                                container.Background(PrimaryColor).PaddingVertical(6).PaddingHorizontal(5);

                            static IContainer CellStyle(IContainer container) =>
                                container.PaddingVertical(5).BorderBottom(0.5f).BorderColor(Gray100);
                        });

                        // Firma
                        col.Item().AlignRight().PaddingTop(20).Text("")
                            .FontSize(10).FontColor(Gray400).Italic();
                    });
                });
            }).GeneratePdf();
        }
    }
}
