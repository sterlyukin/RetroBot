using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.StateMachine;
using RetroBot.Core;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

public sealed class StartCommandHandler : CommandHandler
{
    private readonly IStorage storage;
    
    public StartCommandHandler(IStorage storage) : base(storage)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }
    
    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        var contactName = GetContactName(info);
        var greetingMessage = $"Welcome, {contactName}.\n" +
                              $"Please, look the list of available commands for further action.";

        var findUserResult = await storage.TryGetByUserIdAsync(info.Message.From.Id);
        if (findUserResult.IsSuccess)
        {
            findUserResult.Data.State = UserState.OnStartMessage;
            await storage.TryUpdateUserAsync(findUserResult.Data);
        }
        else
        {
            await storage.TryAddUserAsync(new User
            {
                Id = info.Message.From.Id,
                State = UserState.OnStartMessage,
            });
            await UpdateUserStateAsync(info.Message.From.Id, UserAction.PressedStart);
        }
        return greetingMessage;
    }
    
    private string GetContactName(MessageEventArgs info)
    {
        if (!string.IsNullOrEmpty(info.Message.From.Username))
        {
            return info.Message.From.Username;
        }

        return info.Message.From.FirstName;
    }
}