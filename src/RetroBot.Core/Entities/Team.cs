namespace RetroBot.Core.Entities;

public sealed class Team : IWithGeneratedId
{
    public Guid Id { get; set; }
    public string TeamLeadEmail { get; set; } = default!;
    public IList<User> Users { get; set; } = default!;
}