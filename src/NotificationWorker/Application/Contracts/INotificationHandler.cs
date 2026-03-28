using NotificationWorker.Domain.Enums;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Contracts;

public interface INotificationHandler
{
    NotificationChannel Channel { get; }
    Task HandleAsync(NotificationRequested notification);
}