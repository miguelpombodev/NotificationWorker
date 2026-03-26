using System.Text;
using Newtonsoft.Json;
using NotificationWorker.Domain.Models.Base;

namespace NotificationWorker.Domain.Models.Emails;

public class EmailToBeSend : Message
{
    
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public byte[] ParseToBytes()
    {
        var json = JsonConvert.SerializeObject(this);
        var bytes = Encoding.UTF8.GetBytes(json);

        return bytes;
    }
}