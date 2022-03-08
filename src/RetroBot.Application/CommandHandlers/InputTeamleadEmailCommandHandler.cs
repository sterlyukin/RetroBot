using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamleadEmailCommandHandler : CommandHandler
{
    private readonly IStorage storage;
    private readonly Messages messages;

    public InputTeamleadEmailCommandHandler(IStorage storage, Messages messages) : base(storage, messages)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        var user = await storage.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var team = await storage.TryGetTeamByUserIdAsync(user.Id);
        if (team is null)
            throw new BusinessException(messages.NonexistentTeamId);
        
        team.TeamLeadEmail = info.Message.Text;
        await UpdateTeamIncludeUsersAsync(team, user);

        return string.Format(messages.SuccessfullyCreateTeam, team.Id);
    }
}