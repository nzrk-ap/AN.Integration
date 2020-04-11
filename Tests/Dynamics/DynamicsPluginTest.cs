using Microsoft.Xrm.Sdk;

namespace Tests.Dynamics
{
    public abstract class DynamicsPluginTest<T>: DynamicsTestBase<T> where T : IPlugin, new()
    {
       
    }
}