using System.Text;
using MassTransit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models;
using NotificationWorker.Domain.Models.Emails;
using NotificationWorker.Domain.Models.Providers;
using NotificationWorker.Infrastructure;
using NotificationWorker.Infrastructure.Templates.TemplatesModels;
using RabbitMQ.Client;

namespace NotificationWorker.Application.Services;

public class EmailDispatcher(
    ITemplateRenderer templateRenderer,
    ILogger<EmailDispatcher> logger,
    IOptions<RabbitMqOptions> rabbitMqOptions)
    : IEmailDispatcher
{
    private readonly RabbitMqOptions _rabbitOptions = rabbitMqOptions.Value;

    public async Task SendAsync(NotificationRequested notification)
    {
        await RetryPolicies.EmailRetry.ExecuteAsync(async () =>
        {
            var model = MapTemplate(notification);
            var body = await templateRenderer.RenderAsync(notification.Project,notification.Template, model.TemplateModel);

            await SendEmail(notification.Recipient, body);
        });
    }

    private EmailPayload MapTemplate(NotificationRequested notification)
    {
        return notification.Template switch
        {
            "welcome" => new EmailPayload(
                Subject: notification.Data["subject"].ToString() ?? string.Empty,
                TemplateName: notification.Template,
                TemplateModel: new WelcomeTemplateModel
                {
                    Name = notification.Data["name"]?.ToString() ?? string.Empty,
                    Email = notification.Recipient,
                    LoginUrl = notification.Data["loginUrl"]?.ToString() ?? string.Empty,
                    Role = notification.Data["role"]?.ToString() ?? string.Empty,
                }),
            _ => throw new InvalidOperationException("Invalid template")
        };
    }

    private async Task SendEmail(string recipient, string body)
    {
        logger.LogInformation(
            "Creating connection and channel with Email Worker Queue. Exchange: {EmailSenderExchange}, Routing Key: {EmailSenderRoutingKey}",
            _rabbitOptions.EmailSenderQueueExchangeName,
            _rabbitOptions.EmailSenderQueueRoutingKeyName
        );

        var factory = new ConnectionFactory
        {
            HostName = _rabbitOptions.EmailSenderHostName,
            Port = _rabbitOptions.EmailSenderQueuePort,
            UserName = _rabbitOptions.EmailSenderQueueUserName,
            Password = _rabbitOptions.EmailSenderQueuePassword
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        var message = BuildMessage(recipient, body);

        logger.LogInformation("Created Email Model with subject {MessageSubject} by ID: {MessageId}",
            message.Subject,
            message.Id
        );

        await channel.BasicPublishAsync(
            exchange: _rabbitOptions.EmailSenderQueueExchangeName,
            routingKey: _rabbitOptions.EmailSenderQueueRoutingKeyName,
            body: message.ParseToBytes()
        );

        logger.LogInformation("Email message sent successfully! MessageId: {MessageId}", message.Id);
    }

    private EmailToBeSend BuildMessage(string recipient, string body)
    {
        return new EmailToBeSend
        {
            To = recipient,
            Subject = "Welcome",
            Body = body
        };
    }
}