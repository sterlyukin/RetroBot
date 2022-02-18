﻿using RetroBot.Core;

namespace RetroBot.Application.Contracts.Services.Storage;

public interface IStorage
{
    Task<IList<User>> TryGetUsersAsync();
    Task<IList<Team>> TryGetTeamsAsync();
    Task<User?> TryGetByUserIdAsync(long userId);
    Task<Team?> TryGetByTeamIdAsync(Guid teamId);
    Task TryAddUserAsync(User user);
    Task TryAddTeamAsync(Team team);
    Task<User> TryUpdateUserAsync(User user);
    Task TryAddUserToTeam(Team team, User user);
    Task<IList<Question>> TryGetQuestionsAsync();
    Task<IList<Answer>> TryGetAnswersByUserId(long userId);
    Task TryAddAnswerAsync(Answer answer);
    Task TryUpdateAnswerAsync(Answer answer);
    Task TryDeleteAnswersAsync();
}