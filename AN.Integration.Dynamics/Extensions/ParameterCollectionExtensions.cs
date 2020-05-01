using Microsoft.Xrm.Sdk;
using AN.Integration.DynamicsCore.CoreTypes;

namespace AN.Integration.Dynamics.Extensions
{
    public static class ParameterCollectionExtensions
    {
        public static ParameterCollectionCore ToCollectionCore(this ParameterCollection collection)
        {
            var collectionCore = new ParameterCollectionCore();

            foreach (var item in collection)
            {
                switch (item.Value)
                {
                    case Entity entity:
                        collectionCore.Add(item.Key, entity.ToEntityCore());
                        break;
                    case EntityReference reference:
                        collectionCore.Add(item.Key, new ReferenceCore(reference.LogicalName, reference.Id));
                        break;
                }
            }

            return collectionCore;
        }
    }
}