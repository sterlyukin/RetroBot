namespace RetroBot.Core;

public sealed class Team
{
    public Guid Id { get; set; }
    public string TeamLeadEmail { get; set; }
    public IList<User> Users { get; set; }
}