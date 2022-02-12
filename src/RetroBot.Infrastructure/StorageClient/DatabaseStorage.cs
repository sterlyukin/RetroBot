using MongoDB.Driver;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient;

public class DatabaseStorage : IStorage
{
    private readonly DatabaseClient databaseClient;

    public DatabaseStorage(DatabaseClient databaseClient)
    {
        this.databaseClient = databaseClient ?? throw new ArgumentNullException(nameof(databaseClient));
    }
    
    public async Task<IList<User>> TryGetUsersAsync()
    {
        return await databaseClient.Users.GetAllAsync();
    }

    public async Task<IList<Team>> TryGetTeamsAsync()
    {
        return await databaseClient.Teams.GetAllAsync();
    }

    public async Task<User?> TryGetByUserIdAsync(long userId)
    {
        return await databaseClient.Users.GetByIdAsync(userId);
    }

    public async Task<Team?> TryGetByTeamIdAsync(Guid teamId)
    {
        return await databaseClient.Teams.GetByIdAsync(teamId);
    }

    public async Task TryAddUserAsync(User user)
    {
        await databaseClient.Users.InsertOneAsync(user);
    }

    public async Task TryAddTeamAsync(Team team)
    {
        await databaseClient.Teams.InsertOneAsync(team);
    }

    public async Task<User> TryUpdateUserAsync(User user)
    {
        await databaseClient.Users.UpdateByIssuedIdAsync(user);
        return await databaseClient.Users.GetByIdAsync(user.Id);
    }

    public async Task TryAddUserToTeam(Team team, User user)
    {
        team.Users.Add(user);
        await databaseClient.Teams.UpdateByGeneratedIdAsync(team);
    }

    public async Task<IList<Question>> TryGetQuestionsAsync()
    {
        return await databaseClient.Questions.GetAllAsync();
    }

    public async Task<IList<Answer>> TryGetAnswersByUserId(long userId)
    {
        return await databaseClient.Answers.Find(answer => answer.UserId == userId).ToListAsync();
    }

    public async Task TryAddAnswerAsync(Answer answer)
    {
        await databaseClient.Answers.InsertOneAsync(answer);
    }

    public async Task TryUpdateAnswerAsync(Answer answer)
    {
        await databaseClient.Answers.UpdateByGeneratedIdAsync(answer);
    }
}