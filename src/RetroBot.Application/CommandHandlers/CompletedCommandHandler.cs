using RetroBot.Application.Contracts.Services.Storage;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class CompletedCommandHandler : CommandHandler
{
    private readonly IStorage storage;
    
    public CompletedCommandHandler(IStorage storage, Messages messages) : base(storage, messages)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }
    
    public override Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        throw new NotImplementedException();
    }

    
}