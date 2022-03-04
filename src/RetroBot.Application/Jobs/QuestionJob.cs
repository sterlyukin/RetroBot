using Quartz;
using RetroBot.Application.Contracts.Services.Email;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.QuizProcessors;
using RetroBot.Core;

namespace RetroBot.Application.Jobs;

[DisallowConcurrentExecution]
public class QuestionJob : IJob
{
    private readonly IStorage storage;
    private readonly IQuizProcessor quizProcessor;
    private readonly IEmailNotifier emailNotifier;

    public QuestionJob(IStorage storage, IQuizProcessor quizProcessor, IEmailNotifier emailNotifier)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.quizProcessor = quizProcessor ?? throw new ArgumentNullException(nameof(quizProcessor));
        this.emailNotifier = emailNotifier ?? throw new ArgumentNullException(nameof(emailNotifier));
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await SendAnswersAsync();
        await RemoveObsoleteAnswersAsync();
        await GetUpToDateAnswersAsync();
    }

    private async Task SendAnswersAsync()
    {
        await emailNotifier.ExecuteAsync();
    }

    private async Task RemoveObsoleteAnswersAsync()
    {
        await storage.TryDeleteAnswersAsync();
    }

    private async Task GetUpToDateAnswersAsync()
    {
        var teams = await storage.TryGetTeamsAsync();
        foreach (var team in teams)
        {
            foreach (var user in team.Users)
            {
                if (user.State == UserState.Completed)
                {
                    await quizProcessor.ExecuteAsync(user.Id, string.Empty);
                }
            }            
        }
    }
}