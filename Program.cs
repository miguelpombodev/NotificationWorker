using NotificationWorker.Application.Extensions;
using NotificationWorker.Domain.Models.Providers;
using NotificationWorker.Infrastructure.Extensions;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

var otelEndpoint = builder.Configuration["OpenTelemetry:Endpoint"]
                   ?? "http://localhost:4317";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Services.AddSerilogService(builder.Environment.EnvironmentName, otelEndpoint);

builder.Services.AddOptions<RabbitMqOptions>().Bind(
        builder.Configuration.GetSection("RabbitMq")
    )
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOpenTelemetryService(otelEndpoint);

builder.Services.AddMassTransitService();

builder.Services.AddDependencyInjections();
builder.Services.AddAllHealthChecks();

var host = builder.Build();
host.Run();