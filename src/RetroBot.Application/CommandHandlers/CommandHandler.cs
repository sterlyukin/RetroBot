using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

public abstract class CommandHandler
{
    private readonly IStorage storage;

    protected CommandHandler(IStorage storage)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }
    
    public abstract Task<string> ExecuteAsync(object? sender, MessageEventArgs info);

    protected async Task UpdateUserStateAsync(long userId, UserAction action)
    {
        var user = await storage.TryGetByUserIdAsync(userId);
        if (user is null)
        {
            throw new BusinessException("Sorry, current user is unknown.");
        }

        var stateMachine = new StateMachine.StateMachine(user.State);
        user.State = stateMachine.ChangeState(action);

        await storage.TryUpdateUserAsync(user);
    }
}