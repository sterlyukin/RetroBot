using Microsoft.EntityFrameworkCore;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient.Repositories;

internal sealed class TeamRepository : ITeamRepository
{
    private readonly RetroBotDbContext dbContext;

    public TeamRepository(RetroBotDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<IList<Team>> GetTeamsAsync()
    {
        return await dbContext.Teams.ToListAsync();
    }

    public async Task<Team?> GetTeamByIdAsync(Guid teamId)
    {
        var teams = await dbContext.Teams.ToListAsync();
        return teams.FirstOrDefault(team => team.Id == teamId);
    }

    public async Task AddTeamAsync(Team team)
    {
        await dbContext.Teams.AddAsync(team);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddUserToTeam(Guid teamId, User user)
    {
        await dbContext.Teams.ForEachAsync(team =>
        {
            if (team.Id == teamId)
            {
                team.Users.Add(user);
            }
        });
    }
}