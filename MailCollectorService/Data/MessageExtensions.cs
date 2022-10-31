using Newtonsoft.Json;

namespace MailCollectorService.Data;

public static class MessageExtensions
{
    public static List<Email> ToEmailList(this List<Google.Apis.Gmail.v1.Data.Message> values)
    {
        if (values is null)
            return new();

        return values.Select(m => new Email()
        {
            Id = m.Id,
            LabelIds = JsonConvert.SerializeObject(m.LabelIds),
            Origin = "gmail",
            Payload = JsonConvert.SerializeObject(m.Payload),
            Snippet = m.Snippet,
            ThreadId = m.ThreadId
        }).ToList();
    }
}
