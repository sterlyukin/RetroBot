using RetroBot.Core;

namespace RetroBot.Application.Contracts.Services.Storage;

public interface IUserRepository
{
    Task<IList<User>> GetUsersAsync();
    Task AddUserAsync(User user);
}