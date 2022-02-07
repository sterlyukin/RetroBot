using Microsoft.Extensions.DependencyInjection;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Infrastructure.StorageClient;
using RetroBot.Infrastructure.StorageClient.Repositories;

namespace RetroBot.Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        DatabaseOptions databaseOptions)
    {
        services
            .AddSingleton(databaseOptions)
            .AddDbContext<RetroBotDbContext>()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<ITeamRepository, TeamRepository>()
            .AddSingleton<IQuestionRepository, QuestionRepository>()
            .AddSingleton<IStorage, DatabaseStorage>();

        return services;
    }
}