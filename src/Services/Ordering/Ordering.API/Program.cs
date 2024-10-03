using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddApplicationServices()
    .AddInfrastructure(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

// configure the http request pipeline



app.Run();