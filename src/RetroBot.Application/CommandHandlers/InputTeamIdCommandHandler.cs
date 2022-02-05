using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

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
            throw new BusinessException("Sorry, you entered invalid team id.");
        }

        var team = await storage.TryGetByTeamIdAsync(teamId);
        if (team is null)
        {
            throw new BusinessException("Sorry, you entered non-existent team id.");
        }

        var user = await storage.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is null)
        {
            throw new BusinessException("Sorry, current user is unknown.");
        }

        await UpdateUserStateAsync(user.Id, UserAction.EnteredTeamId);
        await storage.TryAddUserToTeam(team, user);

        return "Congratulations!\n" +
               "You you have successfully joined team.\n" +
               "Quizzes will be sent to you once a week.";
    }
}