using System.Data.Common;
using RetroBot.Application.Contracts.Services;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient;

public class DatabaseStorage : IStorage
{
    private readonly IUserRepository userRepository;
    private readonly ITeamRepository teamRepository;

    public DatabaseStorage(IUserRepository userRepository, ITeamRepository teamRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
    }

    public async Task<ServiceResult<IList<User>>> TryGetUsersAsync()
    {
        try
        {
            var users = await userRepository.GetUsersAsync();
            return ServiceResult<IList<User>>.Success(users);
        }
        catch (DbException ex)
        {
            return ServiceResult<IList<User>>.Fail<IList<User>>($"Error getting users: {ex.Message}");
        }
    }

    public async Task<ServiceResult<IList<Team>>> TryGetTeamsAsync()
    {
        try
        {
            var teams = await teamRepository.GetTeamsAsync();
            return ServiceResult<IList<Team>>.Success(teams);
        }
        catch (DbException ex)
        {
            return ServiceResult<IList<Team>>.Fail<IList<Team>>($"Error getting teams: {ex.Message}");
        }
    }

    public async Task<ServiceResult<User>> TryGetByUserIdAsync(long userId)
    {
        try
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            return user is not null
                ? ServiceResult<User>.Success(user)
                : ServiceResult<User>.Fail<User>($"User with id = {userId} wasn't found");
        }
        catch (DbException ex)
        {
            return ServiceResult<User>.Fail<User>($"Error getting user: {ex.Message}");
        }
    }

    public async Task<ServiceResult<Team>> TryGetByTeamIdAsync(Guid teamId)
    {
        try
        {
            var team = await teamRepository.GetTeamByIdAsync(teamId);
            return team is not null
                ? ServiceResult<Team>.Success(team)
                : ServiceResult<Team>.Fail<Team>($"Team with id = {teamId} wasn't found");
        }
        catch (DbException ex)
        {
            return ServiceResult<Team>.Fail<Team>($"Error getting team: {ex.Message}");
        }
    }

    public async Task<bool> TryAddUserAsync(User user)
    {
        try
        {
            await userRepository.AddUserAsync(user);
            return true;
        }
        catch (DbException ex)
        {
            return false;
        }
    }

    public async Task<bool> TryAddTeamAsync(Team team)
    {
        try
        {
            await teamRepository.AddTeamAsync(team);
            return true;
        }
        catch (DbException ex)
        {
            return false;
        }
    }

    public async Task<bool> TryUpdateUserAsync(User user)
    {
        try
        {
            await userRepository.UpdateUserAsync(user);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> TryAddUserToTeam(Team team, User user)
    {
        try
        {
            await teamRepository.AddUserToTeam(team.Id, user);
            return true;
        }
        catch (DbException ex)
        {
            return false;
        }
    }
}