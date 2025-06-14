using Android.Content.Res;
using Microsoft.Maui.Platform;
using Color = Android.Graphics.Color;

namespace GestionTickets.UI.Platforms.Android
{
    public class CustomPickerHandler : Microsoft.Maui.Handlers.PickerHandler
    {
        protected override void ConnectHandler(MauiPicker platformView)
        {
            platformView.Background = null;
            platformView.SetPadding(0, 0, 0, 0);
            base.ConnectHandler(platformView);
        }
    }
}
