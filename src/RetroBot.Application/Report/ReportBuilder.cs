using System.Text;
using RetroBot.Core.Entities;

namespace RetroBot.Application.Report;

public sealed class ReportBuilder
{
    public string Execute(Team team, IList<Question> questions, IList<Answer> teamAnswers)
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine($"Report for team with name = {team.Name}");
        report.AppendLine($"Teamlead email = {team.TeamLeadEmail}");
            
        foreach (var question in questions)
        {
            report.AppendLine($"Question = {question.Text}");
            report.AppendLine("Answers:");
                
            foreach (var user in team.Users)
            {
                var userAnswers = teamAnswers.Where(currentAnswer => currentAnswer.UserId == user.Id);
                var answer = userAnswers.FirstOrDefault(currentAnswer => currentAnswer.QuestionId == question.Id);
                if(answer is null)
                    continue;

                report.AppendLine($"{answer.Text};");
                report.AppendLine();
            }
        }

        return report.ToString();
    }
}