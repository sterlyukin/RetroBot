using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.StateMachine;
using RetroBot.Core;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class StartCommandHandler : CommandHandler
{
    private readonly IStorage storage;
    private readonly Messages messages;
    
    public StartCommandHandler(IStorage storage, Messages messages) : base(storage, messages)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        var contactName = GetContactName(info);
        var greetingMessage = string.Format(messages.Greeting, contactName);

        var questions = await storage.TryGetQuestionsAsync();
        var user = await storage.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is not null)
        {
            user.State = UserState.OnStartMessage;
            await storage.TryUpdateUserAsync(user);
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
            return info.Message.From.Username;

        return info.Message.From.FirstName;
    }
}