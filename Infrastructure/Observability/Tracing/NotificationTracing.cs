using System.Diagnostics;

namespace NotificationWorker.Infrastructure.Observability.Tracing;

public static class NotificationTracing
{
    public static readonly ActivitySource ActivitySource = new("NotificationWorker");
}