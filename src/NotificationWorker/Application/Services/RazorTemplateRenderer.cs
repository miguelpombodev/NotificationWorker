using NotificationWorker.Application.Contracts;
using RazorLight;

namespace NotificationWorker.Application.Services;

public class RazorTemplateRenderer(
    ILogger<RazorTemplateRenderer> logger) : ITemplateRenderer
{
    private readonly RazorLightEngine _engine = CreateEngine();

    private static RazorLightEngine CreateEngine()
    {
        var templatesPath = Path.Combine(
            AppContext.BaseDirectory,
            "Infrastructure",
            "Templates");

        if (!Directory.Exists(templatesPath))
        {
            throw new DirectoryNotFoundException(
                $"Templates directory not found: {templatesPath}");
        }

        return new RazorLightEngineBuilder()
            .UseFileSystemProject(templatesPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<string> RenderAsync<TModel>(
        string project,
        string template,
        TModel model)
    {
        var templatePath = $"{project}/{template}.cshtml";

        logger.LogInformation(
            "Rendering template {TemplatePath}",
            templatePath);

        return await _engine.CompileRenderAsync(
            templatePath,
            model);
    }
}