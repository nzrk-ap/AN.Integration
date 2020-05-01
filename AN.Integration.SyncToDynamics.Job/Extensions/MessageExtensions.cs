using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace AN.Integration.SyncToDynamics.Job.Extensions
{
    internal static class MessageExtensions
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.All,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
        };

        public static T GetBody<T>(this Message message)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body), JsonSerializerSettings);
        }

        public static object GetBody(this Message message)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.Body), JsonSerializerSettings);
        }
    }
}