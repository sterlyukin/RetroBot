using RetroBot.Application.Contracts.Services.Storage;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

public sealed class CompletedCommandHandler : CommandHandler
{
    public CompletedCommandHandler(IStorage storage) : base(storage)
    {
    }
    
    public override Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        throw new NotImplementedException();
    }

    
}