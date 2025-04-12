using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Management.Data;
using Orders.OutboxProcessor.HostedServices;

var builder = WebApplication.CreateBuilder();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumers(typeof(Program).Assembly);
    
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMqHost"]);
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration["ConnectionString"]));

builder.Services.AddHostedService<OrderCreatedPublisher>();

await builder.Build().RunAsync();