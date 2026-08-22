using Cloudmart.Contracts.Messaging.Interfaces.Notifications;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Contracts;

public interface INotificationService
{
    Task ProcessAsync(INotificationRequest notification, CancellationToken ct);
}