using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.StateMachine;
using RetroBot.Core;
using RetroBot.Core.Entities;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class StartCommandHandler : CommandHandler
{
    private readonly IStorageClient storageClient;
    private readonly Messages messages;
    
    public StartCommandHandler(IStorageClient storageClient, Messages messages) : base(storageClient, messages)
    {
        this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        var contactName = GetContactName(info);
        var greetingMessage = string.Format(messages.Greeting, contactName);

        var user = await storageClient.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is not null)
        {
            user.State = UserState.OnStartMessage;
            await storageClient.TryUpdateUserAsync(user);
        }
        else
        {
            await storageClient.TryAddUserAsync(new User
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
            return info.Message.From.Username;

        return info.Message.From.FirstName;
    }
}