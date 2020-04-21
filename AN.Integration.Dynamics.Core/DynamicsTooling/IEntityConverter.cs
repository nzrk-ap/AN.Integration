using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.DynamicsTooling
{
    internal interface IEntityConverter
    {
        string ToJSon(EntityCore target);
    }
}