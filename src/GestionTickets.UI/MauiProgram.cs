using CommunityToolkit.Maui;
using GestionTickets.Application;
using GestionTickets.Infrastructure;
using GestionTickets.Infrastructure.Data;
using GestionTickets.UI.ViewModels;
using GestionTickets.UI.Views.CreateTicket;
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
                .UseMauiCommunityToolkit(options =>
                {
                    options.SetShouldEnableSnackbarOnWindows(true);
                })
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
                    handlers.AddHandler(typeof(Picker), typeof(Platforms.Android.CustomPickerHandler));
#endif
                })
                .RegisterServices()
                .RegisterViewModels()
                .RegisterViews();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            MauiApp app = builder.Build();

            app.InitialiseDatabase();

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
            mauiAppBuilder.Services.AddSingleton<CreateTicketViewModel>();
            mauiAppBuilder.Services.AddSingleton<TicketsViewModel>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<CreateTicketPage>();
            mauiAppBuilder.Services.AddSingleton<TicketsPage>();

            return mauiAppBuilder;
        }

        public static void InitialiseDatabase(this MauiApp app)
        {
            var dbContext = app.Services.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.EnsureCreated();
        }
    }
}
