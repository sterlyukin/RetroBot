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
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly Messages messages;

    private IDictionary<string, CommandHandler?> menuCommandHandlers;
    private IDictionary<UserState, CommandHandler?> processCommandHandlers;

    public BotCommandHandler(
        ITelegramBotClient bot,
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        Messages messages)
    {
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
        
        InitializeCommandHandlers();
    }

    private void InitializeCommandHandlers()
    {
        menuCommandHandlers = new Dictionary<string, CommandHandler?>
        {
            {
                messages.StartMenuCommand,
                new StartCommandHandler(userRepository, teamRepository, messages)
            },
            {
                messages.JoinTeamMenuCommand,
                new JoinTeamCommandHandler(userRepository, teamRepository, messages)
            },
            {
                messages.CreateTeamMenuCommand,
                new CreateTeamCommandHandler(userRepository, teamRepository, messages)
            }
        };
        
        processCommandHandlers = new Dictionary<UserState, CommandHandler?>
        {
            {
                UserState.OnInputTeamId,
                new InputTeamIdCommandHandler(userRepository, teamRepository, messages)
            },
            {
                UserState.OnInputTeamName,
                new InputTeamNameCommandHandler(userRepository, teamRepository, messages)
            },
            {
                UserState.OnInputTeamleadEmail,
                new InputTeamleadEmailCommandHandler(userRepository, teamRepository, messages)
            },
        };
    }

    public async void OnReceiveMessage(object? sender, MessageEventArgs e)
    {
        try
        {
            var containsHandler = menuCommandHandlers.TryGetValue(e.Message.Text, out var commandHandler);
            if (!containsHandler || commandHandler is null)
            {
                var user = await userRepository.TryGetByUserIdAsync(e.Message.From.Id);
                if (user is null)
                    throw new BusinessException(messages.UnknownUser);
                
                if(user.State == UserState.Completed)
                    return;
                
                var containsState =
                    processCommandHandlers.TryGetValue(user.State, out commandHandler);
                if (!containsState || commandHandler is null)
                    throw new BusinessException(messages.IllegalCommand);
            }

            var handlerResult = await commandHandler.ExecuteAsync(sender, e);
            await SendMessageAsync(e.Message.From.Id, handlerResult);
        }
        catch (Exception ex)
        {
            await SendMessageAsync(e.Message.From.Id,
                $"{ex.Message}.\n" + string.Format(messages.TryAgain, messages.StartMenuCommand));
        }
    }

    private async Task SendMessageAsync(ChatId chat, string message)
    {
        await bot.SendTextMessageAsync(
            chatId: chat,
            text: message
        );
    }
}