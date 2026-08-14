using System.ComponentModel.DataAnnotations;

namespace NotificationWorker.Domain.Models.Providers;

public sealed class ObservabiltyOptions
{
	[Required(ErrorMessage = "OpenTelemetry:EndpointUrl is required.")]
	[Url(ErrorMessage = "Please enter a valid URL Telemetry Provider")]
	public string EndpointUrl { get; set; } = string.Empty;

	[Required(ErrorMessage = "OpenTelemetry:ApplicationName is required.")]
	public string ApplicationName { get; set; } = string.Empty;

	[Required(ErrorMessage = "OpenTelemetry:Environment is required.")]
	[AllowedValues(
			"Development",
			"Production",
			ErrorMessage = "Environment value must be either 'Development' or 'Production'"
		)
	]
	public string Environment { get; set; } = string.Empty;
	
	[Required(ErrorMessage = "OpenTelemetry:ApplicationVersion is required.")]
	public string ApplicationVersion { get; set; } = "1.0.0";
}
