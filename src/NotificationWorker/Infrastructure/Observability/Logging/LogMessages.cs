namespace NotificationWorker.Infrastructure.Observability.Logging;

public static class LogMessages
{
    public static void NotificationReceived(
        ILogger logger,
        Guid notificationId,
        string channel)
    {
        logger.LogInformation(
            "Notification {NotificationId} received for channel {Channel}",
            notificationId,
            channel);
    }

    public static void NotificationSent(
        ILogger logger,
        Guid notificationId)
    {
        logger.LogInformation(
            "Notification {NotificationId} successfully sent",
            notificationId);
    }

    public static void NotificationFailed(
        ILogger logger,
        Guid notificationId,
        Exception exception)
    {
        logger.LogError(
            exception,
            "Notification {NotificationId} failed",
            notificationId);
    }
}