using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Infrastructure.StorageClient;
using RetroBot.Infrastructure.StorageClient.Repositories;

namespace RetroBot.Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddDbContext<RetroBotDbContext>()
            .AddSingleton<IStorage, DatabaseStorage>()
            .AddSingleton<IUserRepository, UserRepository>();

        return services;
    }
}