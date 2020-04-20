using AN.Integration.Database.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.SyncToDatabase.Job.Extensions
{
   internal static class MapperCollectionExtensions
    {
        public static Func<IEntityCore, IDatabaseTable> GetMapper(
            this IDictionary<string, Func<IEntityCore, IDatabaseTable>> mappers, string entityName)
        {
            var mapper = mappers.FirstOrDefault(m => m.Key == entityName).Value;

            return mapper ?? throw new Exception($"Mapper for entity {entityName} is not specified");
        }
    }
}
