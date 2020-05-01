using System;
using AN.Integration.DynamicsCore.CoreTypes;

namespace AN.Integration.DynamicsCore.Extensions
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