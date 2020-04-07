using Newtonsoft.Json;

namespace AN.Integration.Infrastructure.Utilities
{
    public static class JsonSerializer
    {
        public static string ToJson<T>(T value) =>
            JsonConvert.SerializeObject(value);

        public static T ToObject<T>(string json) =>
            JsonConvert.DeserializeObject<T>(json);
    }
}
