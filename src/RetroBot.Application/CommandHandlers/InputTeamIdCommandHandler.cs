using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.StateMachine;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

public sealed class InputTeamIdCommandHandler : CommandHandler
{
    private readonly IStorage storage;
    
    public InputTeamIdCommandHandler(IStorage storage) : base(storage)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }
    
    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        if (!Guid.TryParse(info.Message.Text, out var teamId))
        {
            return "Sorry, you entered invalid team id";
        }

        var getTeamResult = await storage.TryGetByTeamIdAsync(teamId);
        if (!getTeamResult.IsSuccess)
        {
            return "Sorry, you entered non-existent team id";
        }

        var getUserResult = await storage.TryGetByUserIdAsync(info.Message.From.Id);
        if (!getUserResult.IsSuccess)
        {
            throw new Exception("User wasn't found");
        }

        var currentUser = getUserResult.Data;
        var currentTeam = getTeamResult.Data;

        await UpdateUserStateAsync(currentUser.Id, UserAction.EnteredTeamId);
        
        //TODO: fix
        currentTeam.Users.Add(currentUser);
        return "Congratulations!\n" +
                                       "You you have successfully joined team.\n" +
                                       "Quizzes will be sent to you once a week.";
    }
}