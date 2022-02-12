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
        return await databaseClient.Users.Find(_ => true).ToListAsync();
    }

    public async Task<IList<Team>> TryGetTeamsAsync()
    {
        return await databaseClient.Teams.Find(_ => true).ToListAsync();
    }

    public async Task<User?> TryGetByUserIdAsync(long userId)
    {
        return await databaseClient.Users.Find(user => user.Id == userId).FirstOrDefaultAsync();
    }

    public async Task<Team?> TryGetByTeamIdAsync(Guid teamId)
    {
        return await databaseClient.Teams.Find(team => team.Id == teamId).FirstOrDefaultAsync();
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
        var filter = Builders<User>.Filter.Eq(currentUser => currentUser.Id, user.Id);
        await databaseClient.Users.ReplaceOneAsync(filter, user);
        
        return await databaseClient.Users.Find(currentUser => currentUser.Id == user.Id).FirstOrDefaultAsync();
    }

    public async Task TryAddUserToTeam(Team team, User user)
    {
        team.Users.Add(user);

        var filter = Builders<Team>.Filter.Eq(currentTeam => currentTeam.Id, team.Id);
        await databaseClient.Teams.ReplaceOneAsync(filter, team);
    }

    public async Task<IList<Question>> TryGetQuestionsAsync()
    {
        return await databaseClient.Questions.Find(_ => true).ToListAsync();
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
        var filter = Builders<Answer>.Filter.Eq(currentAnswer => currentAnswer.Id, answer.Id);
        await databaseClient.Answers.ReplaceOneAsync(filter, answer);
    }
}