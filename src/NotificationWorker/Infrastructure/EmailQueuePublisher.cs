using Microsoft.Extensions.Options;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Models.Emails;
using NotificationWorker.Domain.Models.Providers;
using RabbitMQ.Client;

namespace NotificationWorker.Infrastructure;

public class EmailQueuePublisher(IOptions<RabbitMqOptions> rabbitMqOptions, ILogger<EmailQueuePublisher> logger)
    : IEmailQueuePublisher
{
    private readonly RabbitMqOptions _rabbitMqOptions = rabbitMqOptions.Value;

    public async Task PublishAsync(EmailToBeSend email, CancellationToken ct)
    {
        logger.LogInformation(
            "Creating connection and channel with Email Worker Queue. Exchange: {EmailSenderExchange}, Routing Key: {EmailSenderRoutingKey}",
            _rabbitMqOptions.EmailSenderQueueExchangeName,
            _rabbitMqOptions.EmailSenderQueueRoutingKeyName
        );
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.EmailSenderHostName,
            Port = _rabbitMqOptions.EmailSenderQueuePort,
            UserName = _rabbitMqOptions.EmailSenderQueueUserName,
            Password = _rabbitMqOptions.EmailSenderQueuePassword
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        logger.LogInformation("Created Email Model with subject {MessageSubject} by ID: {MessageId}",
            email.Subject,
            email.Id
        );

        await channel.BasicPublishAsync(
            exchange: _rabbitMqOptions.EmailSenderQueueExchangeName,
            routingKey: _rabbitMqOptions.EmailSenderQueueRoutingKeyName,
            body: email.ParseToBytes()
        );

        logger.LogInformation("Email message sent successfully! MessageId: {MessageId}", email.Id);
    }
}