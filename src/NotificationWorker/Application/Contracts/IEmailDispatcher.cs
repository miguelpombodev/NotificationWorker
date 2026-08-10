using NotificationWorker.Domain.Models.Emails;

namespace NotificationWorker.Application.Contracts;

public interface IEmailDispatcher
{
    Task SendAsync(EmailToBeSend emailToBeSend, CancellationToken ct);
}