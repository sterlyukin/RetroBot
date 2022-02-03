using RetroBot.Application.Contracts.Services.Storage;
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
        var getUserResult = await storage.GetByUserIdAsync(userId);
        if (!getUserResult.IsSuccess)
        {
            throw new Exception("User wasn't found");
        }

        var currentUser = getUserResult.Data;
        var stateMachine = new StateMachine.StateMachine(currentUser.State);
        currentUser.State = stateMachine.ChangeState(action);

        await storage.UpdateUserAsync(currentUser);
    }
}