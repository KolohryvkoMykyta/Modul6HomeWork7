namespace Infrastructure.Exceptions;

public class BusinessException : Exception
{
    public BusinessException()
    {
    }

    public BusinessException(string message)
        : base(message)
    {
    }

    public BusinessException(string message, BusinessException innerException)
        : base(message, innerException)
    {
    }
}