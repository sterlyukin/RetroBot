namespace RetroBot.Application.CommandHandlers.Commands;

public class Command
{
    public long UserId { get; set; }
    public string Username { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string Text { get; set; } = default!;
}