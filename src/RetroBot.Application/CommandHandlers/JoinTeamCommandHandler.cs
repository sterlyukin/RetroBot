using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;

namespace RetroBot.Application.CommandHandlers;

internal sealed class JoinTeamCommandHandler : CommandHandler, IRequestHandler<JoinTeamCommand, CommandExecutionResult>
{
    private readonly Messages messages;
    
    public JoinTeamCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        Messages messages) : base(userRepository, teamRepository, messages)
    {
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public async Task<CommandExecutionResult> Handle(JoinTeamCommand request, CancellationToken cancellationToken)
    {
        return CommandExecutionResult.Valid(messages.SuggestionToEnterTeamId);
    }
}