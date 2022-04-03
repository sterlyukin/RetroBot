using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.Validators;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class InputTeamleadEmailCommandHandler : CommandHandler, IRequestHandler<InputTeamleadEmailCommand, string>
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly Messages messages;

    public InputTeamleadEmailCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        InputTeamleadEmailCommandValidator validator,
        Messages messages) : base(userRepository, teamRepository, validator, messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<string> Handle(InputTeamleadEmailCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);
        
        var user = await userRepository.TryGetByIdAsync(request.UserId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var team = await teamRepository.TryGetByUserIdAsync(user.Id);
        if (team is null)
            throw new BusinessException(messages.NonexistentTeamId);

        team.TeamLeadEmail = request.Text;
        await UpdateTeamIncludeUsersAsync(team, user);

        return string.Format(messages.SuccessfullyCreateTeam, team.Name, team.Id);
    }
}