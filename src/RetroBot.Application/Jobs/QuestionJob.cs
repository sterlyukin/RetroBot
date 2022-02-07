using Quartz;
using RetroBot.Application.CommandHandlers;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;
using Telegram.Bot;

namespace RetroBot.Application.Jobs;

[DisallowConcurrentExecution]
public class QuestionJob : IJob
{
    private readonly ITelegramBotClient bot;
    private readonly IStorage storage;

    public QuestionJob(ITelegramBotClient bot, IStorage storage)
    {
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
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
                    var questions = await storage.TryGetQuestionsAsync();
                    foreach (var question in questions)
                    {
                        await bot.SendTextMessageAsync(user.Id, question.Text);
                    }
                }
            }            
        }
        
    }
}