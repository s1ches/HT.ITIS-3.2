using OpenMeteoService = OpenMeteoServer.Server.OpenMeteoService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddGrpc();

var app = builder.Build();

app.UseRouting();
app.MapGrpcService<OpenMeteoService>();

app.Run();