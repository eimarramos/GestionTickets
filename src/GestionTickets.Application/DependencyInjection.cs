using GestionTickets.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GestionTickets.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<TicketService>();

        return services;
    }
}