using MediatR;
using Microsoft.Extensions.Hosting;
using RetroBot.Application.CommandHandlers;
using RetroBot.Application.CommandHandlers.Handlers;
using RetroBot.Application.Contracts.Services.DataStorage;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RetroBot.Application;

public sealed class BotClient : IHostedService
{
    private readonly ITelegramBotClient bot;

    private readonly IMediator mediator;
    private readonly IUserRepository userRepository;
    private readonly CommandDispatcher commandDispatcher;
    private readonly Messages messages;

    public BotClient(
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
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var botCommandHandler = new BotCommandHandler(bot, mediator, userRepository, commandDispatcher, messages);
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