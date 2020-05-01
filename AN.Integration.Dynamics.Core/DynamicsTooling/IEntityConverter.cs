using AN.Integration.DynamicsCore.CoreTypes;

namespace AN.Integration.DynamicsCore.DynamicsTooling
{
    internal interface IEntityConverter
    {
        string ToJSon(EntityCore target);
    }
}