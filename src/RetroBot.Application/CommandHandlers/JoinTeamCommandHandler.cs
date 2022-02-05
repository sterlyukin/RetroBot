using RetroBot.Application.Contracts.Services.Storage;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class JoinTeamCommandHandler : CommandHandler
{
    private readonly IStorage storage;
    private readonly Messages messages;
    
    public JoinTeamCommandHandler(IStorage storage, Messages messages) : base(storage, messages)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public override Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        return Task.FromResult(messages.SuggestionToEnterTeamId);
    }
}