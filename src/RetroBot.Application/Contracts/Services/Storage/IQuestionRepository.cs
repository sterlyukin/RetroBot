using RetroBot.Core;

namespace RetroBot.Application.Contracts.Services.Storage;

public interface IQuestionRepository
{
    Task<IList<Question>> GetQuestionsAsync();
    Task<Question?> GetQuestionByIdAsync(Guid questionId);
}