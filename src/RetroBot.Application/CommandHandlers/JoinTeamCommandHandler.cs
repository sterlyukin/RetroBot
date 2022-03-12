using RetroBot.Application.Contracts.Services.Storage;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class JoinTeamCommandHandler : CommandHandler
{
    private readonly Messages messages;
    
    public JoinTeamCommandHandler(IStorageClient storageClient, Messages messages) : base(storageClient, messages)
    {
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public override Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        return Task.FromResult(messages.SuggestionToEnterTeamId);
    }
}