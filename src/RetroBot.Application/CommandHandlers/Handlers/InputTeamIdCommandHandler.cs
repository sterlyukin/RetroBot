using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Application.Validators;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class InputTeamIdCommandHandler : CommandHandler, IRequestHandler<InputTeamIdCommand, string>
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly Messages messages;
    
    public InputTeamIdCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        StandardCommandValidator validator,
        Messages messages) : base(userRepository, teamRepository, validator, messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<string> Handle(InputTeamIdCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);
        
        if (!Guid.TryParse(request.Text, out var teamId))
            throw new BusinessException(messages.InvalidTeamId);

        var team = await teamRepository.TryGetByIdAsync(teamId);
        if (team is null)
            throw new BusinessException( messages.NonexistentTeamId);

        var user = await userRepository.TryGetByIdAsync(request.UserId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var updatedUser = await UpdateUserStateAsync(user.Id, UserAction.EnteredTeamId);
        await teamRepository.TryAddUserToTeamAsync(team, updatedUser);

        return messages.SuccessfullyJoinTeam;
    }
}