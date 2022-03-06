using RetroBot.Application.Contracts.Services.Notify;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core.Entities;

namespace RetroBot.Application.Report;

public class ReportManager
{
    private readonly IStorage storage;
    private readonly INotifier notifier;
    private readonly ReportBuilder reportBuilder;

    public ReportManager(IStorage storage, INotifier notifier, ReportBuilder reportBuilder)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
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

        var teams = await storage.TryGetTeamsAsync();
        var questions = await storage.TryGetQuestionsAsync();

        foreach (var team in teams)
        {
            var teamAnswers = await storage.TryGetAnswersByTeamIdAsync(team.Id);
            var report = reportBuilder.Execute(team, questions, teamAnswers);
            teamReports.Add(team, report);
        }

        return teamReports;
    }
}