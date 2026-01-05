using MyVault.Application.Interfaces.Services;
using MyVault.Application.Services;
using MyVault.Domain.Interfaces.Repositories;
using MyVault.Infrastructure.Persistence.Sqlite;
using MyVault.Shared.Constants;
using MyVault.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
await builder.Services.AddServicesAsync(builder.Configuration);

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var initializer = new Initializer(builder.Configuration[ConfigurationProperty.CONNECTION_STRING_DATABASE]
    ?? throw new Exception(ExceptionMessage.CONFIGURATION_PROPERTY_NOT_FOUND(ConfigurationProperty.CONNECTION_STRING_DATABASE)));
await initializer.ExecuteAsync();

// Add data from file
// var provider = app.Services.CreateScope().ServiceProvider;

// var myDayRepository = provider.GetRequiredService<IDayRepository>();
// var myDayService = provider.GetRequiredService<IMyDayService>();

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

app.Run();