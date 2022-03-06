using Quartz;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Quiz;
using RetroBot.Application.Report;
using RetroBot.Core;

namespace RetroBot.Application.Jobs;

[DisallowConcurrentExecution]
public class QuestionJob : IJob
{
    private readonly IStorage storage;
    private readonly QuizProcessor quizProcessor;
    private readonly ReportManager reportManager;

    public QuestionJob(IStorage storage, QuizProcessor quizProcessor, ReportManager reportManager)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.quizProcessor = quizProcessor ?? throw new ArgumentNullException(nameof(quizProcessor));
        this.reportManager = reportManager ?? throw new ArgumentNullException(nameof(reportManager));
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await SendAnswersNotificationsAsync();
        await RemoveObsoleteAnswersAsync();
        await GetUpToDateAnswersAsync();
    }

    private async Task SendAnswersNotificationsAsync()
    {
        var answers = await storage.TryGetAnswersAsync();
        if (answers.Any())
            await reportManager.ExecuteAsync();
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