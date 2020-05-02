using AN.Integration.DynamicsCore.Api;

namespace AN.Integration.DynamicsCore.DynamicsTooling
{
    public interface IRequestConverter
    {
        string ToJSon(ApiRequest target);
    }
}