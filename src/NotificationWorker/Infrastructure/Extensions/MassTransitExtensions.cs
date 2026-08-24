using System.Text.Json.Serialization;
using Cloudmart.Contracts.Messaging.Interfaces.Notifications;
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
			x.SetKebabCaseEndpointNameFormatter();
			x.AddConsumer<NotificationRequestedConsumer>(cfg =>
			{
				cfg.UseMessageRetry(r =>
				{
					r.Ignore<DirectoryNotFoundException>();
					r.Ignore<FileNotFoundException>();
						
					r.Handle<TimeoutException>();
					r.Handle<HttpRequestException>();
						
					r.Exponential(
						retryLimit: 5, 
						minInterval: TimeSpan.FromSeconds(2),
						maxInterval: TimeSpan.FromSeconds(30),
						intervalDelta: TimeSpan.FromSeconds(5));
				});
			});

			x.UsingRabbitMq((ctx, cfg) =>
			{
				RabbitMqOptions rabbitOptions = ctx.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

				cfg.ConfigureJsonSerializerOptions(options =>
				{
					options.Converters.Add(new JsonStringEnumConverter());
					return options;
				});
				
				cfg.Host(
					rabbitOptions.HostName,
					(ushort)rabbitOptions.Port,
					rabbitOptions.VirtualHost,
					host =>
					{
						host.Username(rabbitOptions.UserName);
						host.Password(rabbitOptions.Password);
						host.RequestedConnectionTimeout(TimeSpan.FromSeconds(10));
						host.Heartbeat(TimeSpan.FromSeconds(60));
					});

				cfg.ReceiveEndpoint(rabbitOptions.QueueName, e =>
				{
					e.Durable = true;
					e.PrefetchCount = rabbitOptions.PrefetchCount;
					e.ConcurrentMessageLimit = Math.Max(1, rabbitOptions.PrefetchCount / 2);
					
					e.DeadLetterExchange = $"{rabbitOptions.QueueName}.dlx";
					e.SetQueueArgument("x-max-length", 10000);
					
					e.UseRateLimit(
						rateLimit: 10,
						interval: TimeSpan.FromSeconds(1));
					
					e.Bind<INotificationRequest>();

					e.ConfigureConsumer<NotificationRequestedConsumer>(ctx);
				});
			});
		});

		return services;
	}
}
