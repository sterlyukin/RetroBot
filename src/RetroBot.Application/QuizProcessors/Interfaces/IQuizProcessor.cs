namespace RetroBot.Application.QuizProcessors.Interfaces;

public interface IQuizProcessor
{
    Task Execute(long userId, string answer);
}