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

    public async Task<ServiceResult<IList<User>>> GetUsersAsync()
    {
        try
        {
            var users = await userRepository.GetUsersAsync();
            return ServiceResult<IList<User>>.Success(users);
        }
        catch (DbException ex)
        {
            //log
            return ServiceResult<IList<User>>.Fail<IList<User>>($"Error query users: {ex.Message}");
        }
    }

    public Task<ServiceResult<IList<Team>>> GetTeamsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<User>> GetByUserIdAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResult<Team>> GetByTeamIdAsync(Guid teamId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddUserAsync(User user)
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

    public async Task<bool> AddTeamAsync(Team team)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}