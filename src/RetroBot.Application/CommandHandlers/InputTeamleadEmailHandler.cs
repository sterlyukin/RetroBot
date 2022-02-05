using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Core;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamleadEmailHandler : CommandHandler
{
    private readonly IStorage storage;
    private readonly Messages messages;

    public InputTeamleadEmailHandler(IStorage storage, Messages messages) : base(storage, messages)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        var user = await storage.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is null)
        {
            throw new BusinessException(messages.UnknownUser);
        }
        
        var inputTeamleadEmail = info.Message.Text;
        var newTeam = new Team
        {
            Id = Guid.NewGuid(),
            TeamLeadEmail = inputTeamleadEmail,
            Users = new List<User>
            {
                user
            }
        };
        
        await storage.TryAddTeamAsync(newTeam);
        await UpdateUserStateAsync(info.Message.From.Id, UserAction.EnteredTeamleadEmail);

        return string.Format(messages.SuccessfullyCreateTeam, newTeam.Id);
    }
}