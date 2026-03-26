using NotificationWorker.Domain.Enums;

namespace NotificationWorker.Domain.Models;

public sealed class NotificationRequested
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public NotificationChannel Channel { get; init; }

    public string Recipient { get; init; } = string.Empty;

    public string Project { get; init; } = string.Empty;

    public string Template { get; init; } = string.Empty;

    public Dictionary<string, object> Data { get; init; } = new();
}