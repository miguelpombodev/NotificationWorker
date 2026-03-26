using NotificationWorker.Application.Contracts;
using RazorLight;

namespace NotificationWorker.Application.Services;

public class RazorTemplateRenderer : ITemplateRenderer
{
    private readonly RazorLightEngine _engine;

    public RazorTemplateRenderer()
    {
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure/Templates"))
            .UseMemoryCachingProvider()
            .Build();
    }

    public Task<string> RenderAsync(string project, string template, object model)
    {
        return _engine.CompileRenderAsync($"{project}/{template}.cshtml", model);
    }
}