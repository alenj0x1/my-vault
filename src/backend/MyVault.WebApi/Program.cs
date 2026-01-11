using Microsoft.Extensions.Options;
using MyVault.Application.Interfaces.Services;
using MyVault.Domain.Interfaces.Repositories;
using MyVault.Infrastructure.Persistence.Sqlite;
using MyVault.Shared.Constants;
using MyVault.WebApi.Extensions;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

await builder.Services.AddServicesAsync(builder.Configuration);

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-reference", options =>
    {
        options.ShowDeveloperTools = DeveloperToolsVisibility.Never;
        options.Title = "my-vault | API Reference";
        options.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecuritySchemes = ["Bearer"]
        };
    });
}

var initializer = new Initializer(builder.Configuration[ConfigurationProperty.CONNECTION_STRING_DATABASE]
    ?? throw new Exception(ExceptionMessage.CONFIGURATION_PROPERTY_NOT_FOUND(ConfigurationProperty.CONNECTION_STRING_DATABASE)));
await initializer.ExecuteAsync();

// Add data from file
var provider = app.Services.CreateScope().ServiceProvider;

var myDayRepository = provider.GetRequiredService<IDayRepository>();
var myDayService = provider.GetRequiredService<IMyDayService>();

var day = await myDayRepository.GetAsync(1);
var days = await myDayRepository.GetAsync();

if (day is not null)
{
    day.Date = DateTime.Now;
    var updateADay = await myDayRepository.UpdateAsync(day);
}

var deleteADay = await myDayRepository.DeleteAsync(22);

// var data = await myDayService.InitDataDeprecated();

// foreach (var day in data)
// {
//     await myDayRepository.Create(day);

//     foreach (var item in day.Items)
//     {
//         await myDayRepository.CreateItem(item);
//     }
// }

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();