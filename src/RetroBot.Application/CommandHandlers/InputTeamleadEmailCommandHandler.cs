using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamleadEmailCommandHandler : CommandHandler
{
    private readonly IStorageClient storageClient;
    private readonly Messages messages;

    public InputTeamleadEmailCommandHandler(IStorageClient storageClient, Messages messages) : base(storageClient, messages)
    {
        this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        var user = await storageClient.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var team = await storageClient.TryGetTeamByUserIdAsync(user.Id);
        if (team is null)
            throw new BusinessException(messages.NonexistentTeamId);
        
        team.TeamLeadEmail = info.Message.Text;
        await UpdateTeamIncludeUsersAsync(team, user);

        return string.Format(messages.SuccessfullyCreateTeam, team.Id);
    }
}