using System.Globalization;
using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace OpenMeteoServer.Server;

public class OpenMeteoService(HttpClient client)
    : OpenMeteoServer.OpenMeteoService.OpenMeteoServiceBase
{
    public override async Task GetWeatherReportsStream(Empty request, IServerStreamWriter<WeatherReport> responseStream,
        ServerCallContext context)
    {
        var httpResponseMessage =
            await client.GetAsync(
                "https://api.open-meteo.com/v1/forecast?latitude=55.796391&longitude=49.108891&hourly=temperature_2m&timezone=GMT",
                cancellationToken: context.CancellationToken);
        
        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync(context.CancellationToken);
        
        var hourly = JsonNode.Parse(responseContent)?["hourly"];

        if (hourly == null)
            throw new RpcException(Status.DefaultCancelled, "Error while parsing response");

        var timeArray = hourly["time"]?.AsArray();
        if (timeArray == null)
            throw new RpcException(Status.DefaultCancelled, "Error while parsing time response");

        var temperatureArray = hourly["temperature_2m"]?.AsArray();
        if(temperatureArray == null)
            throw new RpcException(Status.DefaultCancelled, "Error while parsing temperature response");
        
        if(timeArray.Count != temperatureArray.Count)
            throw new RpcException(Status.DefaultCancelled, "Time array length does not match temperature array length");

        var streamDataList = new List<WeatherReport>(timeArray.Count / 2 + 1);
        
        for (var i = 0; i < temperatureArray.Count; i += 2)
        {
            var temperature = double.Parse(temperatureArray[i]!.ToString(), CultureInfo.InvariantCulture);
            var time =  DateTime.SpecifyKind(DateTime.Parse(timeArray[i]!.ToString()), DateTimeKind.Utc).ToTimestamp();
            
            streamDataList.Add(new WeatherReport
            {
                Time = time,
                TemperatureInCelsius = temperature
            });
        }

        var counter = 0;
        while (!context.CancellationToken.IsCancellationRequested && counter < streamDataList.Count)
        {
            await responseStream.WriteAsync(streamDataList[counter++], context.CancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
        }
    }
}