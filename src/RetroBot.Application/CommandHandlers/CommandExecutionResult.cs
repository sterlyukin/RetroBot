namespace RetroBot.Application.CommandHandlers;

public sealed class CommandExecutionResult
{
    public bool IsValid { get; }
    public string Message { get; }

    private CommandExecutionResult(bool isValid, string message)
    {
        IsValid = isValid;
        Message = message;
    }

    public static CommandExecutionResult Valid(string message)
    {
        return new CommandExecutionResult(true, message);
    }

    public static CommandExecutionResult Invalid(string message)
    {
        return new CommandExecutionResult(false, message);
    }
}