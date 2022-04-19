using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.DataStorage;

public interface IUserRepository
{
    Task<User?> FindAsync(long userId);
    Task AddAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(User user);
}