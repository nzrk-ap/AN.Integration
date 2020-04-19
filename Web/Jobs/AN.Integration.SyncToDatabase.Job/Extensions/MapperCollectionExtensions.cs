using AN.Integration.Database.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AN.Integration.SyncToDatabase.Job.Extensions
{
   internal static class MapperCollectionExtensions
    {
        public static Func<IExtensibleDataObject, IDatabaseTable> GetMapper(
            this IDictionary<string, Func<IExtensibleDataObject, IDatabaseTable>> mappers, string entityName)
        {
            var mapper = mappers.FirstOrDefault(m => m.Key == entityName).Value;

            return mapper ?? throw new Exception($"Mapper for entity {entityName} is not specified");
        }
    }
}
