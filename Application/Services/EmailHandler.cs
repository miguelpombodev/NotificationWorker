using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Enums;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class EmailHandler(IEmailDispatcher dispatcher) : INotificationHandler
{
    public NotificationChannel Channel => NotificationChannel.Email;

    public Task HandleAsync(NotificationRequested notification)
    {
        return dispatcher.SendAsync(notification);
    }
}