using System;
using System.Threading.Tasks;
using AN.Integration.DynamicsCore.CoreTypes;

namespace AN.Integration.DynamicsCore.DynamicsTooling
{
    internal interface IDynamicsConnector
    {
        Task<Guid> CreateAsync(EntityCore target);

        Task UpdateAsync(EntityCore target);

        Task DeleteAsync(ReferenceCore target);
    }
}
