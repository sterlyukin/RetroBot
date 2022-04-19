using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.DataStorage;

public interface ITeamRepository
{
    Task<IList<Team>> GetAllAsync();
    Task<Team?> FindAsync(Guid teamId);
    Task<Team?> FindAsync(long userId);
    Task AddAsync(Team team);
    Task UpdateAsync(Team team);
    Task AddUserToTeamAsync(Team team, User user);
    Task DeleteAsync(Team team);
}