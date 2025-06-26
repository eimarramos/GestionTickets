
using CommunityToolkit.Mvvm.ComponentModel;

namespace GestionTickets.UI.ViewModels
{
    public partial class PdfViewerViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        public partial string PdfSource { get; set; } = string.Empty;

        public PdfViewerViewModel()
        {
            Title = "PDF";
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("PdfSource", out var pdfSource) && pdfSource is string filePath)
            {
                PdfSource = filePath;
            }
        }
    }
}
