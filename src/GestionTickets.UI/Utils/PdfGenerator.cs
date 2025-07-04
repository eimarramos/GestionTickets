using GestionTickets.Domain.Entities;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Globalization;
using Border = iText.Layout.Borders.Border;
using Cell = iText.Layout.Element.Cell;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using VerticalAlignment = iText.Layout.Properties.VerticalAlignment;

namespace GestionTickets.UI.Utils
{
    public static class PdfGenerator
    {
        private static readonly string Signature = "Eimar Ramos";

        public static byte[] GenerateTicketPdfIText(int month, int year, IEnumerable<Ticket> tickets)
        {
            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            var primaryColor = WebColors.GetRGBColor("#2B365E");
            var secondaryColor = WebColors.GetRGBColor("#0F172A");
            var gray100 = WebColors.GetRGBColor("#E1E1E1");
            var gray400 = WebColors.GetRGBColor("#6e6e6e");

            var headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 3, 1 })).UseAllAvailableWidth();
            headerTable.SetBackgroundColor(primaryColor).SetMarginBottom(20);
            headerTable.AddCell(new Cell().Add(new Paragraph("Tickets del mes").SetFontColor(ColorConstants.WHITE).SetFontSize(22).SimulateBold().SetPaddingLeft(5))
                .SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
            headerTable.AddCell(new Cell().Add(new Paragraph($"{month:D2} / {year}").SetFontColor(ColorConstants.WHITE).SetFontSize(16).SetPaddingRight(5))
                .SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetVerticalAlignment(VerticalAlignment.MIDDLE));
            document.Add(headerTable);

            var resumenTable = new Table(1).UseAllAvailableWidth();
            resumenTable.SetBackgroundColor(gray100).SetMarginBottom(20);
            resumenTable.AddCell(new Cell().Add(new Paragraph("Resumen de Tickets").SetFontColor(secondaryColor).SetFontSize(16).SimulateBold().SetPadding(3))
                .SetBorder(Border.NO_BORDER));
            resumenTable.AddCell(new Cell().Add(new Paragraph($"Total de tickets: {tickets.Count()}").SetFontColor(gray400).SetFontSize(12).SetPadding(3))
                .SetBorder(Border.NO_BORDER));
            resumenTable.AddCell(new Cell().Add(new Paragraph($"Total: {tickets.Sum(t => t.Price):C2}").SetFontColor(gray400).SetFontSize(12).SetPadding(3))
                .SetBorder(Border.NO_BORDER));
            document.Add(resumenTable);

            document.Add(new Paragraph("Detalle de Tickets").SetFontColor(primaryColor).SetFontSize(16).SimulateBold().SetMarginBottom(10));

            var table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 2 })).UseAllAvailableWidth();

            table.AddHeaderCell(new Cell().Add(new Paragraph("Nº").SetFontColor(ColorConstants.WHITE).SimulateBold())
                .SetBackgroundColor(primaryColor).SetPadding(6).SetBorder(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Fecha").SetFontColor(ColorConstants.WHITE).SimulateBold())
                .SetBackgroundColor(primaryColor).SetPadding(6).SetBorder(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Precio").SetFontColor(ColorConstants.WHITE).SimulateBold())
                .SetBackgroundColor(primaryColor).SetPadding(6).SetBorder(Border.NO_BORDER));

            foreach (var ticket in tickets.OrderBy(t => t.TicketNumber))
            {
                table.AddCell(new Cell().Add(new Paragraph(ticket.TicketNumber.ToString()))
                    .SetBorderBottom(new SolidBorder(gray100, 0.5f)).SetPadding(5).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(ticket.Date.ToString("dd/MM/yyyy")))
                    .SetBorderBottom(new SolidBorder(gray100, 0.5f)).SetPadding(5).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(ticket.Price.ToString("C2", CultureInfo.CurrentCulture)))
                    .SetBorderBottom(new SolidBorder(gray100, 0.5f)).SetPadding(5).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
            }
            document.Add(table);

            document.Add(new Paragraph(Signature)
                .SetFontColor(gray400).SetFontSize(10).SimulateItalic().SetTextAlignment(TextAlignment.RIGHT).SetMarginTop(20));

            document.Close();
            return ms.ToArray();
        }
    }
}
