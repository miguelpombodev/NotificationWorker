namespace NotificationWorker.Application.Contracts;

public interface ITemplateRenderer
{
    Task<string> RenderAsync<TModel>(string project, string template, TModel model);
}