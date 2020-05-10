using System;
using System.Collections.Generic;

namespace AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Api
{
    public class ApiRequest
    {
        public ApiRequest(string entityName)
        {
            EntityName = entityName;
        }

        public ApiRequest(string entityName, Guid? recordId) : this(entityName)
        {
            RecordId = recordId;
        }

        public ApiRequest(string entityName, string keyName, string keyValue) : this(entityName)
        {
            KeyName = keyName;
            KeyValue = keyValue;
        }

        public ApiRequest(string entityName,
            IDictionary<string, object> bodyAttributes, Guid? recordId) :
            this(entityName, recordId)
        {
            BodyAttributes = bodyAttributes;
        }

        public ApiRequest(string entityName, string keyName, string keyValue,
            IDictionary<string, object> bodyAttributes) :
            this(entityName, keyName, keyValue)
        {
            BodyAttributes = bodyAttributes;
        }

        public string EntityName { get; }

        public Guid? RecordId { get; }

        public string KeyName { get; }

        public string KeyValue { get; }

        public IDictionary<string, object> BodyAttributes { get; set; }
    }
}