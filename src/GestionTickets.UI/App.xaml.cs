using GestionTickets.UI.Views.Window;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace GestionTickets.UI
{
    public partial class App : MauiApp
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new MainWindow(new AppShell());
        }
    }
}