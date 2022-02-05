using RetroBot.Core;

namespace RetroBot.Application.Contracts.Services.Storage;

public interface IStorage
{
    Task<ServiceResult<IList<User>>> TryGetUsersAsync();
    Task<ServiceResult<IList<Team>>> TryGetTeamsAsync();
    Task<ServiceResult<User>> TryGetByUserIdAsync(long userId);
    Task<ServiceResult<Team>> TryGetByTeamIdAsync(Guid teamId);
    Task<bool> TryAddUserAsync(User user);
    Task<bool> TryAddTeamAsync(Team team);
    Task<bool> TryUpdateUserAsync(User user);
    Task<bool> TryAddUserToTeam(Team team, User user);
}