using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Contracts;
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
		var command = new EmailToBeSendContract
		{
			Id = emailToBeSend.Id,
			To = emailToBeSend.To,
			Cc = emailToBeSend.Cc.ToList(),
			Bcc = emailToBeSend.Bcc.ToList(),
			Subject = emailToBeSend.Subject,
			Body = emailToBeSend.Body,
			IsBodyHtml = true,
			Attachments = emailToBeSend.Attachments
				.Select(x => new EmailAttachment
				{
					FileName = x.FileName,
					ContentType = x.ContentType,
					ContentBase64 = Convert.ToBase64String(x.Content)
				})
				.ToList()
		};
		await publisher.PublishAsync(
			command, ct
		);
	}
}
