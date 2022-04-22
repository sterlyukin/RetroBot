using MongoDB.Driver;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Core.Entities;

namespace RetroBot.DataStorage.Internal.Repositories;

internal sealed class AnswerRepository : IAnswerRepository
{
    private readonly Database database;

    public AnswerRepository(Database database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<IList<Answer>> GetByFilterAsync(long userId)
    {
        return await database.Answers.Find(answer => answer.UserId == userId).ToListAsync();
    }

    public async Task AddAsync(Answer answer)
    {
        await database.Answers.InsertOneAsync(answer);
    }

    public async Task UpdateAsync(Answer answer)
    {
        await database.Answers.UpdateByGeneratedIdAsync(answer);
    }

    public async Task DeleteAsync()
    {
        await database.Answers.DeleteAllAsync();
    }

    public async Task<IList<Answer>> GetAllAsync()
    {
        return await database.Answers.GetAllAsync();
    }

    public async Task<IList<Answer>> GetByFilterAsync(Guid teamId)
    {
        var teamAnswers = new List<Answer>();
        var team = await database.Teams.GetByIdAsync(teamId);

        foreach (var user in team.Users)
        {
            var userAnswers = await GetByFilterAsync(user.Id);
            teamAnswers.AddRange(userAnswers);
        }

        return teamAnswers;
    }
}