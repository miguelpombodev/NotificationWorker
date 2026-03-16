namespace NotificationWorker.Application.Contracts;

public interface ITemplateRenderer
{
    Task<string> RenderAsync(string template, object model);
}