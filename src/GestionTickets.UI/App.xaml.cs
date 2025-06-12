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
            return new Window(new AppShell());
        }
    }
}