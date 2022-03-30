using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.DataStorage;

public interface ITeamRepository
{
    Task<IList<Team>> TryGetAsync();
    Task<Team?> TryGetByIdAsync(Guid teamId);
    Task<Team?> TryGetByUserIdAsync(long userId);
    Task TryAddAsync(Team team);
    Task TryUpdateAsync(Team team);
    Task TryAddUserToTeamAsync(Team team, User user);
    Task TryDeleteAsync(Team team);
}