using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.StateMachine;
using RetroBot.Core;
using RetroBot.Core.Entities;

namespace RetroBot.Application.CommandHandlers;

internal sealed class StartCommandHandler : CommandHandler, IRequestHandler<StartCommand, CommandExecutionResult>
{
    private readonly IUserRepository userRepository;
    private readonly Messages messages;
    
    public StartCommandHandler(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        Messages messages) : base(userRepository, teamRepository, messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<CommandExecutionResult> Handle(StartCommand request, CancellationToken cancellationToken)
    {
        var contactName = GetContactName(request);
        var greetingMessage = string.Format(messages.Greeting, contactName);

        var user = await userRepository.TryGetByIdAsync(request.UserId);
        if (user is not null)
        {
            user.State = UserState.OnStartMessage;
            await userRepository.TryUpdateAsync(user);
        }
        else
        {
            await userRepository.TryAddAsync(new User
            {
                Id = request.UserId,
                State = UserState.OnStartMessage,
            });
            await UpdateUserStateAsync(request.UserId, UserAction.PressedStart);
        }

        return CommandExecutionResult.Valid(greetingMessage);
    }
    
    private string GetContactName(StartCommand request)
    {
        if (!string.IsNullOrEmpty(request.Username))
            return request.Username;

        return request.FirstName;
    }
}