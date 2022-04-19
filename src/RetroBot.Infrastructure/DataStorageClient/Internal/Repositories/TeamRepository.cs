using MongoDB.Driver;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Core.Entities;

namespace RetroBot.Infrastructure.DataStorageClient.Internal.Repositories;

internal sealed class TeamRepository : ITeamRepository
{
    private readonly Database database;

    public TeamRepository(Database database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<IList<Team>> GetAllAsync()
    {
        return await database.Teams.GetAllAsync();
    }

    public async Task<Team?> FindAsync(Guid teamId)
    {
        return await database.Teams.GetByIdAsync(teamId);
    }

    public async Task<Team?> FindAsync(long userId)
    {
        return await database.Teams.Find(team => team.Users.Any(user => user.Id == userId)).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Team team)
    {
        await database.Teams.InsertOneAsync(team);
    }

    public async Task UpdateAsync(Team team)
    {
        await database.Teams.UpdateByGeneratedIdAsync(team);
    }

    public async Task AddUserToTeamAsync(Team team, User user)
    {
        team.Users.Add(user);
        await database.Teams.UpdateByGeneratedIdAsync(team);
    }

    public async Task DeleteAsync(Team team)
    {
        await database.Teams.DeleteById(team.Id);
    }
}