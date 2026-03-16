using MassTransit;
using NotificationWorker;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Application.Services;
using NotificationWorker.Domain.Models;
using NotificationWorker.Infrastructure;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog();
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotificationRequestedConsumer>();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"]);
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISender, EmailSender>();

builder.Services.AddSingleton<ITemplateRenderer, RazorTemplateRenderer>();

var host = builder.Build();
host.Run();
