using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.DataStorage;

public interface IUserRepository
{
    Task<User?> TryGetByIdAsync(long userId);
    Task TryAddAsync(User user);
    Task<User> TryUpdateAsync(User user);
    Task TryDeleteAsync(User user);
}