using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Contracts;

public interface IDispatcher
{
    Task SendAsync(NotificationRequested notification);
}