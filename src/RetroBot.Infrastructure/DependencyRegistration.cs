using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Infrastructure.StorageClient;
using RetroBot.Infrastructure.StorageClient.Mappers;

namespace RetroBot.Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        DatabaseOptions databaseOptions)
    {
        ConfigureBson();
        
        services
            .AddSingleton(databaseOptions)
            .AddSingleton<IMongoClient>(new MongoClient(databaseOptions.ConnectionString))
            .AddSingleton(MongoFactory(databaseOptions))
            .AddSingleton<IStorage, DatabaseStorage>();

        return services;
    }
    
    private static Func<IServiceProvider, DatabaseClient> MongoFactory(DatabaseOptions options)
    {
        return sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return new DatabaseClient(client, options);
        };
    }

    private static void ConfigureBson()
    {
        BsonClassMap.RegisterClassMap(new UserMapper());
        BsonClassMap.RegisterClassMap(new TeamMapper());
    }
}