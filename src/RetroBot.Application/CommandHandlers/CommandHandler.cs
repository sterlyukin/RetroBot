using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Core.Entities;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal abstract class CommandHandler
{
    private readonly IStorage storage;
    private readonly Messages messages;

    protected CommandHandler(IStorage storage, Messages messages)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public abstract Task<string> ExecuteAsync(object? sender, MessageEventArgs info);

    protected async Task<User> UpdateUserStateAsync(long userId, UserAction action)
    {
        var user = await storage.TryGetByUserIdAsync(userId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var stateMachine = new StateMachine.StateMachine(user.State);
        user.State = stateMachine.ChangeState(action);

        return await storage.TryUpdateUserAsync(user);
    }

    protected async Task UpdateTeamIncludeUsersAsync(Team team, User user)
    {
        var updatedUser = await UpdateUserStateAsync(user.Id, UserAction.EnteredTeamleadEmail);
        team.Users.ToList().ForEach(currentUser =>
        {
            if (currentUser.Id == updatedUser.Id)
                currentUser.State = updatedUser.State;
        });
        
        await storage.TryUpdateTeamAsync(team);
    }
}