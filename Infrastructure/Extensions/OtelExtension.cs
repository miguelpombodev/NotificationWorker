using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace NotificationWorker.Infrastructure.Extensions;

public static class OtelExtension
{
    public static IServiceCollection AddOpenTelemetryService(this IServiceCollection services, string otelEndpoint)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(r =>
                r.AddService("notification-worker", serviceVersion: "1.0.0").AddEnvironmentVariableDetector())
            .WithTracing(t => t
                .AddSource("MassTransit")
                .AddOtlpExporter(o => o.Endpoint = new Uri(otelEndpoint)))
            .WithMetrics(m => m
                .AddRuntimeInstrumentation()
                .AddMeter("MassTransit")
                .AddOtlpExporter(o => o.Endpoint = new Uri(otelEndpoint)));
        return services;
    }
}