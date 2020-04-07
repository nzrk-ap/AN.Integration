using System;
using AN.Integration.Models.Dynamics;
using Microsoft.Xrm.Sdk;

namespace AN.Integration.Dynamics.Extensions
{
    public static class DynamicsContextExtensions
    {
        public static Entity GetTarget(this DynamicsContext context)
        {
            const string keyName = "Target";
            if (context.InputParameters.ContainsKey(keyName))
            {
                return (Entity) context.InputParameters[keyName];
            }

            throw new Exception($"{nameof(DynamicsContext)} doesn't contain {keyName}");
        }
    }
}