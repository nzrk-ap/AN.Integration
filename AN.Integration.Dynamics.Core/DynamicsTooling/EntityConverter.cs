using System.Linq;
using AN.Integration.DynamicsCore.CoreTypes;
using AN.Integration.DynamicsCore.DynamicsTooling.Conversion;
using AN.Integration.DynamicsCore.DynamicsTooling.Convert;
using AN.Integration.DynamicsCore.Utilities;

namespace AN.Integration.DynamicsCore.DynamicsTooling
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
