using RetroBot.Core;
using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.Notify;

public interface INotifier
{
    Task NotifyAsync(Team team, string report);
}