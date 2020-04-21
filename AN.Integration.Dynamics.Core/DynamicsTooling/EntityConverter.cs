using System.Linq;
using AN.Integration.Dynamics.Core.DynamicsTooling.Conversion;
using AN.Integration.Dynamics.Core.DynamicsTooling.Convert;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AN.Integration.Dynamics.Core.Utilities;

namespace AN.Integration.Dynamics.Core.DynamicsTooling
{
    public class EntityConverter: IEntityConverter
    {
        private readonly IAttributeValue _attributeValue = new AttributeValue();

        public string ToJSon(EntityCore target)
        {
            var values = target.Attributes.ToDictionary(attribute => attribute.Key,
                attribute => _attributeValue.Convert(attribute.Value));

            return DataSerializer.ToJson(values);
        }
    }
}
