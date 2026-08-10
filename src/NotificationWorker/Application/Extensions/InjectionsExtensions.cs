using NotificationWorker.Application.Contracts;
using NotificationWorker.Application.Services;
using NotificationWorker.Infrastructure;

namespace NotificationWorker.Application.Extensions;

public static class InjectionsExtensions
{
    public static IServiceCollection AddDependencyInjections(
        this IServiceCollection services)
    {
        services.AddScoped<INotificationHandler, EmailHandler>();
        services.AddScoped<INotificationHandler, SmsHandler>();

        services.AddScoped<INotificationService, NotificationService>();

        services.AddScoped<IEmailDispatcher, EmailDispatcher>();
        services.AddScoped<ISmsDispatcher, SmsDispatcher>();

        services.AddScoped<IEmailQueuePublisher, EmailQueuePublisher>();

        services.AddSingleton<ITemplateRenderer, RazorTemplateRenderer>();

        return services;
    }
}