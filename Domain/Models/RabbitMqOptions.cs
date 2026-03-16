namespace NotificationWorker.Domain.Models;

public class RabbitMqOptions
{
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "admin";
    public string Password { get; set; } = "admin123";
    public string QueueName { get; set; } = "notification-worker";
    public string ExchangeName { get; set; } = "notification-worker-exchange";
    public string RoutingKeyName { get; set; } = "notification-worker-rk";
    public ushort PrefetchCount { get; set; } = 10;
}