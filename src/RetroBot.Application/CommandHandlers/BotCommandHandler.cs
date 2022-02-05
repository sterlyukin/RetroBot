using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Exceptions;
using RetroBot.Core;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace RetroBot.Application.CommandHandlers;

public class BotCommandHandler
{
    private readonly ITelegramBotClient bot;
    private readonly IStorage storage;

    private IDictionary<string, CommandHandler?> menuCommandHandlers;
    private IDictionary<UserState, CommandHandler?> stateCommandHandlers;

    public BotCommandHandler(ITelegramBotClient bot, IStorage storage)
    {
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        
        InitializeCommandHandlers();
    }

    private void InitializeCommandHandlers()
    {
        menuCommandHandlers = new Dictionary<string, CommandHandler?>
        {
            {
                "/start",
                new StartCommandHandler(storage)
            },
            {
                "/jointeam",
                new JoinTeamCommandHandler(storage)
            },
            {
                "/createteam",
                new CreateTeamCommandHandler(storage)
            }
        };
        
        stateCommandHandlers = new Dictionary<UserState, CommandHandler?>
        {
            {
                UserState.OnInputTeamId,
                new InputTeamIdCommandHandler(storage)
            },
            {
                UserState.OnInputTeamleadEmail,
                new InputTeamleadEmailHandler(storage)
            },
            {
                UserState.Completed,
                new CompletedCommandHandler(storage)
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
                var user = await storage.TryGetByUserIdAsync(e.Message.From.Id);
                if (user is null)
                    throw new BusinessException("Sorry, current user is unknown.");
                
                var containsState =
                    stateCommandHandlers.TryGetValue(user.State, out commandHandler);
                if (!containsState || commandHandler is null)
                    throw new BusinessException("Sorry, illegal command.");
            }

            var handlerResult = await commandHandler.ExecuteAsync(sender, e);
            await SendMessageAsync(e.Message.Chat, handlerResult);
        }
        catch (Exception ex)
        {
            await SendMessageAsync(e.Message.Chat, $"{ex.Message}.\n" +
                                         $"Please, press \"Start\" and try again.");
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