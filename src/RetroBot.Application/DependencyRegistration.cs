using Microsoft.Extensions.DependencyInjection;
using Quartz;
using RetroBot.Application.Jobs;
using RetroBot.Application.QuizProcessors;
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
        
        services
            .AddSingleton<ITelegramBotClient>(bot)
            .AddSingleton(telegramClientOptions)
            .AddSingleton(messages)
            .AddTransient<IQuizProcessor, QuizProcessor>()
            .ConfigureJobs();

        return services;
    }

    private static IServiceCollection ConfigureJobs(this IServiceCollection services)
    {
        services
            .AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                
                var jobKey = new JobKey("QuestionJob");
                q.AddJob<QuestionJob>(o => o.WithIdentity(jobKey));
                q.AddTrigger(o => o
                    .ForJob(jobKey)
                    .WithIdentity("QuestionJob-trigger")
                    .WithCronSchedule("0 0/5 * * * ?"));
            });
        
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}