using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models.Emails;
using NotificationWorker.Infrastructure;

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