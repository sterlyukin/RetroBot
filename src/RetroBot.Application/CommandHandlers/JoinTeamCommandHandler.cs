using RetroBot.Application.Contracts.Services.Storage;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

public sealed class JoinTeamCommandHandler : CommandHandler
{
    private readonly IStorage storage;
    
    public JoinTeamCommandHandler(IStorage storage) : base(storage)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }
    
    public override Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        return Task.FromResult("Please, enter team id.");
    }
}