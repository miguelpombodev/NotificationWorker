using NotificationWorker.Domain.Contracts;
using NotificationWorker.Domain.Models.Emails;

namespace NotificationWorker.Application.Contracts;

public interface IEmailQueuePublisher
{
    Task PublishAsync(EmailToBeSendContract email, CancellationToken ct);
}