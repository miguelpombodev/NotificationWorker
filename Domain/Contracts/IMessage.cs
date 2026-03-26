namespace NotificationWorker.Domain.Contracts;

public interface IMessage
{
    Guid Id { get; set; }
}