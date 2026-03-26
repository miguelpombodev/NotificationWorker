using System.Text;
using MassTransit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;
using NotificationWorker.Domain.Models.Emails;
using NotificationWorker.Infrastructure;
using NotificationWorker.Infrastructure.Templates.TemplatesModels;
using RabbitMQ.Client;

namespace NotificationWorker.Application.Services;

public class EmailDispatcher(ITemplateRenderer templateRenderer, ILogger<EmailDispatcher> logger)
    : IEmailDispatcher
{
    private readonly string _emailSenderExchange = "sub-email-sender-exchange";
    private readonly string _emailSenderRk = "sub-email";

    public async Task SendAsync(NotificationRequested notification)
    {
        await RetryPolicies.EmailRetry.ExecuteAsync(async () =>
        {
            var model = MapTemplate(notification);
            var body = await templateRenderer.RenderAsync(notification.Template, model);
            
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
        logger.LogInformation(
            "Creating connection and channel with Email Worker Queue. Exchange: {EmailSenderExchange}, Routing Key: {}",
            _emailSenderExchange,
            _emailSenderRk
        );

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

        logger.LogInformation("Created Email Model with subject {MessageSubject} by ID: {MessageId}", message.Subject,
            message.Id);

        var json = JsonConvert.SerializeObject(message);
        var bodyBytes = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: _emailSenderExchange,
            routingKey: _emailSenderRk,
            body: bodyBytes
        );

        logger.LogInformation("Email message sent successfully! MessageId: {MessageId}", message.Id);
    }
}