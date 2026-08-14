using Microsoft.Extensions.Options;
using NotificationWorker.Domain.Models.Providers;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.OpenTelemetry;

namespace NotificationWorker.Infrastructure.Extensions;

public static class SerilogExtensions
{
	public static IServiceCollection AddSerilogService(
		this IServiceCollection services)
	{
		services.AddSerilog((config, lc) =>
		{
			ObservabiltyOptions telemetryOptions = config.GetRequiredService<IOptions<ObservabiltyOptions>>().Value;

			lc.ReadFrom.Services(config)
				.Enrich.FromLogContext()
				.Enrich.WithProperty("Application", telemetryOptions.ApplicationName)
				.Enrich.WithProperty("Environment", telemetryOptions.Environment)
				.WriteTo.Console(new JsonFormatter())
				.WriteTo.OpenTelemetry((options) =>
				{
					options.Endpoint = telemetryOptions.EndpointUrl;
					options.Protocol = OtlpProtocol.Grpc;

					options.ResourceAttributes = new Dictionary<string, object>()
					{
						["service.name"] = telemetryOptions.ApplicationName,
						["service.version"] = telemetryOptions.ApplicationVersion
					};
				});
		});

		return services;
	}
}
