using RetroBot.Core.Entities;

namespace RetroBot.Application.Contracts.Services.Notification;

public interface INotifier
{
    Task NotifyAsync(Team team, string report);
}