using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Validators;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class JoinTeamCommandHandler : CommandHandler, IRequestHandler<JoinTeamCommand, string>
{
    private readonly Messages messages;
    
    public JoinTeamCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        StandardCommandValidator validator,
        Messages messages) : base(userRepository, teamRepository, validator, messages)
    {
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public async Task<string> Handle(JoinTeamCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);
        return messages.SuggestionToEnterTeamId;
    }
}