using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamleadEmailCommandHandler : CommandHandler
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly Messages messages;

    public InputTeamleadEmailCommandHandler(
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
        var user = await userRepository.TryGetByUserIdAsync(info.Message.From.Id);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var team = await teamRepository.TryGetTeamByUserIdAsync(user.Id);
        if (team is null)
            throw new BusinessException(messages.NonexistentTeamId);
        
        team.TeamLeadEmail = info.Message.Text;
        await UpdateTeamIncludeUsersAsync(team, user);

        return string.Format(messages.SuccessfullyCreateTeam, team.Id);
    }
}