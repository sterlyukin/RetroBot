using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.StateMachine;
using RetroBot.Application.Validators;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class CreateTeamCommandHandler : CommandHandler, IRequestHandler<CreateTeamCommand, string>
{
    private readonly Messages messages;

    public CreateTeamCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        StandardCommandValidator validator,
        Messages messages) : base(userRepository, teamRepository, validator, messages)
    {
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<string> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);        
            
        await UpdateUserStateAsync(request.UserId, UserAction.PressedCreateTeam);

        return messages.SuggestionToEnterTeamName;
    }
}