namespace Common.ExceptionThrower;

public interface IExceptionThrower
{
    void TryThrowException(string exceptionMessage);
}