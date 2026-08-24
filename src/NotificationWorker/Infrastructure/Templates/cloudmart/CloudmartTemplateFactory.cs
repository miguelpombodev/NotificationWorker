using Cloudmart.Contracts.Messaging.Interfaces.Notifications;
using NotificationWorker.Infrastructure.Templates.cloudmart.TemplatesModels;

namespace NotificationWorker.Infrastructure.Templates.cloudmart;

public sealed class CloudmartTemplateFactory : IProjectTemplateFactory
{
	public string Project => "cloudmart";

	public object Create(string template, INotificationRequest notification)
	{
		return template.ToLowerInvariant() switch
		{
			"welcome" => new WelcomeTemplateModel(
				Name: GetRequired(notification, "name"),
				Email: notification.Recipient,
				LoginUrl: GetRequired(notification, "loginUrl")),

			"welcome_role" => new WelcomeRoleTemplateModel(
				Name: GetRequired(notification, "name"),
				Email: notification.Recipient,
				LoginUrl: GetRequired(notification, "loginUrl"),
				Role: GetRequired(notification, "role")),

			_ => throw new InvalidOperationException(
				$"Template '{template}' não encontrado para o projeto '{Project}'.")
		};
	}
	
	private static string GetRequired(
		INotificationRequest notification,
		string key)
	{
		if (!notification.Data.TryGetValue(key, out var value) ||
		    string.IsNullOrWhiteSpace(value?.ToString()))
		{
			throw new InvalidOperationException(
				$"Required field '{key}' was not provided.");
		}

		return value.ToString()!;
	}
}
