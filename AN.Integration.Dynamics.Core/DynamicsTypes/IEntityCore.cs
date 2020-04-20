using System;

namespace AN.Integration.Dynamics.Core.DynamicsTypes
{
   public interface IEntityCore
    {
        Guid Id { get; }

        string LogicalName { get; }
    }
}
