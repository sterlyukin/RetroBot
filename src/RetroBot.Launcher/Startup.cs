using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetroBot.Application;
using RetroBot.DataStorage;
using RetroBot.Infrastructure.EmailClient;
using RetroBot.Launcher.Infrastructure;
using RetroBot.Notificator;

namespace RetroBot.Launcher;

public sealed class Startup
{
    private readonly IConfiguration configuration = ConfigBuilder.Build();

    public void ConfigureServices(IServiceCollection services)
    {
        var telegramClientOptions = configuration
            .GetSection(nameof(TelegramClientOptions))
            .Get<TelegramClientOptions>();

        var databaseOptions = configuration
            .GetSection(nameof(DatabaseOptions))
            .Get<DatabaseOptions>();

        var emailOptions = configuration
            .GetSection(nameof(EmailOptions))
            .Get<EmailOptions>();
        
        var messages = configuration
            .GetSection(nameof(Messages))
            .Get<Messages>();

        services
            .AddApplication(telegramClientOptions, messages)
            .AddDatabase(databaseOptions)
            .AddNotificator(emailOptions);
    }
}