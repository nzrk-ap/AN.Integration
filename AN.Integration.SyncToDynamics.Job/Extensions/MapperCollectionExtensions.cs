using System;
using System.Collections.Generic;
using System.Linq;
using AN.Integration._1C.Models;
using AN.Integration.DynamicsCore.Api;

namespace AN.Integration.SyncToDynamics.Job.Extensions
{
   internal static class MapperCollectionExtensions
    {
        public static Func<IOneCData, ApiRequest> GetMapper(
            this IDictionary<Type, Func<IOneCData, ApiRequest>> mappers, IOneCData _1CData)
        {
            var mapper = mappers.FirstOrDefault(m => m.Key == _1CData.GetType()).Value;

            return mapper ?? throw new Exception($"Mapper for type {_1CData.GetType().Name} is not specified");
        }
    }
}
