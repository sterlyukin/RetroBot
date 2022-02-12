namespace RetroBot.Core;

public sealed class Question : IWithGeneratedId
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
}