using AN.Integration.Dynamics.Core.DynamicsTypes;
using Microsoft.Xrm.Sdk;

namespace AN.Integration.Dynamics.Extensions
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
                    collectionCore.Add(item.Key, item.Value);
                }
            }

            return collectionCore;
        }
    }
}