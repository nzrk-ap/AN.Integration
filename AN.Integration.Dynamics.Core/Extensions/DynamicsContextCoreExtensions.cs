using System;
using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.Extensions
{
    public static class DynamicsContextCoreExtensions
    {
        public static EntityCore GetTarget(this DynamicsContextCore context)
        {
            if (context.InputParameters.ContainsKey("Target"))
            {
                return (EntityCore) context.InputParameters["Target"];
            }

            throw new Exception($"{context.InputParameters} doesn't contain Target");
        }
    }
}
