namespace RetroBot.Application.Contracts.Services.Email;

public interface IEmailNotifier
{
    Task ExecuteAsync();
}