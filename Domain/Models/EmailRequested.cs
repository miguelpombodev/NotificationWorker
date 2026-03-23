namespace NotificationWorker.Domain.Models;

public class EmailRequested
{
    public string Recipient { get; set; }
    public string Body { get; set; }
}