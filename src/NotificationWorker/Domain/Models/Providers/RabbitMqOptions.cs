using System.ComponentModel.DataAnnotations;

namespace NotificationWorker.Domain.Models.Providers;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";
    
    [Required(ErrorMessage = "RabbitMq:HostName is required.")]
    [MinLength(1)]
    public string HostName { get; set; } = "localhost";

    [Range(1, 65535, ErrorMessage = "RabbitMq:Port must be between 1 and 65535.")]
    public int Port { get; set; } = 5672;

    [Required(ErrorMessage = "RabbitMq:UserName is required.")]
    [MinLength(1)]
    public string UserName { get; set; } = "admin";

    [Required(ErrorMessage = "RabbitMq:Password is required.")]
    [MinLength(1)]
    public string Password { get; set; } = "admin123";
    
    [Required(ErrorMessage = "RabbitMq:QueueName is required.")]
    [MinLength(1)]
    public string QueueName { get; set; } = "notification-worker";

    [Required(ErrorMessage = "RabbitMq:ExchangeName is required.")]
    [MinLength(1)]
    public string ExchangeName { get; set; } = "notification-worker-exchange";
    
    [Required(ErrorMessage = "RabbitMq:VirtualHost is required.")]
    [MinLength(1)]
    public string VirtualHost { get; set; } = "/";

    [Required(ErrorMessage = "RabbitMq:RoutingKeyName is required.")]
    [MinLength(1)]
    public string RoutingKeyName { get; set; } = "notification-worker-rk";
    
    
    [Range(1, 1000, ErrorMessage = "RabbitMq:PrefetchCount must be between 1 and 1000.")]
    public ushort PrefetchCount { get; set; } = 10;
}