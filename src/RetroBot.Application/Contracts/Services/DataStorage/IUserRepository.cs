using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.DataStorage;

public interface IUserRepository
{
    Task<User?> TryGetByUserIdAsync(long userId);
    Task TryAddUserAsync(User user);
    Task<User> TryUpdateUserAsync(User user);
}