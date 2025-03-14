using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using OpenMeteoClient;

using var channel = GrpcChannel.ForAddress("http://localhost:5027");
var client = new OpenMeteoService.OpenMeteoServiceClient(channel);
var request = new Empty();

using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

var call = client.GetWeatherReportsStream(request, cancellationToken: cancellationTokenSource.Token);

await foreach (var report in
               call.ResponseStream.ReadAllAsync(cancellationToken: cancellationTokenSource.Token))
{
    var canReset = cancellationTokenSource.TryReset();

    if (!canReset)
    {
        cancellationTokenSource.Cancel();
        throw new OperationCanceledException("Cancellation requested");
    }

    if (report == null)
        throw new ArgumentException("Weather Report is null");
    
    Console.WriteLine("{0:HH:mm:ss} погода на {1:dd.MM.yyyy HH:mm} = {2}C",
        DateTime.UtcNow,
        report.Time.ToDateTime(),
        report.TemperatureInCelsius);
}