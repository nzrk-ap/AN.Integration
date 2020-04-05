using AN.Integration.Dynamics.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace AN.Integration.Dynamics.EntityProviders
{
    public class Settings : SBCustomSettingsModel
    {
        public Settings(IOrganizationService service) : base(service) { }
        public Settings(IOrganizationService service, Guid id) : base(id, service) { }
        public Settings(Guid id, ColumnSet columnSet, IOrganizationService service)
                : base(service.Retrieve(LogicalName, id, columnSet), service) { }
        public Settings(Entity entity, IOrganizationService service) : base(entity, service) { }

        public Settings GetSettings(params string[] attributes)
        {
            var result = _service.RetrieveMultiple(new QueryExpression
            {
                EntityName = LogicalName,
                ColumnSet = new ColumnSet(attributes.Any() ? attributes : Fields.All),
                TopCount = 1
            });

            return new Settings(result.Entities.First(), _service);
        }
    }
}
