using RetroBot.Application.Contracts.Services.Notify;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core.Entities;

namespace RetroBot.Application.Report;

public class ReportManager
{
    private readonly IStorageClient storageClient;
    private readonly INotifier notifier;
    private readonly ReportBuilder reportBuilder;

    public ReportManager(IStorageClient storageClient, INotifier notifier, ReportBuilder reportBuilder)
    {
        this.storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
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

        var teams = await storageClient.TryGetTeamsAsync();
        var questions = await storageClient.TryGetQuestionsAsync();

        foreach (var team in teams)
        {
            var teamAnswers = await storageClient.TryGetAnswersByTeamIdAsync(team.Id);
            var report = reportBuilder.Execute(team, questions, teamAnswers);
            teamReports.Add(team, report);
        }

        return teamReports;
    }
}