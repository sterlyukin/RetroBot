using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.StateMachine;
using RetroBot.Core;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

public sealed class InputTeamleadEmailHandler : CommandHandler
{
    private readonly IStorage storage;

    public InputTeamleadEmailHandler(IStorage storage) : base(storage)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        var getUserResult = await storage.TryGetByUserIdAsync(info.Message.From.Id);
        if (!getUserResult.IsSuccess)
        {
            throw new Exception("User wasn't found");
        }
        
        var inputTeamleadEmail = info.Message.Text;
        var newTeam = new Team
        {
            Id = Guid.NewGuid(),
            TeamLeadEmail = inputTeamleadEmail,
            Users = new List<User>
            {
                getUserResult.Data
            }
        };
        
        var addTeamResult = await storage.TryAddTeamAsync(newTeam);
        await UpdateUserStateAsync(info.Message.From.Id, UserAction.EnteredTeamleadEmail);

        return "Congratulations!\n" +
                               $"Id of your retro-team: {newTeam.Id}\n" +
                               "Use it to connect to this team.";
    }
}