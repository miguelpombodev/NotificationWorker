using MassTransit;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Infrastructure;

public class NotificationRequestedConsumer : IConsumer<Batch<NotificationRequested>>
{
    private readonly INotificationService _notificationService;

    public NotificationRequestedConsumer(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Consume(ConsumeContext<Batch<NotificationRequested>> context)
    {
        var tasks = context.Message.Select(async x => await _notificationService.ProcessAsync(x.Message));

        await Task.WhenAll(tasks);
    }
}