using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Enums;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class SmsHandler(ISmsDispatcher dispatcher) : INotificationHandler
{
    public NotificationChannel Channel => NotificationChannel.Sms;

    public Task HandleAsync(NotificationRequested notification)
    {
        return dispatcher.SendAsync(notification);
    }
}