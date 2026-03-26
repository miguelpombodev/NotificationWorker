using NotificationWorker.Application.Contracts;
using RazorLight;

namespace NotificationWorker.Application.Services;

public class RazorTemplateRenderer : ITemplateRenderer
{
    private readonly RazorLightEngine _engine;
    private readonly ILogger<RazorTemplateRenderer> _logger;

    private readonly string _templatesBaseFolderName =
        Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure/Templates");

    public RazorTemplateRenderer(ILogger<RazorTemplateRenderer> logger)
    {
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(_templatesBaseFolderName)
            .UseMemoryCachingProvider()
            .Build();

        _logger = logger;
    }

    public Task<string> RenderAsync(string project, string template, object model)
    {
        var formattedProjectFolderName = project.Replace(" ", "_").Replace("-", "_").ToLower();

        if (!Path.Exists($"{_templatesBaseFolderName}/{formattedProjectFolderName}"))
        {
            _logger.LogError(
                "Project Templates Folder {ProjectFolderName} was called but its no exists, ExceptionType: {Exception}",
                formattedProjectFolderName,
                nameof(DirectoryNotFoundException));

            throw new DirectoryNotFoundException(
                $"Project Folder {formattedProjectFolderName} in Templates folder does not exist! Please check it ");
        }


        return _engine.CompileRenderAsync($"{formattedProjectFolderName}/{template}.cshtml", model);
    }
}