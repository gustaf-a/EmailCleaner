namespace MailCollectorService.Data
{
    public static class MessageExtensions
    {
        public static List<Email> ToEmailList(this List<Google.Apis.Gmail.v1.Data.Message> values)
        {
            if (values is null)
                return new();

            return values.Select(m => new Email()
            {
                id = m.Id,
                labelIds = m.LabelIds,
                origin = "gmail",
                payload = m.Payload,
                snippet = m.Snippet,
                threadId = m.ThreadId
            }).ToList();
        }
    }
}
