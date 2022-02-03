using Microsoft.Extensions.Hosting;
using RetroBot.Application.CommandHandlers;
using RetroBot.Application.Contracts.Services.Storage;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RetroBot.Application.Builder;

public class BotClient : IHostedService
{
    private readonly TelegramClientOptions telegramClientOptions;
    private readonly IStorage storage;

    public BotClient(TelegramClientOptions telegramClientOptions, IStorage storage)
    {
        this.telegramClientOptions =
            telegramClientOptions ?? throw new ArgumentNullException(nameof(telegramClientOptions));
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var bot = new TelegramBotClient(telegramClientOptions.ApiKey);

        var botCommandHandler = new BotCommandHandler(bot, storage);
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
            new BotCommand
            {
                Command = "createteam",
                Description = "Create retro process for the team",
            },
            new BotCommand
            {
                Command = "jointeam",
                Description = "Join team retro process",
            },
        };

        await bot.SetMyCommandsAsync(commands);
    }
}