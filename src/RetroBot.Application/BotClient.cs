using Microsoft.Extensions.Hosting;
using RetroBot.Application.CommandHandlers;
using RetroBot.Application.Contracts.Services.Storage;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RetroBot.Application.Builder;

public class BotClient : IHostedService
{
    private readonly IStorage storage;
    private readonly TelegramClientOptions telegramClientOptions;
    private readonly Messages messages;

    public BotClient(IStorage storage, TelegramClientOptions telegramClientOptions, Messages messages)
    {
        this.telegramClientOptions =
            telegramClientOptions ?? throw new ArgumentNullException(nameof(telegramClientOptions));
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var bot = new TelegramBotClient(telegramClientOptions.ApiKey);

        var botCommandHandler = new BotCommandHandler(bot, storage, messages);
        bot.OnMessage += botCommandHandler.OnReceiveMessage;
        await InitializeBotCommandsAsync(bot);
        
        bot.StartReceiving();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task InitializeBotCommandsAsync(ITelegramBotClient bot)
    {
        var commands = new List<BotCommand>
        {
            new ()
            {
                Command = messages.StartMenuCommand,
                Description = messages.StartMenuCommandDescription
            },
            new ()
            {
                Command = messages.JoinTeamMenuCommand,
                Description = messages.JoinTeamMenuCommandDescription
            },
            new ()
            {
                Command = messages.CreateTeamMenuCommand,
                Description = messages.CreateTeamMenuCommandDescription
            }
        };

        await bot.SetMyCommandsAsync(commands);
    }
}