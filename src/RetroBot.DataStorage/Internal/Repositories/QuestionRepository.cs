using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Core.Entities;

namespace RetroBot.DataStorage.Internal.Repositories;

internal sealed class QuestionRepository : IQuestionRepository
{
    private readonly Database database;

    public QuestionRepository(Database database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
    }
    
    public async Task<IList<Question>> GetAllAsync()
    {
        return await database.Questions.GetAllAsync();
    }
}