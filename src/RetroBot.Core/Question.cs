namespace RetroBot.Core;

public sealed class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
}