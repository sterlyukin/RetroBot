using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Application.CommandHandlers.Handlers;
using RetroBot.Application.Jobs;
using RetroBot.Application.Quiz;
using RetroBot.Application.Report;
using RetroBot.Application.Validators;
using Telegram.Bot;

namespace RetroBot.Application;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        TelegramClientOptions telegramClientOptions,
        Messages messages)
    {
        if (telegramClientOptions is null)
            throw new ArgumentNullException(nameof(telegramClientOptions));

        var bot = new TelegramBotClient(telegramClientOptions.ApiKey);
        
        return services
            .AddSingleton<ITelegramBotClient>(bot)
            .AddSingleton(telegramClientOptions)
            .AddSingleton(messages)
            .AddSingleton<CommandDispatcher>()
            .AddSingleton<ReportBuilder>()
            .AddSingleton<ReportManager>()
            .AddSingleton<QuizProcessor>()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .ConfigureHandlers()
            .ConfigureJobs();
    }
    
    private static IServiceCollection ConfigureHandlers(this IServiceCollection services)
    {
        return services
            .AddSingleton<StandardCommandValidator>()
                
            .AddScoped<IRequestHandler<CreateTeamCommand, string>, CreateTeamCommandHandler>()

            .AddScoped<IRequestHandler<InputTeamIdCommand, string>, InputTeamIdCommandHandler>()

            .AddSingleton<InputTeamleadEmailCommandValidator>()
            .AddScoped<IRequestHandler<InputTeamleadEmailCommand, string>, InputTeamleadEmailCommandHandler>()

            .AddScoped<IRequestHandler<InputTeamNameCommand, string>, InputTeamNameCommandHandler>()

            .AddScoped<IRequestHandler<JoinTeamCommand, string>, JoinTeamCommandHandler>()

            .AddScoped<IRequestHandler<StartCommand, string>, StartCommandHandler>();
    }

    private static IServiceCollection ConfigureJobs(this IServiceCollection services)
    {
        return services
            .ConfigureQuestionJob()
            .ConfigureNotifyJob();
    }
    
    private static IServiceCollection ConfigureQuestionJob(this IServiceCollection services)
    {
        return services
            .AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                
                var jobKey = new JobKey(nameof(QuestionJob));
                q.AddJob<QuestionJob>(o => o.WithIdentity(jobKey));
                q.AddTrigger(o => o
                    .ForJob(jobKey)
                    .WithIdentity($"{nameof(QuestionJob)}-trigger")
                    .WithCronSchedule("0 0/5 * * * ?"));
            })
            .AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
    
    private static IServiceCollection ConfigureNotifyJob(this IServiceCollection services)
    {
        return services
            .AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                
                var jobKey = new JobKey(nameof(NotifyJob));
                q.AddJob<NotifyJob>(o => o.WithIdentity(jobKey));
                q.AddTrigger(o => o
                    .ForJob(jobKey)
                    .WithIdentity($"{nameof(NotifyJob)}-trigger")
                    .WithCronSchedule("0 0/6 * * * ?"));
            })
            .AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}