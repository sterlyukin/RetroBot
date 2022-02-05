using RetroBot.Core;

namespace RetroBot.Application.Contracts.Services.Storage;

public interface ITeamRepository
{
    Task<IList<Team>> GetTeamsAsync();
    Task<Team?> GetTeamByIdAsync(Guid teamId);
    Task AddTeamAsync(Team team);
    Task AddUserToTeam(Guid teamId, User user);
}