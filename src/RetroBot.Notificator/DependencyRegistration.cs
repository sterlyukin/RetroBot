using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;
using RetroBot.Application.Contracts.Services.Notification;
using RetroBot.Infrastructure.EmailClient;
using RetroBot.Notificator.Internal;

namespace RetroBot.Notificator;

public static class DependencyRegistration
{
    public static IServiceCollection AddNotificator(
        this IServiceCollection services,
        EmailOptions emailOptions)
    {
        services
            .AddSingleton(emailOptions)
            .AddSingleton<INotifier, EmailNotifier>();
        
        ConfigureEmailNotifier(services, emailOptions);

        return services;
    }
    
    private static void ConfigureEmailNotifier(this IServiceCollection services, EmailOptions emailOptions)
    {
        var client = new SmtpClient(emailOptions.Host, emailOptions.Port);
        client.Credentials = new NetworkCredential(emailOptions.FromEmail, emailOptions.EmailPassword);
        client.EnableSsl = true;

        services
            .AddFluentEmail(emailOptions.FromEmail, emailOptions.FromDisplayName)              
            .AddSmtpSender(client);
    }
}