using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamIdCommandHandler : CommandHandler
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly Messages messages;
    
    public InputTeamIdCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        Messages messages) : base(userRepository, teamRepository, messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public override async Task<string> ExecuteAsync(object? sender, MessageEventArgs info)
    {
        if (!Guid.TryParse(info.Message.Text, out var teamId))
            throw new BusinessException(messages.InvalidTeamId);

        var team = await teamRepository.TryGetByTeamIdAsync(teamId);
        if (team is null)
            throw new BusinessException( messages.NonexistentTeamId);

        var user = await userRepository.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var updatedUser = await UpdateUserStateAsync(user.Id, UserAction.EnteredTeamId);
        await teamRepository.TryAddUserToTeamAsync(team, updatedUser);

        return messages.SuccessfullyJoinTeam;
    }
}