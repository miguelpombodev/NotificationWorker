using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Enums;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class NotificationService : INotificationService
{
    private readonly ISender _emailSender;
    private readonly ISender _smsSender;

    public NotificationService(
        ISender emailSender,
        ISender smsSender)
    {
        _emailSender = emailSender;
        _smsSender = smsSender;
    }
    
    public async Task ProcessAsync(NotificationRequested notification)
    {
        switch (notification.Channel)
        {
            case NotificationChannel.Email:
                await _emailSender.SendAsync(notification);
                break;
            
            case NotificationChannel.Sms:
                await _smsSender.SendAsync(notification);
                break;
        }
    }
}