using CloudMart.Messaging.Contracts;
using MassTransit;
using Microsoft.Extensions.Options;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models.Providers;

namespace NotificationWorker.Infrastructure;

public sealed class EmailQueuePublisher(
	ISendEndpointProvider sendEndpointProvider,
	IOptions<EmailSender> options,
	ILogger<EmailQueuePublisher> logger)
	: IEmailQueuePublisher
{
	private readonly EmailSender _options = options.Value;

	public async Task PublishAsync(
		EmailToBeSendContract email,
		CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(email);

		var endpoint = await sendEndpointProvider.GetSendEndpoint(
			new Uri($"queue:{_options.QueueName}"));

		await endpoint.Send(
			email,
			ct);

		logger.LogInformation(
			"Email command sent successfully. MessageId: {MessageId}",
			email.Id);
	}
}