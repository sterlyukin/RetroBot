using FluentValidation;
using RetroBot.Application.CommandHandlers.Commands;

namespace RetroBot.Application.Validators;

public class StandardCommandValidator : AbstractValidator<Command>
{
    public StandardCommandValidator()
    {
        RuleFor(c => c.Text)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Invalid text");
    }
}