using System;
using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.Extensions
{
    public static class DynamicsCoreExtensions
    {
        public static EntityCore Merge(this EntityCore target, EntityCore source)
        {
            foreach (var attribute in source.Attributes)
            {
                target[attribute.Key] = source[attribute.Key];
            }

            return target;
        }

        public static string GetText(this EntityCore entity, string attributeName) =>
            entity.GetAttributeValue<string>(attributeName);

        public static int GetInt(this EntityCore entity, string attributeName) =>
            entity.GetAttributeValue<int>(attributeName);

        public static DateTime GetDateTime(this EntityCore entity, string attributeName) =>
            entity.GetAttributeValue<DateTime>(attributeName);
    }
}