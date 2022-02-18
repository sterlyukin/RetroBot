using Quartz;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.QuizProcessors;
using RetroBot.Core;

namespace RetroBot.Application.Jobs;

[DisallowConcurrentExecution]
public class QuestionJob : IJob
{
    private readonly IStorage storage;
    private readonly IQuizProcessor quizProcessor;

    public QuestionJob(IStorage storage, IQuizProcessor quizProcessor)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.quizProcessor = quizProcessor ?? throw new ArgumentNullException(nameof(quizProcessor));
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await RemoveObsoleteAnswers();
        await GetUpToDateAnswers();
    }

    private async Task RemoveObsoleteAnswers()
    {
        await storage.TryDeleteAnswersAsync();
    }

    private async Task GetUpToDateAnswers()
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