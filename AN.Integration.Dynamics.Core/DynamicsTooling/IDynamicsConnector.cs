using System;
using System.Threading.Tasks;
using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.DynamicsTooling
{
    internal interface IDynamicsConnector
    {
        Task<Guid> CreateAsync(EntityCore target);

        Task UpdateAsync(EntityCore target);

        Task DeleteAsync(ReferenceCore target);
    }
}
