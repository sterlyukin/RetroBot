namespace RetroBot.Application.Exceptions;

public class BusinessException : ApplicationException
{
    public BusinessException() : base(default)
    {
    }
    
    public BusinessException(string message) : base(message)
    {
    }
}