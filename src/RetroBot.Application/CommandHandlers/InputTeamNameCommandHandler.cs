using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Core.Entities;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamNameCommandHandler : CommandHandler
{
    private readonly IStorageClient storageClient;
    private readonly Messages messages;
    
    public InputTeamNameCommandHandler(IStorageClient storageClient, Messages messages) : base(storageClient, messages)
    {
        this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        var user = await storageClient.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);
        
        var updatedUser = await UpdateUserStateAsync(info.Message.From.Id, UserAction.EnteredTeamName);
        var inputTeamName = info.Message.Text;
        var newTeam = new Team
        {
            Id = Guid.NewGuid(),
            Name = inputTeamName,
            Users = new List<User>
            {
                updatedUser
            }
        };
        await storageClient.TryAddTeamAsync(newTeam);

        return string.Format(messages.SuggestionToEnterTeamleadEmail, newTeam.Id);
    }
}