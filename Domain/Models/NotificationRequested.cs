using NotificationWorker.Domain.Enums;

namespace NotificationWorker.Domain.Models;

public class NotificationRequested
{
    public Guid Id { get; init; }

    public NotificationChannel Channel { get; init; }

    public string Recipient { get; init; } = string.Empty;

    public string Template { get; init; } = string.Empty;

    public Dictionary<string, object> Data { get; init; } = new();
}