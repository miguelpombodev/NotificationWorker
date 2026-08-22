using Cloudmart.Contracts.Messaging.Interfaces.Notifications;

namespace NotificationWorker.Application.Contracts;

public interface IDispatcher
{
    Task SendAsync(INotificationRequest notification);
}