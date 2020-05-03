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

        public ApiRequest(string entityName, Guid? recordId)
        {
            EntityName = entityName;
            RecordId = recordId;
        }

        public ApiRequest(string entityName,
            IDictionary<string, object> bodyAttributes, Guid? recordId) :
            this(entityName, recordId)
        {
            BodyAttributes = bodyAttributes;
        }

        public string EntityName { get; private set; }

        public Guid? RecordId { get; private set; }

        public IDictionary<string, object> BodyAttributes { get; set; }
    }
}