using NotificationWorker.Application.Extensions;
using NotificationWorker.Domain;
using NotificationWorker.Domain.Models.Providers;
using NotificationWorker.Infrastructure.Extensions;
using Serilog;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);


Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.WriteTo.Console()
	.CreateBootstrapLogger();

IConfiguration configuration = builder.Configuration;

builder.Services
	.AddModels(configuration)
	.AddSerilogService()
	.AddOpenTelemetryService(configuration)
	.AddMassTransitService()
	.AddDependencyInjections()
	.AddAllHealthChecks();

await builder.Build().RunAsync();
