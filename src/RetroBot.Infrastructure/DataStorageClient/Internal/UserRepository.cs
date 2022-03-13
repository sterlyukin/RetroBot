using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Core.Entities;

namespace RetroBot.Infrastructure.DataStorageClient.Internal;

internal class UserRepository : IUserRepository
{
    private readonly Database database;

    public UserRepository(Database database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }
    
    public async Task<User?> TryGetByUserIdAsync(long userId)
    {
        return await database.Users.GetByIdAsync(userId);
    }

    public async Task TryAddUserAsync(User user)
    {
        await database.Users.InsertOneAsync(user);
    }

    public async Task<User> TryUpdateUserAsync(User user)
    {
        await database.Users.UpdateByIssuedIdAsync(user);
        return await database.Users.GetByIdAsync(user.Id);
    }
}