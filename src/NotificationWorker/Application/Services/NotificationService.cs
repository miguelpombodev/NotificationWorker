using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class NotificationService(IEnumerable<INotificationHandler> handlers, ILogger<NotificationService> logger)
    : INotificationService
{
    public async Task ProcessAsync(NotificationRequested notification)
    {
        var handler = handlers.FirstOrDefault(n => n.Channel == notification.Channel);

        if (handler is null)
        {
            logger.LogInformation(
                "[ERROR] There was an failed attempt to reach an unkwown channel. Channel Option: {ChannelOption}, Correlation Id: {CorrelationId}, Exception Type: {ExceptionType}",
                notification.Channel,
                notification.Id,
                nameof(InvalidOperationException));
            
            throw new InvalidOperationException("Handler does not exist");
        }

        await handler.HandleAsync(notification);
    }
}