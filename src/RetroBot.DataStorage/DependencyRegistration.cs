using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.DataStorage.Internal;
using RetroBot.DataStorage.Internal.Mappers;
using RetroBot.DataStorage.Internal.Repositories;

namespace RetroBot.DataStorage;

public static class DependencyRegistration
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        DatabaseOptions databaseOptions)
    {
        ConfigureBson();

        services
            .AddSingleton(databaseOptions)
            .AddSingleton<IMongoClient>(new MongoClient(databaseOptions.ConnectionString))
            .AddSingleton(MongoFactory(databaseOptions))
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<ITeamRepository, TeamRepository>()
            .AddSingleton<IQuestionRepository, QuestionRepository>()
            .AddSingleton<IAnswerRepository, AnswerRepository>();
        
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
}