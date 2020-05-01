using Microsoft.Xrm.Sdk;
using AN.Integration.DynamicsCore.CoreTypes;

namespace AN.Integration.Dynamics.Extensions
{
    public static class EntityImageCollectionExtensions
    {
        public static ImageCollectionCore ToCollectionCore(this EntityImageCollection collection)
        {
            var collectionCore = new ImageCollectionCore();

            foreach (var item in collection)
            {
                collectionCore.Add(item.Key, item.Value.ToEntityCore());
            }

            return collectionCore;
        }
    }
}