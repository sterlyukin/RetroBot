using RetroBot.Application.Contracts.Services.Storage;
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
        var containsHandler = menuCommandHandlers.TryGetValue(e.Message.Text, out var commandHandler);
        if (!containsHandler || commandHandler is null)
        {
            var currentUserResult = await storage.GetByUserIdAsync(e.Message.From.Id);
            if (!currentUserResult.IsSuccess)
            {
                await SendErrorMessageAsync(e.Message.Chat);
                return;
            }

            var containsState =
                stateCommandHandlers.TryGetValue(currentUserResult.Data.State, out commandHandler);
            if (!containsState)
            {
                await SendErrorMessageAsync(e.Message.Chat);
                return;
            }
        }

        var handlerResult = await commandHandler.ExecuteAsync(sender, e);
        await bot.SendTextMessageAsync(
            chatId: e.Message.Chat,
            text: handlerResult
        );
    }

    private async Task SendErrorMessageAsync(ChatId chat)
    {
        await SendMessageAsync(chat, $"Illegal command.\n" +
                               $"Please, try again.");
    }

    private async Task SendMessageAsync(ChatId chat, string message)
    {
        await bot.SendTextMessageAsync(
            chatId: chat,
            text: message
        );
    }
}