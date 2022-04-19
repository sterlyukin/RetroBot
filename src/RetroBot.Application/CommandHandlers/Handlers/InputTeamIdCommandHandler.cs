using FluentValidation;
using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Application.Validators;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class InputTeamIdCommandHandler : IRequestHandler<InputTeamIdCommand, string>
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly StandardCommandValidator validator;
    private readonly UserPostProcessor userPostProcessor;
    private readonly Messages messages;
    
    public InputTeamIdCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        StandardCommandValidator validator,
        UserPostProcessor userPostProcessor,
        Messages messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this.userPostProcessor = userPostProcessor ?? throw new ArgumentNullException(nameof(userPostProcessor));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<string> Handle(InputTeamIdCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new BusinessException(validationResult.GetCombinedErrorMessage());
        
        if (!Guid.TryParse(request.Text, out var teamId))
            throw new BusinessException(messages.InvalidTeamId);

        var team = await teamRepository.FindAsync(teamId);
        if (team is null)
            throw new BusinessException( messages.NonexistentTeamId);

        var user = await userRepository.FindAsync(request.UserId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var updatedUser = await userPostProcessor.UpdateUserStateAsync(user.Id, UserAction.EnteredTeamId);
        await teamRepository.AddUserToTeamAsync(team, updatedUser);

        return messages.SuccessfullyJoinTeam;
    }
}