using Quartz;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.Quiz;
using RetroBot.Application.Report;
using RetroBot.Core;

namespace RetroBot.Application.Jobs;

[DisallowConcurrentExecution]
public class QuestionJob : IJob
{
    private readonly IStorageClient storageClient;
    private readonly QuizProcessor quizProcessor;
    private readonly ReportManager reportManager;

    public QuestionJob(IStorageClient storageClient, QuizProcessor quizProcessor, ReportManager reportManager)
    {
        this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
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
        var answers = await storageClient.TryGetAnswersAsync();
        if (answers.Any())
            await reportManager.ExecuteAsync();
    }

    private async Task RemoveObsoleteAnswersAsync()
    {
        await storageClient.TryDeleteAnswersAsync();
    }

    private async Task GetUpToDateAnswersAsync()
    {
        var teams = await storageClient.TryGetTeamsAsync();
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