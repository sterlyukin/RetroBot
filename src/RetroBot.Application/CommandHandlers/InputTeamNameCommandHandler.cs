using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Core.Entities;
using Telegram.Bot.Args;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamNameCommandHandler : CommandHandler
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly Messages messages;
    
    public InputTeamNameCommandHandler(
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
        await teamRepository.TryAddTeamAsync(newTeam);

        return string.Format(messages.SuggestionToEnterTeamleadEmail, newTeam.Id);
    }
}