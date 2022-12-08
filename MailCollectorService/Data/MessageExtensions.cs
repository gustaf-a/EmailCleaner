using Newtonsoft.Json;

namespace MailCollectorService.Data;

public static class MessageExtensions
{
    public static List<Email> ToEmailList(this List<Google.Apis.Gmail.v1.Data.Message> values)
    {
        if (values is null)
            return new();

        return values.Select(m => m.ToEmail()).ToList();
    }

    public static Email ToEmail(this Google.Apis.Gmail.v1.Data.Message value)
    {
        if (value is null)
            return new();

        return new Email()
        {
            Id = value.Id,
            LabelIds = JsonConvert.SerializeObject(value.LabelIds),
            Origin = "gmail",
            Payload = JsonConvert.SerializeObject(value.Payload),
            Snippet = value.Snippet,
            ThreadId = value.ThreadId
        };
    }
}
