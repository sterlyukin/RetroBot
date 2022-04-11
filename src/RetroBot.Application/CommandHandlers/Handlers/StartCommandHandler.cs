using FluentValidation;
using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Application.StateMachine;
using RetroBot.Application.Validators;
using RetroBot.Core;
using RetroBot.Core.Entities;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class StartCommandHandler : IRequestHandler<StartCommand, string>
{
    private readonly IUserRepository userRepository;
    private readonly UserPostProcessor userPostProcessor;
    private readonly Messages messages;
    
    public StartCommandHandler(
        IUserRepository userRepository,
        UserPostProcessor userPostProcessor,
        Messages messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.userPostProcessor = userPostProcessor ?? throw new ArgumentNullException(nameof(userPostProcessor));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async Task<string> Handle(StartCommand request, CancellationToken cancellationToken)
    {
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
            await userPostProcessor.UpdateUserStateAsync(request.UserId, UserAction.PressedStart);
        }
        
        var contactName = GetContactName(request);
        return string.Format(messages.Greeting, contactName);
    }
    
    private string GetContactName(StartCommand request)
    {
        if (!string.IsNullOrEmpty(request.Username))
            return request.Username;

        return request.FirstName;
    }
}