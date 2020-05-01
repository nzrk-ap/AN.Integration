using System.Collections.Generic;

namespace AN.Integration.DynamicsCore.Api
{
    public class ApiRequest
    {
        public ApiRequest(string entityName)
        {
            EntityName = entityName;
        }

        public ApiRequest(string entityName, 
            IDictionary<string, object> bodyAttributes) : this(entityName)
        {
            BodyAttributes = bodyAttributes;
        }

        public string EntityName { get; private set; }

        public IDictionary<string, object> BodyAttributes { get; set; }

        public RequestType Type { get; set; }

        public enum RequestType 
        {
            Post = 1,
            Patch,
            Delete
        }
    }
}
