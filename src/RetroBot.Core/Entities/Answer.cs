namespace RetroBot.Core.Entities;

public sealed class Answer : IWithGeneratedId
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
    public long UserId { get; set; }
    public Guid QuestionId { get; set; }
}