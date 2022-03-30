using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Core;
using RetroBot.Core.Entities;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace RetroBot.Application.Quiz;

public sealed class QuizProcessor
{
    private readonly ITelegramBotClient bot;
    private readonly IUserRepository userRepository;
    private readonly IQuestionRepository questionRepository;
    private readonly IAnswerRepository answerRepository;

    public QuizProcessor(
        ITelegramBotClient bot,
        IUserRepository userRepository,
        IQuestionRepository questionRepository,
        IAnswerRepository answerRepository)
    {
        this.bot = bot ?? throw new ArgumentNullException(nameof(bot));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
        this.answerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
        this.bot.OnMessage += BotOnOnMessage;
    }
    
    private async void BotOnOnMessage(object? sender, MessageEventArgs e)
    {
        await ExecuteAsync(e.Message.From.Id, e.Message.Text);
    }

    public async Task ExecuteAsync(long userId, string answer)
    {
        var semaphoreObject = new Semaphore(1, 1, name: "Question job");
        semaphoreObject.WaitOne();
        var user = await userRepository.TryGetByIdAsync(userId);
        if (user is null || user.State is not UserState.Completed)
        {
            semaphoreObject.Release();
            return;
        }
        
        var questions = await questionRepository.TryGetQuestionsAsync();
        var userAnswers = await answerRepository.TryGetAnswersByUserIdAsync(userId);

        if (userAnswers.Any(userAnswer => string.Equals(userAnswer.Text, answer)))
        {
            semaphoreObject.Release();
            return;
        }

        await SetPreviousAnswerAsync(answer, userId, userAnswers);
        await AskQuestionAsync(userId, questions, userAnswers);

        semaphoreObject.Release();
    }

    private async Task SetPreviousAnswerAsync(string answer, long userId, IList<Answer> userAnswers)
    {
        var lastUnansweredQuestion = userAnswers.FirstOrDefault(userAnswer =>
            userAnswer.UserId == userId && string.IsNullOrEmpty(userAnswer.Text));
        if (lastUnansweredQuestion is not null && !string.IsNullOrEmpty(answer))
        {
            lastUnansweredQuestion.Text = answer;
            await answerRepository.TryUpdateAnswerAsync(lastUnansweredQuestion);
        }
    }

    private async Task AskQuestionAsync(long userId, IList<Question> questions, IList<Answer> userAnswers)
    {
        var unAnsweredQuestion =
            questions.FirstOrDefault(question =>
                userAnswers.All(currentAnswer => currentAnswer.QuestionId != question.Id));
        if (unAnsweredQuestion is not null)
        {
            var answerObj = new Answer
            {
                Id = Guid.NewGuid(),
                QuestionId = unAnsweredQuestion.Id,
                Text = string.Empty,
                UserId = userId
            };
            
            await answerRepository.TryAddAnswerAsync(answerObj);

            await bot.SendTextMessageAsync(userId, unAnsweredQuestion.Text);
        }
    }
}