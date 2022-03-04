using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RetroBot.Application.Contracts.Services.Email;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Infrastructure.EmailClient;
using RetroBot.Infrastructure.StorageClient;
using RetroBot.Infrastructure.StorageClient.Mappers;

namespace RetroBot.Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        DatabaseOptions databaseOptions,
        EmailOptions emailOptions)
    {
        ConfigureBson();

        services
            .AddSingleton(databaseOptions)
            .AddSingleton(emailOptions)
            .AddSingleton<IMongoClient>(new MongoClient(databaseOptions.ConnectionString))
            .AddSingleton(MongoFactory(databaseOptions))
            .AddSingleton<IStorage, DatabaseStorage>()
            .AddSingleton<IEmailNotifier, EmailNotifier>();
        
        ConfigureEmailNotifier(services, emailOptions);

        return services;
    }
    
    private static Func<IServiceProvider, Database> MongoFactory(DatabaseOptions options)
    {
        return sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return new Database(client, options);
        };
    }

    private static void ConfigureBson()
    {
        BsonClassMap.RegisterClassMap(new UserMapper());
        BsonClassMap.RegisterClassMap(new TeamMapper());
        BsonClassMap.RegisterClassMap(new AnswerMapper());
        BsonClassMap.RegisterClassMap(new QuestionMapper());
    }

    private static void ConfigureEmailNotifier(this IServiceCollection services, EmailOptions emailOptions)
    {
        var client = new SmtpClient(emailOptions.Host, emailOptions.Port);
        client.Credentials = new NetworkCredential(emailOptions.FromEmail, emailOptions.EmailPassword);
        client.EnableSsl = true;

        services
            .AddFluentEmail(emailOptions.FromEmail, emailOptions.FromDisplayName)              
            .AddSmtpSender(client);
    }
}