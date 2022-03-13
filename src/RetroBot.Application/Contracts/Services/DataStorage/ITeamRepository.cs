using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.DataStorage;

public interface ITeamRepository
{
    Task<IList<Team>> TryGetTeamsAsync();
    Task<Team?> TryGetByTeamIdAsync(Guid teamId);
    Task<Team?> TryGetTeamByUserIdAsync(long userId);
    Task TryAddTeamAsync(Team team);
    Task TryUpdateTeamAsync(Team team);
    Task TryAddUserToTeamAsync(Team team, User user);
}