using NotificationWorker.Application.Contracts;
using RazorLight;

namespace NotificationWorker.Application.Services;

public class RazorTemplateRenderer : ITemplateRenderer
{
    private readonly RazorLightEngine _engine;

    public RazorTemplateRenderer()
    {
        _engine = new RazorLightEngineBuilder().UseEmbeddedResourcesProject(typeof(RazorTemplateRenderer))
            .UseMemoryCachingProvider().Build();
    }
    
    public Task<string> RenderAsync(string template, object model)
    {
        return _engine.CompileRenderAsync(template, model);
    }
}