using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Core.Entities;

namespace RetroBot.Application;

public sealed class UserPostProcessor
{
    private readonly IUserRepository userRepository;
    private readonly Messages messages;

    public UserPostProcessor(
        IUserRepository userRepository,
        Messages messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<User> UpdateUserStateAsync(long userId, UserAction action)
    {
        var user = await userRepository.TryGetByIdAsync(userId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var stateMachine = new StateMachine.StateMachine(user.State);
        user.State = stateMachine.ChangeState(action);

        return await userRepository.TryUpdateAsync(user);
    }
}