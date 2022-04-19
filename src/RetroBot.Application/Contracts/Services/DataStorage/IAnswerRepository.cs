using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.DataStorage;

public interface IAnswerRepository
{
    Task<IList<Answer>> GetByFilterAsync(long userId);
    Task AddAsync(Answer answer);
    Task UpdateAsync(Answer answer);
    Task DeleteAsync();
    Task<IList<Answer>> GetAllAsync();
    Task<IList<Answer>> GetByFilterAsync(Guid teamId);
}