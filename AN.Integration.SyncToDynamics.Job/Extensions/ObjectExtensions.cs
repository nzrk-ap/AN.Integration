using System;
using System.Linq;

namespace AN.Integration.SyncToDynamics.Job.Extensions
{
    internal static class ObjectExtensions
    {
        public static string GetTypeName(this object obj) => obj.GetType().Name;

        public static Type GetSingleGenericArgument(this object obj)
        {
            return obj.GetType().GenericTypeArguments.Single();
        }

        public static T GetFirstPropertyValue<T>(this object obj)
        {
            var value = obj.GetType().GetProperties()
                 .Select(p => p.GetValue(obj))
                 .SingleOrDefault(i => i.GetType() == typeof(T));

            return (T) value;
        }

        public static T GetFirstPropertyValue<T>(this object obj, string propertyName)
        {
            var value = obj.GetType().GetProperty(propertyName)
                ?.GetValue(obj);

            return (T) value;
        }

        public static object GetSinglePropertyValue<T>(this object obj)
        {
            var property = obj.GetType().GetProperties().Single();

            return property.GetValue(obj);
        }

        public static T GetFirstPropertyValueByInterface<T>(this object obj)
        {
            var value = obj.GetType().GetProperties()
                 .Select(p => p.GetValue(obj))
                 .SingleOrDefault(i => i != null && i.GetType()
                                           .GetInterfaces().FirstOrDefault() == typeof(T));

            return (T) value;
        }
    }
}