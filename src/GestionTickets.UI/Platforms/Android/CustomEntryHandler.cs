using AndroidX.AppCompat.Widget;

namespace GestionTickets.UI.Platforms.Android
{
    public class CustomEntryHandler : Microsoft.Maui.Handlers.EntryHandler
    {
        protected override void ConnectHandler(AppCompatEditText platformView)
        {
            platformView.Background = null;
            base.ConnectHandler(platformView);
        }
    }
}