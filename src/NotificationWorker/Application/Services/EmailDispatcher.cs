using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models.Emails;
using NotificationWorker.Infrastructure;

namespace NotificationWorker.Application.Services;

public class EmailDispatcher(
	IEmailQueuePublisher publisher)
	: IEmailDispatcher
{
	public async Task SendAsync(
		EmailToBeSend emailToBeSend, CancellationToken ct)
	{
		await publisher.PublishAsync(
			emailToBeSend, ct
		);
	}
}
