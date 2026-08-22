using Cloudmart.Contracts.Messaging.Enums;
using Cloudmart.Contracts.Messaging.Interfaces.Notifications;

namespace NotificationWorker.Application.Contracts;

public interface INotificationHandler
{
    NotificationChannel Channel { get; }
    Task HandleAsync(INotificationRequest notification, CancellationToken ct);
}