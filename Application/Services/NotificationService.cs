using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Enums;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IEmailDispatcher _emailDispatcher;

    public NotificationService(
        IEmailDispatcher emailDispatcher
    )
    {
        _emailDispatcher = emailDispatcher;
    }

    public async Task ProcessAsync(NotificationRequested notification)
    {
        switch (notification.Channel)
        {
            case NotificationChannel.Email:
                await _emailDispatcher.SendAsync(notification);
                break;

            // case NotificationChannel.Sms:
            //     await _smsSender.SendAsync(notification);
            //     break;
        }
    }
}