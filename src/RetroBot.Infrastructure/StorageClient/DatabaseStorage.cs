using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient;

public class DatabaseStorage : IStorage
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;
    private readonly IQuestionRepository questionRepository;

    public DatabaseStorage(IUserRepository userRepository, ITeamRepository teamRepository, IQuestionRepository questionRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
    }

    public async Task<IList<User>> TryGetUsersAsync()
    {
        return await userRepository.GetUsersAsync();
    }

    public async Task<IList<Team>> TryGetTeamsAsync()
    {
        return await teamRepository.GetTeamsAsync();
    }

    public async Task<User?> TryGetByUserIdAsync(long userId)
    {
        return await userRepository.GetUserByIdAsync(userId);
    }

    public async Task<Team?> TryGetByTeamIdAsync(Guid teamId)
    {
        return await teamRepository.GetTeamByIdAsync(teamId);
    }

    public async Task TryAddUserAsync(User user)
    {
        await userRepository.AddUserAsync(user);
    }

    public async Task TryAddTeamAsync(Team team)
    {
        await teamRepository.AddTeamAsync(team);
    }

    public async Task TryUpdateUserAsync(User user)
    {
        await userRepository.UpdateUserAsync(user);
    }

    public async Task TryAddUserToTeam(Team team, User user)
    {
        await teamRepository.AddUserToTeam(team.Id, user);
    }

    public async Task<IList<Question>> TryGetQuestionsAsync()
    {
        return await questionRepository.GetQuestionsAsync();
    }
}