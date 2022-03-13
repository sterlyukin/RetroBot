using Microsoft.Extensions.Hosting;
using RetroBot.Application.CommandHandlers;
using RetroBot.Application.Contracts.Services.DataStorage;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RetroBot.Application;

public class BotClient : IHostedService
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly ITelegramBotClient bot;
    private readonly Messages messages;

    public BotClient(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        ITelegramBotClient bot,
        Messages messages)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var botCommandHandler = new BotCommandHandler(bot, userRepository, teamRepository, messages);
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