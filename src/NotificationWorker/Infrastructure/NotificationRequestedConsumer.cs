using Cloudmart.Contracts.Messaging.Interfaces.Notifications;
using MassTransit;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;

namespace NotificationWorker.Infrastructure;

public class NotificationRequestedConsumer : IConsumer<INotificationRequest>
{
	private readonly INotificationService _notificationService;

	public NotificationRequestedConsumer(
		INotificationService notificationService)
	{
		_notificationService = notificationService;
	}

	public async Task Consume(
		ConsumeContext<INotificationRequest> context)
	{
		CancellationToken ct = context.CancellationToken;
		
		if (context.CancellationToken.IsCancellationRequested)
		{
			throw new OperationCanceledException();
		}
		
		await _notificationService.ProcessAsync(
			context.Message, ct);
	}
}
