using AuthServer.Options;
using AuthServer.Server;
using AuthServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseRouting();
app.MapGrpcService<AuthService>();

app.Run();