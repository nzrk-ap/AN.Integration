using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.Extensions
{
    public static class EntityExtensions
    {
        public static EntityCore ToEntityCore(this Entity entity)
        {
            var dynamicsEntity = new EntityCore(entity.LogicalName, entity.Id);

            foreach (var attribute in entity.Attributes)
            {
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
                OptionSetValue osv => osv.Value,
                EntityReference er => new ReferenceCore(er.LogicalName, er.Id),
                _ => throw new ArgumentException($"Type conversion for {attribute.Value.GetType().Name} is not supported")
            };

            return value;
        }
    }
}
