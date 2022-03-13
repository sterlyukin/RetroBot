using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.StateMachine;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class CreateTeamCommandHandler : CommandHandler
{
    private readonly Messages messages;
    
    public CreateTeamCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        Messages messages) : base(userRepository, teamRepository, messages)
    {
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        await UpdateUserStateAsync(info.Message.From.Id, UserAction.PressedCreateTeam);

        return messages.SuggestionToEnterTeamName;
    }
}