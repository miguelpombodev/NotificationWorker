using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;
using NotificationWorker.Domain.Models.Emails;
using NotificationWorker.Domain.Models.Providers;
using NotificationWorker.Infrastructure;
using NotificationWorker.Infrastructure.Templates.TemplatesModels;
using RabbitMQ.Client;

namespace NotificationWorker.Application.Services;

public class EmailDispatcher(
    ILogger<EmailDispatcher> logger,
    IEmailQueuePublisher publisher)
    : IEmailDispatcher
{

    public async Task SendAsync(EmailToBeSend emailToBeSend)
    {
        await RetryPolicies.EmailRetry(logger).ExecuteAsync(async token =>
        {
            await publisher.PublishAsync(emailToBeSend, token);
        });
    }
}