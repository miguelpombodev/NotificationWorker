namespace NotificationWorker.Domain.Models.Emails;

public sealed record EmailPayload(
    string Subject,
    string TemplateName,
    object TemplateModel
);