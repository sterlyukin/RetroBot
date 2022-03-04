namespace RetroBot.Core;

public class TeamReport
{
    public Guid TeamId { get; init; }
    public string TeamleadEmail { get; init; } = default!;
    public string Report { get; init; } = default!;
}