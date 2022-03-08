namespace RetroBot.Application;

public sealed class Messages
{
    public string Greeting { get; set; } = default!;
    public string SuggestionToEnterTeamId { get; set; } = default!;
    public string SuggestionToEnterTeamName { get; set; } = default!;
    public string SuggestionToEnterTeamleadEmail { get; set; } = default!;
    public string SuccessfullyCreateTeam { get; set; } = default!;
    public string SuccessfullyJoinTeam { get; set; } = default!;

    public string UnknownUser { get; set; } = default!;
    public string InvalidTeamId { get; set; } = default!;
    public string NonexistentTeamId { get; set; } = default!;

    public string StartMenuCommand { get; set; } = default!;
    public string StartMenuCommandDescription { get; set; } = default!;
    public string JoinTeamMenuCommand { get; set; } = default!;
    public string JoinTeamMenuCommandDescription { get; set; } = default!;
    public string CreateTeamMenuCommand { get; set; } = default!;
    public string CreateTeamMenuCommandDescription { get; set; } = default!;
    
    public string IllegalCommand { get; set; } = default!;

    public string TryAgain { get; set; } = default!;
}