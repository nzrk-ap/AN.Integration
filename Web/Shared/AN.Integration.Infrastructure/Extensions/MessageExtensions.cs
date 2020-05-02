using System.Text;
using Microsoft.Azure.ServiceBus;
using AN.Integration.Infrastructure.Utilities;

namespace AN.Integration.Infrastructure.Extensions
{
    public static class MessageExtensions
    {
        public static T GetBody<T>(this Message message)
        {
            return JsonSerializer.ToObject<T>(Encoding.UTF8.GetString(message.Body));
        }

        public static object GetBody(this Message message)
        {
            return JsonSerializer.ToObject(Encoding.UTF8.GetString(message.Body));
        }
    }
}