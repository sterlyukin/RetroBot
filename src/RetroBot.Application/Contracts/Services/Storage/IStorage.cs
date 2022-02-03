using RetroBot.Core;

namespace RetroBot.Application.Contracts.Services.Storage;

public interface IStorage
{
    Task<ServiceResult<IList<User>>> GetUsersAsync();
    Task<ServiceResult<IList<Team>>> GetTeamsAsync();
    Task<ServiceResult<User>> GetByUserIdAsync(long userId);
    Task<ServiceResult<Team>> GetByTeamIdAsync(Guid teamId);
    Task<bool> AddUserAsync(User user);
    Task<bool> AddTeamAsync(Team team);
    Task<bool> UpdateUserAsync(User user);
}