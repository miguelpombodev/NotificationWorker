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

    [Required(ErrorMessage = "RabbitMq:RoutingKeyName is required.")]
    [MinLength(1)]
    public string RoutingKeyName { get; set; } = "notification-worker-rk";
    
    [Required(ErrorMessage = "RabbitMq:EmailSenderHostName is required.")]
    [MinLength(1)]
    public string EmailSenderHostName { get; set; } = "localhost";

    [Required(ErrorMessage = "RabbitMq:EmailSenderQueueName is required.")]
    [MinLength(1)]
    public string EmailSenderQueueName { get; set; } = "sub-email-sender";

    [Range(1, 65535, ErrorMessage = "RabbitMq:EmailSenderQueuePort must be between 1 and 65535.")]
    public int EmailSenderQueuePort { get; set; } = 5672;

    [Required(ErrorMessage = "RabbitMq:EmailSenderQueueUserName is required.")]
    [MinLength(1)]
    public string EmailSenderQueueUserName { get; set; } = "admin";

    [Required(ErrorMessage = "RabbitMq:EmailSenderQueuePassword is required.")]
    [MinLength(1)]
    public string EmailSenderQueuePassword { get; set; } = "admin123";

    [Required(ErrorMessage = "RabbitMq:EmailSenderQueueExchangeName is required.")]
    [MinLength(1)]
    public string EmailSenderQueueExchangeName { get; set; } = "sub-email-sender-exchange";

    [Required(ErrorMessage = "RabbitMq:EmailSenderQueueRoutingKeyName is required.")]
    [MinLength(1)]
    public string EmailSenderQueueRoutingKeyName { get; set; } = "sub-email";
    
    [Range(1, 1000, ErrorMessage = "RabbitMq:PrefetchCount must be between 1 and 1000.")]
    public ushort PrefetchCount { get; set; } = 10;
}