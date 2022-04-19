using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Application.Validators;
using RetroBot.Core.Entities;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class InputTeamleadEmailCommandHandler : IRequestHandler<InputTeamleadEmailCommand, string>
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly InputTeamleadEmailCommandValidator validator;
    private readonly UserPostProcessor userPostProcessor;
    private readonly Messages messages;

    public InputTeamleadEmailCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        InputTeamleadEmailCommandValidator validator,
        UserPostProcessor userPostProcessor,
        Messages messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this.userPostProcessor = userPostProcessor ?? throw new ArgumentNullException(nameof(userPostProcessor));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<string> Handle(InputTeamleadEmailCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new BusinessException(validationResult.GetCombinedErrorMessage());
        
        var user = await userRepository.FindAsync(request.UserId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var team = await teamRepository.FindAsync(user.Id);
        if (team is null)
            throw new BusinessException(messages.NonexistentTeamId);

        team.TeamLeadEmail = request.Text;
        await UpdateTeamIncludeUsersAsync(team, user);

        return string.Format(messages.SuccessfullyCreateTeam, team.Name, team.Id);
    }
    
    private async Task UpdateTeamIncludeUsersAsync(Team team, User user)
    {
        var updatedUser = await userPostProcessor.UpdateUserStateAsync(user.Id, UserAction.EnteredTeamleadEmail);
        team.Users.ToList().ForEach(currentUser =>
        {
            if (currentUser.Id == updatedUser.Id)
                currentUser.State = updatedUser.State;
        });
        
        await teamRepository.UpdateAsync(team);
    }
}