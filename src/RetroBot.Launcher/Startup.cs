using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetroBot.Application;
using RetroBot.Infrastructure;
using RetroBot.Infrastructure.StorageClient;
using RetroBot.Launcher.Infrastructure;

namespace RetroBot.Launcher;

public class Startup
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

        services
            .AddApplication(telegramClientOptions)
            .AddInfrastructure(databaseOptions);
    }
}