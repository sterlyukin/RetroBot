using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamIdCommandHandler : CommandHandler
{
    private readonly IStorage storage;
    private readonly Messages messages;
    
    public InputTeamIdCommandHandler(IStorage storage, Messages messages) : base(storage, messages)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        if (!Guid.TryParse(info.Message.Text, out var teamId))
            throw new BusinessException(messages.InvalidTeamId);

        var team = await storage.TryGetByTeamIdAsync(teamId);
        if (team is null)
            throw new BusinessException( messages.NonexistentTeamId);

        var user = await storage.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var updatedUser = await UpdateUserStateAsync(user.Id, UserAction.EnteredTeamId);
        await storage.TryAddUserToTeamAsync(team, updatedUser);

        return messages.SuccessfullyJoinTeam;
    }
}