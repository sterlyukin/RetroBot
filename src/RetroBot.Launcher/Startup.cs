using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetroBot.Application;
using RetroBot.Infrastructure;
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

        services
            .AddApplication(telegramClientOptions)
            .AddInfrastructure();
    }
}