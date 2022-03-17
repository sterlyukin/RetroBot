using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Core.Entities;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamNameCommandHandler : CommandHandler, IRequestHandler<InputTeamNameCommand, string>
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

    public async Task<string> Handle(InputTeamNameCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.TryGetByUserIdAsync(request.UserId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);
        
        var updatedUser = await UpdateUserStateAsync(request.UserId, UserAction.EnteredTeamName);
        var inputTeamName = request.Text;
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