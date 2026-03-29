using MassTransit;
using Microsoft.Extensions.Options;
using NotificationWorker.Domain.Models;
using NotificationWorker.Domain.Models.Providers;

namespace NotificationWorker.Infrastructure.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitService(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<NotificationRequestedConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                var rabbitOptions = ctx.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                cfg.Host(rabbitOptions.HostName, "/", host =>
                {
                    host.Username(rabbitOptions.UserName);
                    host.Password(rabbitOptions.Password);
                    host.RequestedConnectionTimeout(TimeSpan.FromSeconds(10));
                });

                cfg.UseMessageRetry(r =>
                {
                    r.Exponential(
                        retryLimit:    5,
                        minInterval:   TimeSpan.FromSeconds(1),
                        maxInterval:   TimeSpan.FromSeconds(30),
                        intervalDelta: TimeSpan.FromSeconds(2));

                    r.Ignore<DirectoryNotFoundException>();
                    r.Ignore<FileNotFoundException>();
                });

                cfg.ReceiveEndpoint("notification-worker", e =>
                {
                    e.PrefetchCount = rabbitOptions.PrefetchCount;
                    e.ConcurrentMessageLimit = rabbitOptions.PrefetchCount / 2;
                    
                    e.Batch<NotificationRequested>(b =>
                    {
                        b.MessageLimit = 10;
                        b.TimeLimit = TimeSpan.FromSeconds(2);
                        b.ConcurrencyLimit = 4;
                    });
                    
                    e.ConfigureConsumer<NotificationRequestedConsumer>(ctx);
                });
            });
        });

        return services;
    }
}