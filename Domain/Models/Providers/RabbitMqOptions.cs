namespace NotificationWorker.Domain.Models.Providers;

public class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "admin";
    public string Password { get; set; } = "admin123";
    public string QueueName { get; set; } = "notification-worker";
    public string ExchangeName { get; set; } = "notification-worker-exchange";
    public string RoutingKeyName { get; set; } = "notification-worker-rk";
    public string EmailSenderHostName { get; set; } = "localhost";
    public string EmailSenderQueueName { get; set; } = "localhost";
    public int EmailSenderQueuePort { get; set; } = 5672;
    public string EmailSenderQueueUserName { get; set; } = "admin";
    public string EmailSenderQueuePassword { get; set; } = "admin123";
    public string EmailSenderQueueExchangeName { get; set; } = "sub-email-sender-exchange";
    public string EmailSenderQueueRoutingKeyName { get; set; } = "sub-email";
    public ushort PrefetchCount { get; set; } = 10;
}