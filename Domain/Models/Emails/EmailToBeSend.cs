using NotificationWorker.Domain.Models.Base;

namespace NotificationWorker.Domain.Models.Emails;

public class EmailToBeSend : Message
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}