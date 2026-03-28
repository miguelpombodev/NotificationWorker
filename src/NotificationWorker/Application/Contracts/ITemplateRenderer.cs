namespace NotificationWorker.Application.Contracts;

public interface ITemplateRenderer
{
    Task<string> RenderAsync(string project, string template, object model);
}