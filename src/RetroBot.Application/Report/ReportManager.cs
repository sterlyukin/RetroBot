using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Contracts.Services.Notification;
using RetroBot.Core.Entities;

namespace RetroBot.Application.Report;

public sealed class ReportManager
{
    private readonly ITeamRepository teamRepository;
    private readonly IQuestionRepository questionRepository;
    private readonly IAnswerRepository answerRepository;
    private readonly INotifier notifier;
    private readonly ReportBuilder reportBuilder;

    public ReportManager(
        ITeamRepository teamRepository,
        IQuestionRepository questionRepository,
        IAnswerRepository answerRepository,
        INotifier notifier,
        ReportBuilder reportBuilder)
    {
        this.teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        this.questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
        this.answerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
        this.notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        this.reportBuilder = reportBuilder ?? throw new ArgumentNullException(nameof(reportBuilder));
    }

    public async Task ExecuteAsync()
    {
        var teamsReports = await BuildTeamsReportsAsync();
        foreach (var teamReport in teamsReports)
        {
            await notifier.NotifyAsync(teamReport.Key, teamReport.Value);
        }
    }
    
    private async Task<IDictionary<Team, string>> BuildTeamsReportsAsync()
    {
        var teamReports = new Dictionary<Team, string>();

        var teams = await teamRepository.TryGetAsync();
        var questions = await questionRepository.TryGetQuestionsAsync();

        foreach (var team in teams)
        {
            var teamAnswers = await answerRepository.TryGetAnswersByTeamIdAsync(team.Id);
            var report = reportBuilder.Execute(team, questions, teamAnswers);
            teamReports.Add(team, report);
        }

        return teamReports;
    }
}