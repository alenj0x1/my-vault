using MyVault.Application.Interfaces.Services;
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

var serviceProvider = app.Services.CreateScope().ServiceProvider;
var myDayService = serviceProvider.GetRequiredService<IMyDayService>();

var data = await myDayService.InitData();
await myDayService.InitCache(data);

app.UseHttpsRedirection();

app.Run();