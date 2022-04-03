using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.Validators;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class ResetCommandHandler : CommandHandler, IRequestHandler<ResetCommand, string>
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly Messages messages;
    
    public ResetCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        StandardCommandValidator validator,
        Messages messages) : base(userRepository, teamRepository, validator, messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<string> Handle(ResetCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        var user = await userRepository.TryGetByIdAsync(request.UserId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var team = await teamRepository.TryGetByUserIdAsync(user.Id);
        if (team is null)
            throw new BusinessException(messages.NonexistentTeamId);

        await teamRepository.TryDeleteAsync(team);
        await userRepository.TryDeleteAsync(user);
        
        return string.Empty;
    }
}