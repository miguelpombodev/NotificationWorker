using NotificationWorker.Domain.Models.Emails;

namespace NotificationWorker.Application.Contracts;

public interface IEmailQueuePublisher
{
    Task PublishAsync(EmailToBeSend email, CancellationToken ct);
}