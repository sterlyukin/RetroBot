using RetroBot.Application.Contracts.Services.Storage;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class CompletedCommandHandler : CommandHandler
{
    public CompletedCommandHandler(IStorage storage, Messages messages) : base(storage, messages)
    {
    }
    
    public override Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        throw new NotImplementedException();
    }

    
}