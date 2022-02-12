using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using RetroBot.Core;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace RetroBot.Application.CommandHandlers;

internal sealed class BotCommandHandler
{
    private readonly ITelegramBotClient bot;
    private readonly IStorage storage;
    private readonly Messages messages;

    private IDictionary<string, CommandHandler?> menuCommandHandlers;
    private IDictionary<UserState, CommandHandler?> stateCommandHandlers;

    public BotCommandHandler(ITelegramBotClient bot, IStorage storage, Messages messages)
    {
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
        
        InitializeCommandHandlers();
    }

    private void InitializeCommandHandlers()
    {
        menuCommandHandlers = new Dictionary<string, CommandHandler?>
        {
            {
                messages.StartMenuCommand,
                new StartCommandHandler(storage, messages)
            },
            {
                messages.JoinTeamMenuCommand,
                new JoinTeamCommandHandler(storage, messages)
            },
            {
                messages.CreateTeamMenuCommand,
                new CreateTeamCommandHandler(storage, messages)
            }
        };
        
        stateCommandHandlers = new Dictionary<UserState, CommandHandler?>
        {
            {
                UserState.OnInputTeamId,
                new InputTeamIdCommandHandler(storage, messages)
            },
            {
                UserState.OnInputTeamleadEmail,
                new InputTeamleadEmailHandler(storage, messages)
            },
            /*{
                UserState.Completed,
                new CompletedCommandHandler(storage, messages)
            },*/
        };
    }
    
    public async void OnReceiveMessage(object? sender, MessageEventArgs e)
    {
        try
        {
            var containsHandler = menuCommandHandlers.TryGetValue(e.Message.Text, out var commandHandler);
            if (!containsHandler || commandHandler is null)
            {
                var user = await storage.TryGetByUserIdAsync(e.Message.From.Id);
                if (user is null)
                    throw new BusinessException(messages.UnknownUser);
                
                if(user.State == UserState.Completed)
                    return;
                
                var containsState =
                    stateCommandHandlers.TryGetValue(user.State, out commandHandler);
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