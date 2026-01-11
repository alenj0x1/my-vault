using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyVault.Application.Interfaces.Services;
using MyVault.Application.Services;
using MyVault.Domain.Interfaces.Repositories;
using MyVault.Infrastructure.Persistence.Sqlite.Repositories;
using MyVault.Shared.Constants;
using MyVault.WebApi.Scalar.DocumentTransformers;
using Serilog;

namespace MyVault.WebApi.Extensions;

public static class ServicesExtension
{
    public static async Task AddServicesAsync(this IServiceCollection services, IConfiguration configuration)
    {
        // Logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        // Docs
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
        services.AddControllers();

        // Data
        services.AddMemoryCache();

        SQLitePCL.Batteries.Init();

        services.AddTransient<IDayRepository, DayRepository>();

        // Services
        services.AddScoped<IMyDayService, MyDayService>();

        // Auth
        services.AddAuthorization()
        .AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration[ConfigurationProperty.JWT_PRIVATE_KEY] ?? throw new Exception(ExceptionMessage.CONFIGURATION_PROPERTY_NOT_FOUND(ConfigurationProperty.JWT_PRIVATE_KEY)))),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });
    }
}
