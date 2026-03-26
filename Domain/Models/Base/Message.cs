using NotificationWorker.Domain.Contracts;

namespace NotificationWorker.Domain.Models.Base;

public abstract class Message : IMessage
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
}