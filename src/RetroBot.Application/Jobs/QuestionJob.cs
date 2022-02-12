using Quartz;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.QuizProcessors.Interfaces;
using RetroBot.Core;
using Telegram.Bot;

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
        var teams = await storage.TryGetTeamsAsync();
        foreach (var team in teams)
        {
            foreach (var user in team.Users)
            {
                if (user.State == UserState.Completed)
                {
                    await quizProcessor.Execute(user.Id, string.Empty);
                }
            }            
        }
        
    }
}