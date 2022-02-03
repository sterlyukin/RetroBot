using Microsoft.EntityFrameworkCore;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly RetroBotDbContext dbContext;

    public UserRepository(RetroBotDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IList<User>> GetUsersAsync()
    {
        return await dbContext.Users.ToListAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }
}