using System;
using System.Linq.Expressions;
using AN.Integration.DynamicsCore.CoreTypes;
using AutoMapper;

namespace AN.Integration.Mapper.Extensions
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<EntityCore, TDestination> MapField<TDestination, TMember>(
            this IMappingExpression<EntityCore, TDestination> expression, string sourceName,
            Expression<Func<TDestination, TMember>> destinationMember)
        {
            return expression.ForMember(destinationMember,
                o => o.MapFrom(s => s.GetAttributeValue<object>(sourceName)));
        }

        public static IMappingExpression<EntityCore, TDestination> MapField<TDestination, TSourceMember,
            TDestMember>(this IMappingExpression<EntityCore, TDestination> expression,
            Expression<Func<EntityCore, TSourceMember>> sourceMember,
            Expression<Func<TDestination, TDestMember>> destinationMember)
        {
            return expression.ForMember(destinationMember, o => o.MapFrom(sourceMember));
        }
    }
}