

using CloudMart.Messaging.Contracts;
using NotificationWorker.Domain.Contracts;

namespace NotificationWorker.Application.Contracts;

public interface IEmailQueuePublisher
{
    Task PublishAsync(EmailToBeSendContract email, CancellationToken ct);
}