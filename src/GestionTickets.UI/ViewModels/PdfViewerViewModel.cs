using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Web;

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

        [RelayCommand]
        private async Task SharePdf()
        {
            string file = PdfSource;

            if (file.Contains("file="))
            {
                var uri = new Uri(file, UriKind.RelativeOrAbsolute);
                var query = HttpUtility.ParseQueryString(uri.Query);
                var fileParam = query.Get("file");
                if (!string.IsNullOrEmpty(fileParam))
                {
                    file = Uri.UnescapeDataString(fileParam.Replace("file:/", "/"));
                }
            }

            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Compartir PDF",
                File = new ShareFile(file)
            });
        }
    }
}
