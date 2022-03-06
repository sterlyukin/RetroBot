using MongoDB.Driver;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;
using RetroBot.Core.Entities;

namespace RetroBot.Infrastructure.StorageClient;

internal class DatabaseStorage : IStorage
{
    private readonly Database database;

    public DatabaseStorage(Database database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }
    
    public async Task<IList<User>> TryGetUsersAsync()
    {
        return await database.Users.GetAllAsync();
    }

    public async Task<IList<Team>> TryGetTeamsAsync()
    {
        return await database.Teams.GetAllAsync();
    }

    public async Task<User?> TryGetByUserIdAsync(long userId)
    {
        return await database.Users.GetByIdAsync(userId);
    }

    public async Task<Team?> TryGetByTeamIdAsync(Guid teamId)
    {
        return await database.Teams.GetByIdAsync(teamId);
    }

    public async Task TryAddUserAsync(User user)
    {
        await database.Users.InsertOneAsync(user);
    }

    public async Task TryAddTeamAsync(Team team)
    {
        await database.Teams.InsertOneAsync(team);
    }

    public async Task<User> TryUpdateUserAsync(User user)
    {
        await database.Users.UpdateByIssuedIdAsync(user);
        return await database.Users.GetByIdAsync(user.Id);
    }

    public async Task TryAddUserToTeamAsync(Team team, User user)
    {
        team.Users.Add(user);
        await database.Teams.UpdateByGeneratedIdAsync(team);
    }

    public async Task<IList<Question>> TryGetQuestionsAsync()
    {
        return await database.Questions.GetAllAsync();
    }

    public async Task<IList<Answer>> TryGetAnswersByUserIdAsync(long userId)
    {
        return await database.Answers.Find(answer => answer.UserId == userId).ToListAsync();
    }

    public async Task TryAddAnswerAsync(Answer answer)
    {
        await database.Answers.InsertOneAsync(answer);
    }

    public async Task TryUpdateAnswerAsync(Answer answer)
    {
        await database.Answers.UpdateByGeneratedIdAsync(answer);
    }

    public async Task TryDeleteAnswersAsync()
    {
        await database.Answers.DeleteAllAsync();
    }

    public async Task<IList<Answer>> TryGetAnswersAsync()
    {
        return await database.Answers.GetAllAsync();
    }

    public async Task<IList<Answer>> TryGetAnswersByTeamIdAsync(Guid teamId)
    {
        var team = await database.Teams.GetByIdAsync(teamId);
        var teamAnswers = new List<Answer>();

        foreach (var user in team.Users)
        {
            var userAnswers = await TryGetAnswersByUserIdAsync(user.Id);
            teamAnswers.AddRange(userAnswers);
        }

        return teamAnswers;
    }
}