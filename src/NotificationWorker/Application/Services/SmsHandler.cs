using Cloudmart.Contracts.Messaging.Enums;
using Cloudmart.Contracts.Messaging.Interfaces.Notifications;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class SmsHandler(ISmsDispatcher dispatcher) : INotificationHandler
{
    public NotificationChannel Channel => NotificationChannel.Sms;

    public Task HandleAsync(INotificationRequest notification, CancellationToken ct)
    {
        return dispatcher.SendAsync(notification);
    }
}