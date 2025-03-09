using Grpc.Core;

namespace CalculatorServer.Services;

public class CalculatorService : Calculator.CalculatorBase
{
    public override Task<CalculateResponse> Calculate(CalculateRequest request, ServerCallContext context)
        => Task.FromResult(new CalculateResponse
        {
            Result = Calculate(request.FirstOperand, request.SecondOperand, request.Operation)
        });

    protected double Calculate(double firstOperand, double secondOperand, Operation operation)
        => operation switch
        {
            Operation.Plus => firstOperand + secondOperand,
            Operation.Minus => firstOperand - secondOperand,
            Operation.Multiply => firstOperand * secondOperand,
            Operation.Divide => secondOperand != 0
                ? firstOperand / secondOperand
                : throw new RpcException(new Status(StatusCode.InvalidArgument,
                    "Second argument must be not 0, when operation is Divide")),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, "Invalid operation")
        };
}