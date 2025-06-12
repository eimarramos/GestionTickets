using GestionTickets.Domain.Repositories;
using GestionTickets.Infrastructure.Data;
using GestionTickets.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GestionTickets.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        string DbConnetion = $"Filename={Constants.DatabasePath}";

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlite(DbConnetion);
        });

        services.AddScoped<ITicketRepository, TicketRepository>();

        return services;
    }
}