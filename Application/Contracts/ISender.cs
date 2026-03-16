using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Contracts;

public interface ISender
{
    Task SendAsync(NotificationRequested notification);
}