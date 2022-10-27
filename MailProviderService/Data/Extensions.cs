using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace MailProviderService.Data
{
    public static class Extensions
    {
        public static bool TryParseListOf<T>(this ReadOnlyMemory<byte> value, out List<T> parsedValues) 
        {
            parsedValues = new List<T>();

            if (value.IsEmpty)
                return true;

            var serializedString = Encoding.UTF8.GetString(value.ToArray());

            parsedValues = JsonConvert.DeserializeObject<List<T>>(serializedString);

            if (parsedValues is null)
            {
                Log.Error($"Failed to serialize string: {serializedString}");
                return false;
            }

            return true;
        }
    }
}
