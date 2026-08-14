using System.ComponentModel.DataAnnotations;

namespace NotificationWorker.Domain.Models.Providers;

public sealed class EmailSender
{
	[Required(ErrorMessage = "EmailSender:HostName is required.")]
	[MinLength(1)]
	public string HostName { get; set; } = "localhost";

	[Required(ErrorMessage = "EmailSender:QueueName is required.")]
	[MinLength(1)]
	public string QueueName { get; set; } = "sub-email-sender";

	[Range(1, 65535, ErrorMessage = "EmailSender:QueuePort must be between 1 and 65535.")]
	public int QueuePort { get; set; } = 5672;

	[Required(ErrorMessage = "EmailSender:QueueUserName is required.")]
	[MinLength(1)]
	public string QueueUserName { get; set; } = "admin";

	[Required(ErrorMessage = "EmailSender:QueuePassword is required.")]
	[MinLength(1)]
	public string QueuePassword { get; set; } = "admin123";

	[Required(ErrorMessage = "EmailSender:QueueExchangeName is required.")]
	[MinLength(1)]
	public string QueueExchangeName { get; set; } = "sub-email-sender-exchange";

	[Required(ErrorMessage = "EmailSender:QueueRoutingKeyName is required.")]
	[MinLength(1)]
	public string QueueRoutingKeyName { get; set; } = "sub-email";
}
