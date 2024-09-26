using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);  // id kullanmadık bunu kullandık unique olarak
}).UseLightweightSessions();

// register services
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();