using Microsoft.EntityFrameworkCore;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient.Repositories;

internal sealed class QuestionRepository : IQuestionRepository
{
    private readonly RetroBotDbContext dbContext;

    public QuestionRepository(RetroBotDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<IList<Question>> GetQuestionsAsync()
    {
        return await dbContext.Questions.ToListAsync();
    }

    public async Task<Question?> GetQuestionByIdAsync(Guid questionId)
    {
        var questions = await dbContext.Questions.ToListAsync();
        return questions.FirstOrDefault(question => question.Id == questionId);
    }
}