using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Exceptions;
using RetroBot.Application.Validators;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class JoinTeamCommandHandler : IRequestHandler<JoinTeamCommand, string>
{
    private readonly StandardCommandValidator validator;
    private readonly Messages messages;
    
    public JoinTeamCommandHandler(
        StandardCommandValidator validator,
        Messages messages)
    {
        this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public async Task<string> Handle(JoinTeamCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new BusinessException(validationResult.GetCombinedErrorMessage());

        return messages.SuggestionToEnterTeamId;
    }
}