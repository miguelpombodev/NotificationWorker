using NotificationWorker.Application.Contracts;
using NotificationWorker.Domain.Enums;
using NotificationWorker.Domain.Models;
using NotificationWorker.Domain.Models.Emails;
using NotificationWorker.Infrastructure.Templates.TemplatesModels;

namespace NotificationWorker.Application.Services;

public class EmailHandler(
    IEmailDispatcher dispatcher,
    ITemplateRenderer templateRenderer,
    ILogger<EmailHandler> logger)
    : INotificationHandler
{
    public NotificationChannel Channel => NotificationChannel.Email;


    public async Task HandleAsync(NotificationRequested notification)
    {
        var model = MapTemplate(notification);
        var body = await templateRenderer.RenderAsync(notification.Project, notification.Template,
            model.TemplateModel);

        var emailBuilt = BuildMessage(notification, body);

        await dispatcher.SendAsync(emailBuilt);
    }

    private EmailToBeSend BuildMessage(NotificationRequested notification, string body)
    {
        var subject = notification.Data["subject"].ToString();
        if (string.IsNullOrWhiteSpace(subject))
        {
            logger.LogError(
                "[ERROR] There was an attempt to send a email without subject! Template requested: {Template}, Project requested: {Project}",
                notification.Template,
                notification.Project);

            throw new InvalidOperationException("Email subject cannot be nullable or whitespace! Please check it!");
        }

        var attachments = ParseAttachment(notification.Data["attachments"]);
        var cc = ParseStringList(notification.Data["cc"].ToString());
        var bcc = ParseStringList(notification.Data["bcc"].ToString());


        return new EmailToBeSend
        {
            To = [notification.Recipient],
            Cc = cc,
            Bcc = bcc,
            Attachments = attachments,
            Subject = subject,
            Body = body
        };
    }

    private IReadOnlyList<string> ParseStringList(object? value)
    {
        return value switch
        {
            IEnumerable<object> list => list.Select(x => x.ToString() ?? string.Empty)
                .Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),

            string str when !string.IsNullOrWhiteSpace(str) => [str],

            _ => new List<string> { }
        };
    }

    private IReadOnlyList<Attachment> ParseAttachment(object? value)
    {
        if (value is not IEnumerable<object> items)
        {
            return [];
        }

        var result = new List<Attachment>();

        foreach (var item in items)
        {
            if (item is not IDictionary<string, object> dict)
                continue;

            dict.TryGetValue("fileName", out var fileNameObj);
            dict.TryGetValue("content", out var contentObj);
            dict.TryGetValue("contentType", out var contentTypeObj);

            var fileName = fileNameObj?.ToString();
            var contentBase64 = contentObj?.ToString();
            var contentType = contentTypeObj?.ToString() ?? "application/octet-stream";

            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(contentBase64))
                continue;

            try
            {
                var bytes = Convert.FromBase64String(contentBase64);
                result.Add(new Attachment(fileName, bytes, contentType));
            }
            catch (FormatException exc)
            {
                logger.LogError(
                    "[ERROR] Error trying to convert ContentBase64 in bytes! Exception Message: {Message}; Exception StackTrace: {StackTrace}",
                    exc.Message, exc.StackTrace);
            }
        }

        return result;
    }

    private EmailPayload MapTemplate(NotificationRequested notification)
    {
        return notification.Template switch
        {
            "welcome" => new EmailPayload(
                Subject: notification.Data["subject"].ToString() ?? string.Empty,
                TemplateName: notification.Template,
                TemplateModel: new WelcomeTemplateModel
                {
                    Name = notification.Data["name"]?.ToString() ?? string.Empty,
                    Email = notification.Recipient,
                    LoginUrl = notification.Data["loginUrl"]?.ToString() ?? string.Empty,
                    Role = notification.Data["role"]?.ToString() ?? string.Empty,
                }),
            _ => throw new InvalidOperationException("Invalid template")
        };
    }
}