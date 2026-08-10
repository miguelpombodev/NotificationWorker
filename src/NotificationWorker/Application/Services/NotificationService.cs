using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class NotificationService(
    IEnumerable<INotificationHandler> handlers,
    ILogger<NotificationService> logger)
    : INotificationService
{
    public async Task ProcessAsync(NotificationRequested notification)
    {
        try
        {
            var handler = handlers.SingleOrDefault(
                x => x.Channel == notification.Channel);

            if (handler is null)
            {
                throw new InvalidOperationException(
                    $"No notification handler registered for channel '{notification.Channel}'.");
            }

            logger.LogInformation(
                "Processing notification using {Handler} for channel {Channel}",
                handler.GetType().Name,
                notification.Channel);

            await handler.HandleAsync(notification);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error processing notification. Channel: {Channel}, Template: {Template}, Recipient: {Recipient}",
                notification.Channel,
                notification.Template,
                notification.Recipient);

            throw;
        }
    }
}