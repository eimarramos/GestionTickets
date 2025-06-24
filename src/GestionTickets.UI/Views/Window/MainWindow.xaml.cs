namespace GestionTickets.UI.Views.Window;
using Window = Microsoft.Maui.Controls.Window;

public partial class MainWindow : Window    
{
    public MainWindow(Page page)
    {
        InitializeComponent();

        Page = page;
    }
}
