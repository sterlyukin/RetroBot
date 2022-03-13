using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.DataStorage;

public interface IAnswerRepository
{
    Task<IList<Answer>> TryGetAnswersByUserIdAsync(long userId);
    Task TryAddAnswerAsync(Answer answer);
    Task TryUpdateAnswerAsync(Answer answer);
    Task TryDeleteAnswersAsync();
    Task<IList<Answer>> TryGetAnswersAsync();
    Task<IList<Answer>> TryGetAnswersByTeamIdAsync(Guid teamId);
}