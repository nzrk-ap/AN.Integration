using AN.Integration.Dynamics.Core.DynamicsTypes;
using Microsoft.Xrm.Sdk;

namespace AN.Integration.Dynamics.Core.Extensions
{
    public static class ParameterCollectionExtensions
    {
        public static ParameterCollectionCore ToCollectionCore(this ParameterCollection collection)
        {
            var collectionCore = new ParameterCollectionCore();

            foreach (var item in collection)
            {
                if (item.Value is Entity entity)
                {
                    collectionCore.Add(item.Key, entity.ToEntityCore());
                }
                else
                {
                    collection.Add(item.Key, item.Value);
                }
            }

            return collectionCore;
        }
    }
}