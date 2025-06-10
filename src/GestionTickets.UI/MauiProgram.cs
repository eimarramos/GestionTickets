using GestionTickets.UI.ViewModels;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using GestionTickets.UI.Views.CreateTickets;
using GestionTickets.UI.Views.Tickets;


namespace GestionTickets.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterViewModels()
                .RegisterViews();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            MauiApp app = builder.Build();

            return app;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<CreateTicketsViewModel>();
            mauiAppBuilder.Services.AddSingleton<TicketsViewModel>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<CreateTicketsPage>();
            mauiAppBuilder.Services.AddSingleton<TicketsPage>();

            return mauiAppBuilder;
        }
    }
}
