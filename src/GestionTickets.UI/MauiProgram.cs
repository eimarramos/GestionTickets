using CommunityToolkit.Maui;
using GestionTickets.Application;
using GestionTickets.Infrastructure;
using GestionTickets.UI.ViewModels;
using GestionTickets.UI.Views.CreateTickets;
using GestionTickets.UI.Views.Tickets;
using Microsoft.Extensions.Logging;



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
                    fonts.AddFont("Catamaran-Regular.ttf", "CatamaranRegular");
                    fonts.AddFont("Catamaran-Medium.ttf", "CatamaranMedium");
                    fonts.AddFont("Catamaran-SemiBold.ttf", "CatamaranSemiBold");
                    fonts.AddFont("Catamaran-Bold.ttf", "CatamaranBold");
                })
                .ConfigureMauiHandlers(handlers =>
                {
#if ANDROID
                    handlers.AddHandler(typeof(Entry), typeof(Platforms.Android.CustomEntryHandler));
                    handlers.AddHandler(typeof(DatePicker), typeof(Platforms.Android.CustomDatePickerHandler));
#endif
                })
                .RegisterServices()
                .RegisterViewModels()
                .RegisterViews();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            MauiApp app = builder.Build();

            return app;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddInfrastructureServices();
            mauiAppBuilder.Services.AddApplicationServices();

            return mauiAppBuilder;
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
