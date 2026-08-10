using System.Text;
using Newtonsoft.Json;
using NotificationWorker.Domain.Models.Base;

namespace NotificationWorker.Domain.Models.Emails;

public class EmailToBeSend : Message
{
    public string To { get; init; } = string.Empty;
    public IReadOnlyList<string> Cc { get; init; } = [];
    public IReadOnlyList<string> Bcc { get; init; } = [];
    public IReadOnlyList<Attachment> Attachments { get; set; } = [];
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;

    public byte[] ParseToBytes()
    {
        var json = JsonConvert.SerializeObject(this);
        var bytes = Encoding.UTF8.GetBytes(json);

        return bytes;
    }
}