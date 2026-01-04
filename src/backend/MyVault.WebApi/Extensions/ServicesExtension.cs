using MyVault.Application.Interfaces.Services;
using MyVault.Application.Services;

namespace MyVault.WebApi.Extensions;

public static class ServicesExtension
{
    public static async Task AddServicesAsync(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        services.AddControllers();
        services.AddMemoryCache();

        services.AddScoped<IMyDayService, MyDayService>();
    }
}
