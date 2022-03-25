using MediatR;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Exceptions;
using RetroBot.Core;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace RetroBot.Application.CommandHandlers;

internal sealed class BotCommandHandler
{
    private readonly ITelegramBotClient bot;

    private readonly IMediator mediator;
    private readonly IUserRepository userRepository;
    private readonly Messages messages;

    private IDictionary<string, Command?> menuCommands;
    private IDictionary<UserState, Command?> processCommands;

    public BotCommandHandler(
        ITelegramBotClient bot,
        IMediator mediator,
        IUserRepository userRepository,
        Messages messages)
    {
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
        
        InitializeCommandHandlers();
    }

    private void InitializeCommandHandlers()
    {
        menuCommands = new Dictionary<string, Command?>
        {
            {
                messages.StartMenuCommand,
                new StartCommand()
            },
            {
                messages.JoinTeamMenuCommand,
                new JoinTeamCommand()
            },
            {
                messages.CreateTeamMenuCommand,
                new CreateTeamCommand()
            }
        };
        
        processCommands = new Dictionary<UserState, Command?>
        {
            {
                UserState.OnInputTeamId,
                new InputTeamIdCommand()
            },
            {
                UserState.OnInputTeamName,
                new InputTeamNameCommand()
            },
            {
                UserState.OnInputTeamleadEmail,
                new InputTeamleadEmailCommand()
            },
        };
    }

    public async void OnReceiveMessage(object? sender, MessageEventArgs e)
    {
        try
        {
            var containsHandler = menuCommands.TryGetValue(e.Message.Text, out var command);
            if (!containsHandler || command is null)
            {
                var user = await userRepository.TryGetByUserIdAsync(e.Message.From.Id);
                if (user is null)
                    throw new BusinessException(messages.UnknownUser);
                
                if(user.State == UserState.Completed)
                    return;
                
                var containsState =
                    processCommands.TryGetValue(user.State, out command);
                if (!containsState || command is null)
                    throw new BusinessException(messages.IllegalCommand);
            }

            InitializeCommand(command, e);
            var result = await mediator.Send(command);
            await SendMessageAsync(e.Message.From.Id, result?.ToString());
        }
        catch (Exception ex)
        {
            await SendMessageAsync(e.Message.From.Id,
                $"{ex.Message}.\n" + string.Format(messages.TryAgain, messages.StartMenuCommand));
        }
    }

    private void InitializeCommand(Command command, MessageEventArgs e)
    {
        command.UserId = e.Message.From.Id;
        command.Text = e.Message.Text;
        command.Username = e.Message.From.Username;
        command.FirstName = e.Message.From.FirstName;
    }

    private async Task SendMessageAsync(ChatId chat, string? message)
    {
        await bot.SendTextMessageAsync(
            chatId: chat,
            text: message ?? default
        );
    }
}