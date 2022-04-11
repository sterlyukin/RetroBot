using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Core;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace RetroBot.Application.CommandHandlers.Handlers;

internal sealed class BotCommandHandler
{
    private readonly ITelegramBotClient bot;

    private readonly IMediator mediator;
    private readonly IUserRepository userRepository;
    private readonly CommandDispatcher commandDispatcher;
    private readonly Messages messages;

    public BotCommandHandler(
        ITelegramBotClient bot,
        IMediator mediator,
        IUserRepository userRepository,
        CommandDispatcher commandDispatcher,
        Messages messages)
    {
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public async void OnReceiveMessage(object? sender, MessageEventArgs e)
    {
        try
        {
            var command = commandDispatcher.BuildCommand(e.Message.Text, e);
            if (command is null)
            {
                var user = await userRepository.TryGetByIdAsync(e.Message.From.Id);
                if (user is null)
                    throw new BusinessException(messages.UnknownUser);
                
                if(user.State == UserState.OnComplete)
                    return;

                command = commandDispatcher.BuildCommand(user.State, e);
                if (command is null)
                    throw new BusinessException(messages.IllegalCommand);
            }

            var commandExecutionResult = await mediator.Send(command);
            await SendMessageAsync(e.Message.From.Id, commandExecutionResult?.ToString());
        }
        catch (Exception ex)
        {
            await SendResetCommandAsync(e, ex.Message);
        }
    }

    private async Task SendResetCommandAsync(MessageEventArgs e, string message)
    {
        await SendMessageAsync(e.Message.From.Id,
            $"{message}.\n" + string.Format(messages.TryAgain, messages.StartMenuCommand));

        var resetCommand = commandDispatcher.BuildCommand(UserState.OnReset, e);
        if(resetCommand is not null)
            await mediator.Send(resetCommand);
    }
    
    private async Task SendMessageAsync(ChatId chat, string? message)
    {
        await bot.SendTextMessageAsync( chat, message ?? default);
    }
}