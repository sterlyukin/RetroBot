using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.StateMachine;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

public sealed class CreateTeamCommandHandler : CommandHandler
{
    private readonly IStorage storage;

    public CreateTeamCommandHandler(IStorage storage) : base(storage)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        await UpdateUserStateAsync(info.Message.From.Id, UserAction.PressedCreateTeam);
        return "Input teamlead email, please";
    }
}