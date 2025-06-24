using CommunityToolkit.Maui.Alerts;
using Microsoft.UI.Xaml;
using System.Runtime.InteropServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestionTickets.UI.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        private static Mutex? _mutex;
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            const string mutexName = "GestionTickets.UI.SingleInstanceMutex";
            bool createdNew;

            _mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox(IntPtr.Zero, "Gestión de Tickets ya está en ejecución.", "Aviso", 0);
                Environment.Exit(0);
            }

            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
