using System;
using AutoMapper;
using AN.Integration.DynamicsCore.CoreTypes;

namespace AN.Integration.Mapper.Profiles
{
    public abstract class DynamicsCoreProfile : Profile
    {
        protected DynamicsCoreProfile()
        {
            CreateMap<object, Guid?>()
                .ConvertUsing((source, dest)=>
                {
                    return source switch
                    {
                        null => null,
                        Guid id => id,
                        ReferenceCore rc => rc.Id,
                        _ => throw new Exception($"Type {source.GetType().Name} is not supported"),
                    };
                });

            CreateMap<object, Guid?>().ConvertUsing((source, dest) =>
            {
                if (source is ReferenceCore rc) return rc.Id;
                return null;
            });

            CreateMap<object, Enum>().ConvertUsing(value =>
                (Enum)Enum.ToObject(typeof(TypeCode), ((OptionSetCore)value).Value)
            );
        }

        protected IMappingExpression<EntityCore, TDestination> CreateEntityMap<TDestination>()
        {
            return CreateMap<EntityCore, TDestination>();
        }
    }
}
