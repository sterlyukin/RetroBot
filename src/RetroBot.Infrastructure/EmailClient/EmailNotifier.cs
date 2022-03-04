using System.Text;
using FluentEmail.Core;
using RetroBot.Application.Contracts.Services.Email;
using RetroBot.Application.Contracts.Services.Storage;
using RetroBot.Core;

namespace RetroBot.Infrastructure.EmailClient;

internal sealed class EmailNotifier : IEmailNotifier
{
    private readonly IStorage storage;
    private readonly EmailOptions emailOptions;
    private readonly IFluentEmail mailSender;

    public EmailNotifier(IStorage storage, EmailOptions emailOptions, IFluentEmail mailSender)
    {
        this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.emailOptions = emailOptions ?? throw new ArgumentNullException(nameof(emailOptions));
        this.mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
    }

    public async Task ExecuteAsync()
    {
        var answers = await storage.TryGetAnswersAsync();
        if (!answers.Any())
            return;

        var teamsReports = await BuildTeamsReportsAsync();
        foreach (var teamReport in teamsReports)
        {
            await SendTeamReportAsync(teamReport);
        }
    }

    private async Task<IList<TeamReport>> BuildTeamsReportsAsync()
    {
        var teamReports = new List<TeamReport>();

        var teams = await storage.TryGetTeamsAsync();
        var questions = await storage.TryGetQuestionsAsync();

        foreach (var team in teams)
        {
            var teamReport = await BuildTeamReportAsync(team, questions);
            teamReports.Add(teamReport);
        }

        return teamReports;
    }

    private async Task SendTeamReportAsync(TeamReport teamReport)
    {
        var sendEmailResponse = await mailSender
            .To(teamReport.TeamleadEmail)
            .Subject(emailOptions.Subject)
            .Body(teamReport.Report)
            .SendAsync();
    }

    private async Task<TeamReport> BuildTeamReportAsync(Team team, IList<Question> questions)
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine($"Report for team with id = {team.Id}");
        report.AppendLine($"Teamlead email = {team.TeamLeadEmail}");
            
        foreach (var question in questions)
        {
            report.AppendLine($"Question = {question.Text}");
            report.AppendLine("Answers:");
                
            foreach (var user in team.Users)
            {
                var userAnswers = await storage.TryGetAnswersByUserId(user.Id);
                var answer = userAnswers.FirstOrDefault(currentAnswer => currentAnswer.QuestionId == question.Id);
                if(answer is null)
                    continue;

                report.AppendLine($"{answer.Text};");
                report.AppendLine();
            }
        }
        
        return new TeamReport
        {
            TeamId = team.Id,
            TeamleadEmail = team.TeamLeadEmail,
            Report = report.ToString()
        };
    }
}