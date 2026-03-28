namespace NotificationWorker.Infrastructure.Extensions;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddAllHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks().AddRabbitMQ();
        return services;
    }
}