using DodoPizza;
using DodoPizza.Feaches.Basket.AddBasket;
using DodoPizza.Feaches.Basket.ChekBasket;
using DodoPizza.Feaches.Basket.DeleteBasket;
using DodoPizza.Feaches.GetMenu;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<PizzaDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(9, 4, 0))
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

GetMenuEndpoint.GetMenuMap(app);
AddBasketEndpoint.AddBasketMap(app);
ChekBasketEndpoint.ChekBasketMap(app);
DeleteBasketEndpoint.DeleteBasketMap(app);

app.Run();
