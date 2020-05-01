using AN.Integration.Dynamics.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using AN.Integration.DynamicsCore.CoreTypes;

namespace AN.Integration.Dynamics.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Get type specified value from aliased value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static T GetAliasedValue<T>(this Entity entity, string attributeName)
        {
            if (!entity.Attributes.Contains(attributeName))
                return default(T);

            var type = entity.Attributes[attributeName].GetType();

            if (type == typeof(AliasedValue))
            {
                var value = entity.GetAttributeValue<AliasedValue>(attributeName);
                return (value is null) ? default(T) : (T)value.Value;
            }
            else
            {
                var value = entity.GetAttributeValue<T>(attributeName);
                return (value == null) ? default(T) : value;
            }
        }
        /// <summary>
        /// Create new Instance Entity Base
        /// </summary>
        /// <typeparam name="T">Entity Base</typeparam>
        /// <param name="entity">Entity CRM</param>
        /// <param name="service">IOrganizationService</param>
        /// <returns></returns>
        public static T ToEntity<T>(this Entity entity, IOrganizationService service) where T : EntityBase
        {
            return (T)Activator.CreateInstance(typeof(T), entity, service);
        }

        /// <summary>
        /// Set attribute if value is not null
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="attributeName"></param>
        public static void SetAttribute(this Entity target, Entity source, string attributeName)
        {
            var value = source.GetAttributeValue<object>(attributeName);

            if (value is null) return;

            target[attributeName] = value;
        }

        /// <summary>
        /// Return int attribute value
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static int? GetOptionSetValue(this Entity entity, string attributeName) =>
            entity.GetAttributeValue<OptionSetValue>(attributeName)?.Value;

        /// <summary>
        /// Return EntityRefrence attribute value
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static EntityReference GetReference(this Entity entity, string attributeName) =>
            entity.GetAttributeValue<EntityReference>(attributeName);

        /// <summary>
        /// Return boolean attribute value
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool GetBool(this Entity entity, string attributeName) =>
            entity.GetAttributeValue<bool>(attributeName);

        /// <summary>
        /// Return string attribute value
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetText(this Entity entity, string attributeName) =>
            entity.GetAttributeValue<string>(attributeName);

        /// <summary>
        /// Return decimal attribute value
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static decimal GetDecimal(this Entity entity, string attributeName) =>
            entity.GetAttributeValue<decimal>(attributeName);

        /// <summary>
        /// Return int attribute value
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static int GetInt(this Entity entity, string attributeName) =>
            entity.GetAttributeValue<int>(attributeName);

        /// <summary>
        /// Get DateTime attribute value
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this Entity entity, string attributeName) =>
            entity.GetAttributeValue<DateTime>(attributeName);

        /// <summary>
        /// Copy attributes from source to target if not exist
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Entity Merge(this Entity target, Entity source)
        {
            foreach (var attribute in source.Attributes)
            {
                target[attribute.Key] = source[attribute.Key];
            }

            return target;
        }

        public static EntityCore ToEntityCore(this Entity entity)
        {
            var dynamicsEntity = new EntityCore(entity.LogicalName, entity.Id);

            foreach (var attribute in entity.Attributes)
            {
                if (attribute.Value is null) continue;

                dynamicsEntity.Attributes.Add(attribute.Key, ConvertAttributeValue(attribute));
            }

            return dynamicsEntity;
        }

        private static object ConvertAttributeValue(KeyValuePair<string, object> attribute)
        {
            object value = attribute.Value switch
            {
                string s => s,
                int i => i,
                float f => f,
                decimal d => d,
                Guid g => g,
                DateTime dt => dt,
                Money m => m.Value,
                OptionSetValue osv => new OptionSetCore(osv.Value),
                EntityReference er => new ReferenceCore(er.LogicalName, er.Id),
                _ => throw new ArgumentException($"Type conversion for {attribute.Value.GetType().Name} is not supported")
            };

            return value;
        }
    }
}
