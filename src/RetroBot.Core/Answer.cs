namespace RetroBot.Core;

public sealed class Answer
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
    public long UserId { get; set; }
    public Guid QuestionId { get; set; }
}