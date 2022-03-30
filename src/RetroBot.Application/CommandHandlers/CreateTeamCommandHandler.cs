using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.StateMachine;

namespace RetroBot.Application.CommandHandlers;

internal sealed class CreateTeamCommandHandler : CommandHandler, IRequestHandler<CreateTeamCommand, CommandExecutionResult>
{
    private readonly Messages messages;
    
    public CreateTeamCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        Messages messages) : base(userRepository, teamRepository, messages)
    {
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<CommandExecutionResult> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        await UpdateUserStateAsync(request.UserId, UserAction.PressedCreateTeam);

        return CommandExecutionResult.Valid(messages.SuggestionToEnterTeamName);
    }
}