using CalculatorClient;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5022");
var client = new Calculator.CalculatorClient(channel);
var request = new CalculateRequest
{
    FirstOperand = 2.3,
    SecondOperand = 3.4,
    Operation = Operation.Plus
};

var response = await client.CalculateAsync(request);
Console.WriteLine($"Result: {response.Result}");