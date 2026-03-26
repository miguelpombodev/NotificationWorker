using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class NotificationService(IEnumerable<INotificationHandler> handlers) : INotificationService
{
    public async Task ProcessAsync(NotificationRequested notification)
    {
        var handler = handlers.FirstOrDefault(n => n.Channel == notification.Channel);

        if (handler is null) throw new InvalidOperationException("Handler does not exist");

        await handler.HandleAsync(notification);
    }
}