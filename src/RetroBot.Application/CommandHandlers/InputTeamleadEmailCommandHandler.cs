﻿using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.Validators;

namespace RetroBot.Application.CommandHandlers;

internal sealed class InputTeamleadEmailCommandHandler : CommandHandler, IRequestHandler<InputTeamleadEmailCommand, CommandExecutionResult>
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly InputTeamleadEmailCommandValidator validator;
    private readonly Messages messages;

    public InputTeamleadEmailCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        InputTeamleadEmailCommandValidator validator,
        Messages messages) : base(userRepository, teamRepository, messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<CommandExecutionResult> Handle(InputTeamleadEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.TryGetByIdAsync(request.UserId);
        if (user is null)
            throw new BusinessException(messages.UnknownUser);

        var team = await teamRepository.TryGetByUserIdAsync(user.Id);
        if (team is null)
            throw new BusinessException(messages.NonexistentTeamId);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return CommandExecutionResult.Invalid(validationResult.Errors[0].ErrorMessage);

        team.TeamLeadEmail = request.Text;
        await UpdateTeamIncludeUsersAsync(team, user);

        return CommandExecutionResult.Valid(string.Format(messages.SuccessfullyCreateTeam, team.Id));
    }
}