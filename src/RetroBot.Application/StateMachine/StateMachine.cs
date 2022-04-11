using RetroBot.Core;
using Stateless;

namespace RetroBot.Application.StateMachine;

public sealed class StateMachine
{
    private readonly StateMachine<UserState, UserAction> stateMachine;
    
    public StateMachine(UserState currentState)
    {
        stateMachine = new StateMachine<UserState, UserAction>(currentState);

        stateMachine.Configure(UserState.OnStartMessage)
            .Permit(UserAction.PressedStart, UserState.OnJoinBot);

        stateMachine.Configure(UserState.OnJoinBot)
            .Permit(UserAction.PressedJoinTeam, UserState.OnInputTeamId);

        stateMachine.Configure(UserState.OnJoinBot)
            .Permit(UserAction.PressedCreateTeam, UserState.OnInputTeamName);
        
        stateMachine.Configure(UserState.OnInputTeamName)
            .Permit(UserAction.EnteredTeamName, UserState.OnInputTeamleadEmail);
        
        stateMachine.Configure(UserState.OnInputTeamleadEmail)
            .Permit(UserAction.EnteredTeamleadEmail, UserState.OnComplete);

        stateMachine.Configure(UserState.OnInputTeamId)
            .Permit(UserAction.EnteredTeamId, UserState.OnComplete);
    }

    public UserState ChangeState(UserAction action)
    {
        stateMachine.Fire(action);
        return stateMachine.State;
    }
}