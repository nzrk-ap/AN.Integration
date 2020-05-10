using System.Linq;

namespace AN.Integration.SyncToDynamics.Job.Extensions
{
    internal static class ObjectExtensions
    {
        public static T GetFirstPropertyValue<T>(this object obj)
        {
            var value = obj.GetType().GetProperties()
                 .Select(p => p.GetValue(obj))
                 .SingleOrDefault(i => i.GetType() == typeof(T));

            return (value is T typedValue) ? typedValue : default;
        }

        public static T GetFirstPropertyValueByInterface<T>(this object obj)
        {
            var value = obj.GetType().GetProperties()
                 .Select(p => p.GetValue(obj))
                 .SingleOrDefault(i => i != null && i.GetType()
                                           .GetInterfaces().FirstOrDefault() == typeof(T));

            return (value is T typedValue) ? typedValue : default;
        }
    }
}