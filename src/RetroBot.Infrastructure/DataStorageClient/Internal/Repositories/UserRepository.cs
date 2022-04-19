using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Core.Entities;

namespace RetroBot.Infrastructure.DataStorageClient.Internal.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly Database database;

    public UserRepository(Database database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }
    
    public async Task<User?> FindAsync(long userId)
    {
        return await database.Users.GetByIdAsync(userId);
    }

    public async Task AddAsync(User user)
    {
        await database.Users.InsertOneAsync(user);
    }

    public async Task<User> UpdateAsync(User user)
    {
        await database.Users.UpdateByIssuedIdAsync(user);
        return await database.Users.GetByIdAsync(user.Id);
    }

    public async Task DeleteAsync(User user)
    {
        await database.Users.DeleteById(user.Id);
    }
}