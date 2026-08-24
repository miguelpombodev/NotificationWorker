namespace NotificationWorker.Infrastructure.Templates.cloudmart.TemplatesModels;

public sealed record WelcomeTemplateModel(
	string Name,
	string Email,
	string LoginUrl);
