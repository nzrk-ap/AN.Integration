using System.Linq;
using AN.Integration.DynamicsCore.DynamicsTooling.Convert;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Api;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Conversion;
using Newtonsoft.Json;

namespace AN.Integration.Infrastructure.Dynamics.DynamicsTooling
{
    public class RequestConverter: IRequestConverter
    {
        private readonly IAttributeValue _attributeValue = new AttributeValue();

        public string ToJSon(ApiRequest request)
        {
            var values = request.BodyAttributes.ToDictionary(attribute => attribute.Key,
                attribute => _attributeValue.Convert(attribute.Value));

            return JsonConvert.SerializeObject(values);
        }
    }
}
