namespace RetroBot.Application.Exceptions;

public class BusinessException : ApplicationException
{
    public BusinessException(string message) : base(message)
    {
    }
}