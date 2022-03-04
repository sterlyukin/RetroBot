﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetroBot.Application;
using RetroBot.Infrastructure;
using RetroBot.Infrastructure.EmailClient;
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

        var emailOptions = configuration
            .GetSection(nameof(EmailOptions))
            .Get<EmailOptions>();
        
        var messages = configuration
            .GetSection(nameof(Messages))
            .Get<Messages>();

        services
            .AddApplication(telegramClientOptions, messages)
            .AddInfrastructure(databaseOptions, emailOptions);
    }
}