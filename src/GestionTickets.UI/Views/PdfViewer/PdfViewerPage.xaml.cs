using GestionTickets.UI.ViewModels;

namespace GestionTickets.UI.Views.PdfViewer;

public partial class PdfViewerPage : ContentPage
{
	public PdfViewerPage(PdfViewerViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;

#if ANDROID
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("pdfviewer", (handler, View) =>
        {
            handler.PlatformView.Settings.JavaScriptEnabled = true;
            handler.PlatformView.Settings.AllowFileAccess = true;
            handler.PlatformView.Settings.AllowFileAccessFromFileURLs = true;
            handler.PlatformView.Settings.AllowUniversalAccessFromFileURLs = true;
        });
#endif

    }
}