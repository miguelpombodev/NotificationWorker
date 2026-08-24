using Cloudmart.Contracts.Messaging.Interfaces.Notifications;

namespace NotificationWorker.Infrastructure.Templates;

public interface IProjectTemplateFactory
{
	string Project { get; }

	object Create(
		string template,
		INotificationRequest notification
	);
}
