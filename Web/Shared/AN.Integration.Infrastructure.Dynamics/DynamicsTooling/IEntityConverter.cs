using AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Api;

namespace AN.Integration.Infrastructure.Dynamics.DynamicsTooling
{
    public interface IRequestConverter
    {
        string ToJSon(ApiRequest target);
    }
}