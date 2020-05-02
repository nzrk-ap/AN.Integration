using System;
using System.Collections.Generic;
using System.Linq;
using AN.Integration.DynamicsCore.Api;
using AN.Integration.OneC.Models;

namespace AN.Integration.SyncToDynamics.Job.Extensions
{
   internal static class MapperCollectionExtensions
    {
        public static Func<IOneCData, ApiRequest> GetMapper(
            this IDictionary<Type, Func<IOneCData, ApiRequest>> mappers, Type type)
        {
            var mapper = mappers.FirstOrDefault(m => m.Key == type).Value;

            return mapper ?? throw new Exception($"Mapper for type {type.Name} is not specified");
        }
    }
}
