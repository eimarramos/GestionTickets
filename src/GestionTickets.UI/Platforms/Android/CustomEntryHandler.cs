﻿using AndroidX.AppCompat.Widget;

namespace GestionTickets.UI.Platforms.Android
{
    public class CustomEntryHandler : Microsoft.Maui.Handlers.EntryHandler
    {
        protected override void ConnectHandler(AppCompatEditText platformView)
        {
            platformView.Background = null;
            platformView.SetPadding(0, 0, 0, 0);
            base.ConnectHandler(platformView);
        }
    }
}