using Microsoft.Maui.Platform;

namespace GestionTickets.UI.Platforms.Android
{
    public class CustomDatePickerHandler : Microsoft.Maui.Handlers.DatePickerHandler
    {
        protected override void ConnectHandler(MauiDatePicker platformView)
        {
            platformView.Background = null;
            platformView.SetPadding(0, 0, 0, 0);
            base.ConnectHandler(platformView);
        }
    }
}
