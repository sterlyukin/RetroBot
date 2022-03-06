namespace RetroBot.Core.Entities;

public sealed class Question : IWithGeneratedId
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
}