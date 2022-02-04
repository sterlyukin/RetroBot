using System.Data.Common;
using RetroBot.Application.Contracts.Services;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient;

public class DatabaseStorage : IStorage
{
    private readonly IUserRepository userRepository;

    public DatabaseStorage(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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

    public Task<ServiceResult<IList<Team>>> TryGetTeamsAsync()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public async Task<bool> TryAddUserAsync(User user)
    {
        try
        {
            await userRepository.AddUserAsync(user);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> TryAddTeamAsync(Team team)
    {
        throw new NotImplementedException();
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
}