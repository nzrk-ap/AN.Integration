using System;
using AN.Integration.Dynamics.Core.DynamicsTooling.Convert;
using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.DynamicsTooling.Conversion
{
    internal class AttributeValue: IAttributeValue
    {
        public object Convert(object value)
        {
            return value switch
            {
                null => null,
                string s => s,
                int i => i,
                float f => f,
                decimal d => d,
                DateTime dt => dt,
                OptionSetCore opc => opc.Value,
                ReferenceCore rc => rc.ToString(),
                _ => throw new ArgumentException($"Type {value.GetType().Name} is not supported")
            };
        }
    }
}