using MongoDB.Driver;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Core.Entities;

namespace RetroBot.Infrastructure.DataStorageClient.Internal.Repositories;

internal sealed class AnswerRepository : IAnswerRepository
{
    private readonly Database database;

    public AnswerRepository(Database database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
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
        var teamAnswers = new List<Answer>();
        var team = await database.Teams.GetByIdAsync(teamId);

        foreach (var user in team.Users)
        {
            var userAnswers = await TryGetAnswersByUserIdAsync(user.Id);
            teamAnswers.AddRange(userAnswers);
        }

        return teamAnswers;
    }
}