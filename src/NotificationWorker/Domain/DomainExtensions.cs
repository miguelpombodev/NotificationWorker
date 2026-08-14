using NotificationWorker.Domain.Models.Providers;

namespace NotificationWorker.Domain;

public static class DomainExtensions
{
	public static IServiceCollection AddModels(this IServiceCollection services, IConfiguration configuration)
	{
		GetOptionsBySection<RabbitMqOptions>(services, configuration, "RabbitMq");
		GetOptionsBySection<EmailSender>(services, configuration, "EmailSender");
		GetOptionsBySection<ObservabiltyOptions>(services, configuration, "OpenTelemetry");

		return services;
	}

	private static void GetOptionsBySection<T>(
		IServiceCollection services,
		IConfiguration configuration,
		string sectionName) where T : class
	{
		try
		{
			services.AddOptions<T>().Bind(
					configuration.GetSection(sectionName)
				)
				.ValidateDataAnnotations()
				.ValidateOnStart();
		}
		catch (Exception e)
		{
			throw new ArgumentNullException(
				$"Value for {sectionName.ToUpper()} must be valid, please check the informed data");
		}
	}
}
