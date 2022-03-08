namespace RetroBot.Application.StateMachine;

public enum UserAction
{
    PressedStart = 0,
    PressedJoinTeam,
    PressedCreateTeam,
    EnteredTeamId,
    EnteredTeamName,
    EnteredTeamleadEmail
}