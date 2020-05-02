using System;
using System.Threading.Tasks;
using AN.Integration.DynamicsCore.Api;

namespace AN.Integration.DynamicsCore.DynamicsTooling
{
    public interface IDynamicsConnector
    {
        Task<Guid> UpsertAsync(ApiRequest request);

        Task DeleteAsync(ApiRequest request);
    }
}