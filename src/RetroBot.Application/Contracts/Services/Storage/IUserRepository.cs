using RetroBot.Core;

namespace RetroBot.Application.Contracts.Services.Storage;

public interface IUserRepository
{
    Task<IList<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(long userId);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
}