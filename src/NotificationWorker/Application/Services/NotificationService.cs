using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class NotificationService(INotificationHandler handler, ILogger<NotificationService> logger)
    : INotificationService
{
    public async Task ProcessAsync(NotificationRequested notification)
    {
        try
        {
            await handler.HandleAsync(notification);
        }
        catch (Exception e)
        {
            logger.LogError(
                "[ERROR] Something went wrong, please check stack trace. Exception Message: {Message}, Exception StackTrace: {StackTrace}",
                e.Message,
                e.StackTrace);
            throw;
        }
    }
}