using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Delivery.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration["ConnectionString"]));

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    var assembly = typeof(Program).Assembly;
    
    x.AddConsumers(assembly);
    
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMqHost"]);
        cfg.ConfigureEndpoints(ctx);
    });
});

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();