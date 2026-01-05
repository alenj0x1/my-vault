using MyVault.Application.Interfaces.Services;
using MyVault.Application.Services;
using MyVault.Domain.Interfaces.Repositories;
using MyVault.Infrastructure.Persistence.Sqlite.Repositories;

namespace MyVault.WebApi.Extensions;

public static class ServicesExtension
{
    public static async Task AddServicesAsync(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        services.AddControllers();
        services.AddMemoryCache();

        SQLitePCL.Batteries.Init();

        services.AddScoped<IMyDayService, MyDayService>();
        services.AddTransient<IDayRepository, DayRepository>();
    }
}
