using System.Diagnostics.Metrics;

namespace NotificationWorker.Infrastructure.Observability.Metrics;

public static class NotificationMetrics
{
    private static readonly Meter Meter = new("NotificationWorker");
    
    public static Counter<int> NotificationsProcessed = Meter.CreateCounter<int>("notifications_processed");
    public static Counter<int> NotificationsFailed = Meter.CreateCounter<int>("notifications_failed");
}