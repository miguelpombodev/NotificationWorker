using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class SmsDispatcher(ILogger<SmsDispatcher> logger) : ISmsDispatcher
{
    public async Task SendAsync(NotificationRequested notification)
    {
        var phone = notification.Recipient;
        var message = notification.Data["message"]?.ToString();

        if (message == null) throw new NullReferenceException($"SMS message is missing in message to {phone}");
        
        logger.LogInformation("Sending SMS to {Phone}", phone);

        await Task.Delay(500);
        
        logger.LogInformation("SMS sent successfully to {Phone}!", phone);
    }
}