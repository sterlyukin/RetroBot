using RetroBot.Core;
using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.Storage;

public interface IStorage
{
    Task<IList<User>> TryGetUsersAsync();
    Task<IList<Team>> TryGetTeamsAsync();
    Task<User?> TryGetByUserIdAsync(long userId);
    Task<Team?> TryGetByTeamIdAsync(Guid teamId);
    Task<Team?> TryGetTeamByUserIdAsync(long userId);
    Task TryAddUserAsync(User user);
    Task TryAddTeamAsync(Team team);
    Task TryUpdateTeamAsync(Team team);
    Task<User> TryUpdateUserAsync(User user);
    Task TryAddUserToTeamAsync(Team team, User user);
    Task<IList<Question>> TryGetQuestionsAsync();
    Task<IList<Answer>> TryGetAnswersByUserIdAsync(long userId);
    Task TryAddAnswerAsync(Answer answer);
    Task TryUpdateAnswerAsync(Answer answer);
    Task TryDeleteAnswersAsync();
    Task<IList<Answer>> TryGetAnswersAsync();
    Task<IList<Answer>> TryGetAnswersByTeamIdAsync(Guid teamId);
}