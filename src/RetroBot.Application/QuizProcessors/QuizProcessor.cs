using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Application.QuizProcessors.Interfaces;
using RetroBot.Core;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace RetroBot.Application.QuizProcessors;

public sealed class QuizProcessor : IQuizProcessor
{
    private readonly ITelegramBotClient bot;
    private readonly IStorage storage;

    public QuizProcessor(ITelegramBotClient bot, IStorage storage)
    {
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.bot.OnMessage += BotOnOnMessage;
    }
    
    private async void BotOnOnMessage(object? sender, MessageEventArgs e)
    {
        await Execute(e.Message.From.Id, e.Message.Text);
    }

    public async Task Execute(long userId, string answer)
    {
        var user = await storage.TryGetByUserIdAsync(userId);
        if (user is null || user.State != UserState.Completed)
        {
            return;
        }

        var questions = await storage.TryGetQuestionsAsync();
        var userAnswers = await storage.TryGetAnswersByUserId(userId);

        var lastAnsweredQustion = userAnswers.FirstOrDefault(userAnswer => string.IsNullOrEmpty(userAnswer.Text));
        if (lastAnsweredQustion is not null && !string.IsNullOrEmpty(answer))
        {
            lastAnsweredQustion.Text = answer;
            await storage.TryUpdateAnswerAsync(lastAnsweredQustion);
        }
        
        var unAnsweredQuestion =
            questions.FirstOrDefault(question => userAnswers.All(answer => answer.QuestionId != question.Id));
        if (unAnsweredQuestion is not null)
        {
            await bot.SendTextMessageAsync(userId, unAnsweredQuestion.Text);

            var answerObj = new Answer
            {
                Id = Guid.NewGuid(),
                QuestionId = unAnsweredQuestion.Id,
                Text = string.Empty,
                UserId = userId
            };
            await storage.TryAddAnswerAsync(answerObj);
        }
    }
}