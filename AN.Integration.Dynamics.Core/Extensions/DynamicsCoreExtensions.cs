using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.Extensions
{
   public static class DynamicsCoreExtensions
    {
        public static EntityCore Merge(this EntityCore target, EntityCore source)
        {
            foreach (var attribute in source.Attributes)
            {
                target[attribute.Key] = source[attribute.Key];
            }

            return target;
        }
    }
}
