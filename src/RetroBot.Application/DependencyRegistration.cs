using Microsoft.Extensions.DependencyInjection;

namespace RetroBot.Application;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        TelegramClientOptions telegramClientOptions,
        Messages messages)
    {
        if (telegramClientOptions is null)
            throw new ArgumentNullException(nameof(telegramClientOptions));

        services
            .AddSingleton(telegramClientOptions)
            .AddSingleton(messages);

        return services;
    }
}