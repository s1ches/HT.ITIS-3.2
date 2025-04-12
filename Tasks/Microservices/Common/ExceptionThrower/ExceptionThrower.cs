using Common.Exception;

namespace Common.ExceptionThrower;

public class ExceptionThrower : IExceptionThrower
{
    private static readonly Random Random = new();
    
    public void TryThrowException(string exceptionMessage)
    {
        if (Random.Next(0, 2) == 0)
            throw new RandomException(exceptionMessage);
    }
}