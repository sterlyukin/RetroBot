using FluentValidation;
using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Application.Validators;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, string>
{
    private readonly StandardCommandValidator validator;
    private readonly UserPostProcessor userPostProcessor;
    private readonly Messages messages;

    public CreateTeamCommandHandler(
       StandardCommandValidator validator,
        UserPostProcessor userPostProcessor,
        Messages messages)
    {
        this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this.userPostProcessor = userPostProcessor ?? throw new ArgumentNullException(nameof(userPostProcessor));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<string> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
             throw new BusinessException(validationResult.GetCombinedErrorMessage());

        await userPostProcessor.UpdateUserStateAsync(request.UserId, UserAction.PressedCreateTeam);

        return messages.SuggestionToEnterTeamName;
    }
}