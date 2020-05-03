using System.Threading.Tasks;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Api;

namespace AN.Integration.Infrastructure.Dynamics.DynamicsTooling
{
    public interface IDynamicsConnector
    {
        Task UpsertAsync(ApiRequest request);

        Task DeleteAsync(ApiRequest request);
    }
}