using System;
using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.Extensions
{
    public static class DynamicsContextCoreExtensions
    {
        public static EntityCore GetTargetEntity(this DynamicsContextCore context)
        {
            if (context.InputParameters.ContainsKey("Target"))
            {
                return (EntityCore)context.InputParameters["Target"];
            }

            throw new Exception($"{context.InputParameters} doesn't contain Target");
        }

        public static ReferenceCore GetTargetRef(this DynamicsContextCore context)
        {
            if (context.InputParameters.ContainsKey("Target"))
            {
                return context.InputParameters["Target"] is EntityCore
                    ? context.GetTargetEntity().ToEntityReference()
                    : (ReferenceCore)context.InputParameters["Target"];
            }

            throw new Exception($"{context.InputParameters} doesn't contain Target");
        }
    }
}