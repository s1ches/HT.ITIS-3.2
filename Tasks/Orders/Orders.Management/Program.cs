using Microsoft.EntityFrameworkCore;
using Orders.Common.Interceptors;
using Orders.Management.Data;
using Orders.Management.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrderManager, OrderManager>();

builder.Services.AddDbContext<DataContext>(x =>
    x.UseNpgsql(builder.Configuration["ConnectionString"]));

var app = builder.Build();

// Auto migrations.
using var scope = app.Services.CreateScope();
var sp = scope.ServiceProvider;
var dataContext = sp.GetRequiredService<DataContext>();

dataContext.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();