using Quartz;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Quiz;
using RetroBot.Application.Report;
using RetroBot.Core;

namespace RetroBot.Application.Jobs;

[DisallowConcurrentExecution]
public sealed class QuestionJob : IJob
{
    private readonly ITeamRepository teamRepository;
    private readonly IAnswerRepository answerRepository;
    private readonly QuizProcessor quizProcessor;
    private readonly ReportManager reportManager;

    public QuestionJob(
        ITeamRepository teamRepository,
        IAnswerRepository answerRepository,
        QuizProcessor quizProcessor,
        ReportManager reportManager)
    {
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.answerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
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
        var answers = await answerRepository.TryGetAnswersAsync();
        if (answers.Any())
            await reportManager.ExecuteAsync();
    }

    private async Task RemoveObsoleteAnswersAsync()
    {
        await answerRepository.TryDeleteAnswersAsync();
    }

    private async Task GetUpToDateAnswersAsync()
    {
        var teams = await teamRepository.TryGetTeamsAsync();
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