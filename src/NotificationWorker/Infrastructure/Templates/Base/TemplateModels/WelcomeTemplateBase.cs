namespace NotificationWorker.Infrastructure.Templates.TemplatesModels;

public class WelcomeTemplateBase(string name, string email, string loginUrl)
{
	public string Name { get; set; } = name;

	public string Email { get; set; } = email;

	public string LoginUrl { get; set; } = loginUrl;
}
