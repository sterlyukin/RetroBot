﻿using FluentValidation;
using RetroBot.Application.CommandHandlers.Commands;

namespace RetroBot.Application.Validators;

public sealed class InputTeamleadEmailCommandValidator : StandardCommandValidator
{
    public InputTeamleadEmailCommandValidator()
    {
        RuleFor(c => c.Text)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email format");
    }
}