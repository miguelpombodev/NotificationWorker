using System.Text;
using MassTransit;
using Newtonsoft.Json;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;
using NotificationWorker.Infrastructure;
using NotificationWorker.Infrastructure.Templates.TemplatesModels;
using RabbitMQ.Client;

namespace NotificationWorker.Application.Services;

public class EmailDispatcher : IEmailDispatcher
{
    private readonly ITemplateRenderer _templateRenderer;
    private readonly ILogger<EmailDispatcher> _logger;

    public EmailDispatcher(ITemplateRenderer templateRenderer, ILogger<EmailDispatcher> logger)
    {
        _templateRenderer = templateRenderer;
        _logger = logger;
    }

    public async Task SendAsync(NotificationRequested notification)
    {
        await RetryPolicies.EmailRetry.ExecuteAsync(async () =>
        {
            var model = MapTemplate(notification);
            var body = await _templateRenderer.RenderAsync(notification.Template, model);

            // send to email sender queue
            await SendEmail(notification.Recipient, body);
        });
    }

    private object MapTemplate(NotificationRequested notification)
    {
        return notification.Template switch
        {
            "welcome" => new WelcomeTemplateModel
            {
                Name = notification.Data["name"]?.ToString() ?? string.Empty,
                Email = notification.Recipient,
                LoginUrl = notification.Data["loginUrl"]?.ToString() ?? string.Empty
            },

            _ => throw new InvalidOperationException("Invalid template")
        };
    }

    private async Task SendEmail(string recipient, string body)
    {
        _logger.LogInformation("Creating connection and channel with Email Worker Queue");
        
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "admin",
            Password = "admin123"
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        var message = new EmailToBeSend
        {
            To = recipient,
            Subject = "Welcome",
            Body = body
        };
        
        _logger.LogInformation("Created Email Model with subject {MessageSubject}", message.Subject);

        var json = JsonConvert.SerializeObject(message);
        var bodyBytes = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "sub-email-sender-exchange",
            routingKey: "sub-email",
            body: bodyBytes
        );
        
        _logger.LogInformation("Email message sent successfully!");
    }
}