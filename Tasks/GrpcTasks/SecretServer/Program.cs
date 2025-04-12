using Common.ConfigurationExtensions;
using Common.Options;
using SecretService = SecretServer.Server.SecretService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var jwtOptions = new JwtOptions();
builder.Configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
builder.Services.AddAuthentication(jwtOptions);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<SecretService>();

app.Run();