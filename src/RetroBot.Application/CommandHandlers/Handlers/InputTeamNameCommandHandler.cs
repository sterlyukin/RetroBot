using FluentValidation;
using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Application.Validators;
using RetroBot.Core.Entities;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class InputTeamNameCommandHandler : IRequestHandler<InputTeamNameCommand, string>
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly StandardCommandValidator validator;
    private readonly UserPostProcessor userPostProcessor;
    private readonly Messages messages;
    
    public InputTeamNameCommandHandler(
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

    public async Task<string> Handle(InputTeamNameCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new BusinessException(validationResult.GetCombinedErrorMessage());

        var user = await userRepository.FindAsync(request.UserId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);
        
        var updatedUser = await userPostProcessor.UpdateUserStateAsync(request.UserId, UserAction.EnteredTeamName);
        var newTeam = new Team
        {
            Id = Guid.NewGuid(),
            Name = request.Text,
            Users = new List<User>
            {
                updatedUser
            }
        };
        await teamRepository.AddAsync(newTeam);

        return messages.SuggestionToEnterTeamleadEmail;
    }
}