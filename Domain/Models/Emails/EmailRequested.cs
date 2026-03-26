namespace NotificationWorker.Domain.Models.Emails;

public class EmailRequested
{
    public string Recipient { get; set; }
    public string Body { get; set; }
}