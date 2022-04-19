using Quartz;
using RetroBot.Application.Contracts.Services.DataStorage;
using RetroBot.Application.Report;

namespace RetroBot.Application.Jobs;

[DisallowConcurrentExecution]
public sealed class NotifyJob : IJob
{
    private readonly IAnswerRepository answerRepository;
    private readonly ReportManager reportManager;

    public NotifyJob(IAnswerRepository answerRepository, ReportManager reportManager)
    {
        this.answerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
        this.reportManager = reportManager ?? throw new ArgumentNullException(nameof(reportManager));
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await SendAnswersNotificationsAsync();
    }

    private async Task SendAnswersNotificationsAsync()
    {
        var answers = await answerRepository.GetAllAsync();
        if (answers.Any())
            await reportManager.ExecuteAsync();
    }
}