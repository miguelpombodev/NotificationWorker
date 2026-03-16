using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;
using NotificationWorker.Infrastructure;

namespace NotificationWorker.Application.Services;

public class EmailSender : ISender
{
    private readonly ITemplateRenderer _templateRenderer;

    public EmailSender(ITemplateRenderer templateRenderer)
    {
        _templateRenderer = templateRenderer;
    }

    public async Task SendAsync(NotificationRequested notification)
    {
        await RetryPolicies.EmailRetry.ExecuteAndCaptureAsync(async () =>
        {
            var body = await _templateRenderer.RenderAsync(notification.Template, notification.Data);
           
            // send to email sender queue
            await SendEmail(notification.Recipient, body);
        });
    }
}