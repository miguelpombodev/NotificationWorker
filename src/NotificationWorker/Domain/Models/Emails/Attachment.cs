namespace NotificationWorker.Domain.Models.Emails;

public sealed record Attachment(
    string FileName,
    byte[] Content,
    string ContentType = "application/octet-stream"
);