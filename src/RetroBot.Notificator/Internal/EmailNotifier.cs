﻿using FluentEmail.Core;
using RetroBot.Application.Contracts.Services.Notification;
using RetroBot.Core.Entities;
using RetroBot.Infrastructure.EmailClient;

namespace RetroBot.Notificator.Internal;

internal sealed class EmailNotifier : INotifier
{
    private readonly EmailOptions emailOptions;
    private readonly IFluentEmail mailSender;

    public EmailNotifier(EmailOptions emailOptions, IFluentEmail mailSender)
    {
        this.emailOptions = emailOptions ?? throw new ArgumentNullException(nameof(emailOptions));
        this.mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
    }

    public async Task NotifyAsync(Team team, string report)
    {
        await mailSender
            .To(team.TeamLeadEmail)
            .Subject(emailOptions.Subject)
            .Body(report)
            .SendAsync();
    }
}