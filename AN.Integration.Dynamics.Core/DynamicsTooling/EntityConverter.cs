using System.Linq;
using AN.Integration.DynamicsCore.Api;
using AN.Integration.DynamicsCore.DynamicsTooling.Conversion;
using AN.Integration.DynamicsCore.DynamicsTooling.Convert;
using AN.Integration.DynamicsCore.Utilities;

namespace AN.Integration.DynamicsCore.DynamicsTooling
{
    public class RequestConverter: IRequestConverter
    {
        private readonly IAttributeValue _attributeValue = new AttributeValue();

        public string ToJSon(ApiRequest request)
        {
            var values = request.BodyAttributes.ToDictionary(attribute => attribute.Key,
                attribute => _attributeValue.Convert(attribute.Value));

            return DataSerializer.ToJson(values);
        }
    }
}
