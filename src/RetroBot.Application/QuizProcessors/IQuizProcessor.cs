namespace RetroBot.Application.QuizProcessors;

public interface IQuizProcessor
{
    Task ExecuteAsync(long userId, string answer);
}