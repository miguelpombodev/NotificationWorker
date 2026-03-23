using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.OpenTelemetry;

namespace NotificationWorker.Infrastructure.Extensions;

public static class SerilogExtensions
{
    public static IServiceCollection AddSerilogService(this IServiceCollection services, string env, string otelEndpoint)
    {
        services.AddSerilog((services, lc) =>
        {
            lc.ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "Notification Worker")
                .Enrich.WithProperty("Environment", env)
                .WriteTo.Console(new JsonFormatter())
                .WriteTo.OpenTelemetry((options) =>
                {
                    options.Endpoint = otelEndpoint;
                    options.Protocol = OtlpProtocol.Grpc;
                    options.ResourceAttributes = new Dictionary<string, object>()
                    {
                        ["service.name"] = "Notification Worker",
                        ["service.version"] = "1.0.0"
                    };
                });
        });
        return services;
    }

}