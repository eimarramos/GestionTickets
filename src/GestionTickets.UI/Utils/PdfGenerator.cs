using GestionTickets.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace GestionTickets.UI.Utils
{
    public static class PdfGenerator
    {
        public static byte[] GenerateTicketPdf(string title, int month, int year, IEnumerable<Ticket> tickets)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header()
                        .Text($"{title} - {month:D2}/{year}")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        // Tabla de tickets
                        col.Item().Table(table =>
                        {
                            // Definir columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);   // Nº
                                columns.RelativeColumn(2);    // Fecha
                                columns.RelativeColumn(2);    // Precio
                            });

                            // Encabezados
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Nº").Bold();
                                header.Cell().Element(CellStyle).Text("Fecha").Bold();
                                header.Cell().Element(CellStyle).Text("Precio").Bold();
                            });

                            // Filas de tickets
                            foreach (var ticket in tickets.OrderBy(t => t.TicketNumber))
                            {
                                table.Cell().Element(CellStyle).Text(ticket.TicketNumber.ToString());
                                table.Cell().Element(CellStyle).Text(ticket.Date.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(ticket.Price.ToString("C2"));
                            }

                            // Estilo de celda
                            static IContainer CellStyle(IContainer container) =>
                                container.PaddingVertical(5).PaddingHorizontal(2);
                        });

                        // Total
                        col.Item().AlignRight().Text($"Total: {tickets.Sum(t => t.Price):C2}")
                            .FontSize(14).Bold();
                    });
                });
            }).GeneratePdf();
        }
    }
}
