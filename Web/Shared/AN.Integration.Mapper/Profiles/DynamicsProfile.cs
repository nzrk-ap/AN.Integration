//using System;
//using System.Diagnostics;
//using AutoMapper;
//using Microsoft.Xrm.Sdk;

//namespace AN.Integration.Mapper.Profiles
//{
//    internal abstract class DynamicsProfile : Profile
//    {
//        protected DynamicsProfile()
//        {
//            //CreateMap<object, Guid?>().ConvertUsing(value => (value as EntityReference)?.Id);

//            // Maping to any enum from OptionSet and int 
//            // Use TypeCode instead of concrete enum
//            CreateMap<object, Enum>().ConvertUsing(value =>
//                (Enum)Enum.ToObject(typeof(TypeCode), ((OptionSetValue)value).Value)
//            );
//        }

//        protected IMappingExpression<Entity, TDestination> CreateRecordMap<TDestination>()
//        {
//            return CreateMap<Entity, TDestination>();
//        }
//    }
//}
