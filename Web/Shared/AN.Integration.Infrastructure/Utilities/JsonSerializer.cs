using Newtonsoft.Json;

namespace AN.Integration.Infrastructure.Utilities
{
    public static class JsonSerializer
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.All,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
        };

        public static string ToJson<T>(T value) =>
            JsonConvert.SerializeObject(value, JsonSerializerSettings);

        public static T ToObject<T>(string json) =>
            JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);

        public static object ToObject(string json) =>
            JsonConvert.DeserializeObject(json, JsonSerializerSettings);
    }
}
