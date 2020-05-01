using System;

namespace AN.Integration.DynamicsCore.CoreTypes
{
   public interface IEntityCore
    {
        Guid Id { get; }

        string LogicalName { get; }
    }
}
