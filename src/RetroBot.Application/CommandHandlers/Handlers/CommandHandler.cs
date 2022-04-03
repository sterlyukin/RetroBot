using FluentValidation;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Core.Entities;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal abstract class CommandHandler
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly AbstractValidator<Command> validator;
    private readonly Messages messages;

    protected CommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        AbstractValidator<Command> validator,
        Messages messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    protected async Task ValidateAsync(Command request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new BusinessException(validationResult.Errors[0].ErrorMessage);
    }

    protected async Task<User> UpdateUserStateAsync(long userId, UserAction action)
    {
        var user = await userRepository.TryGetByIdAsync(userId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var stateMachine = new StateMachine.StateMachine(user.State);
        user.State = stateMachine.ChangeState(action);

        return await userRepository.TryUpdateAsync(user);
    }

    protected async Task UpdateTeamIncludeUsersAsync(Team team, User user)
    {
        var updatedUser = await UpdateUserStateAsync(user.Id, UserAction.EnteredTeamleadEmail);
        team.Users.ToList().ForEach(currentUser =>
        {
            if (currentUser.Id == updatedUser.Id)
                currentUser.State = updatedUser.State;
        });
        
        await teamRepository.TryUpdateAsync(team);
    }
}