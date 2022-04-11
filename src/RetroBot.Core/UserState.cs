namespace RetroBot.Core;

public enum UserState
{
    OnStartMessage = 0,
    OnJoinBot,
    OnInputTeamId,
    OnInputTeamName,
    OnInputTeamleadEmail,
    OnComplete,
    OnReset
}