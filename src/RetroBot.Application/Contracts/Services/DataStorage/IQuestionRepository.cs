using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.DataStorage;

public interface IQuestionRepository
{
    Task<IList<Question>> TryGetQuestionsAsync();
}