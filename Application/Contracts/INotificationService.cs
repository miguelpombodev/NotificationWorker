using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Contracts;

public interface INotificationService
{
    Task ProcessAsync(NotificationRequested notification);
}