namespace NotificationWorker.Infrastructure.Templates.TemplatesModels;

public class WelcomeTemplateWithRoleModel: WelcomeTemplateBase
{
    public WelcomeTemplateWithRoleModel(string name, string email, string loginUrl, string role) : base(name, email, loginUrl)
    {
        Role = role;
    }

    public string Role { get; set; }
}