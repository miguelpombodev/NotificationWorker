using MassTransit;
using Microsoft.Extensions.Options;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models.Emails;
using NotificationWorker.Domain.Models.Providers;

namespace NotificationWorker.Infrastructure;

public sealed class EmailQueuePublisher(
	ISendEndpointProvider sendEndpointProvider,
	IOptions<RabbitMqOptions> options,
	ILogger<EmailQueuePublisher> logger)
	: IEmailQueuePublisher
{
	private readonly RabbitMqOptions _options = options.Value;

	public async Task PublishAsync(
		EmailToBeSend email,
		CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(email);

		var endpoint = await sendEndpointProvider.GetSendEndpoint(
			new Uri($"queue:{_options.EmailSenderQueueName}"));

		await endpoint.Send(
			email,
			ct);

		logger.LogInformation(
			"Email command sent successfully. MessageId: {MessageId}",
			email.Id);
	}
}